using System;
using System.IO;
using System.Linq;
using DotNetMatrix;
using System.Collections.Generic;

namespace MinimalLSA
{
	public class MathHandler
	{
		private double _rankFactor;

		DocProcessor TermExtractor;

		public List<string> Paths { get; set; }

		public double RankScaleFactor 
		{ 
			get
			{
				return _rankFactor;
			}

			set
			{
				_rankFactor = (value >  1.0d || value < 0.0d)? 0.7d : value;
			}
		}

		TermGenerator Generator;

		public MathHandler (string directoryPath, string extension)
		{
			RankScaleFactor = 0.7d;
			var directory = new DirectoryInfo (directoryPath);

			extension = extension.StartsWith(".")? extension : "." + extension;
			var fileGlob = "*" + extension;

			Paths = directory.GetFileSystemInfos (fileGlob, SearchOption.AllDirectories)
				.Select (f => f.FullName).ToList ();

			TermExtractor = new DocProcessor ();
			Generator = new TermGenerator (new List<string>());
		}


		public bool TryOrderDocumentsByQuery(OutputDataSet data, string query, out string[] documents)
		{
			documents = null;
			var queryTerms = Generator.GetTerms (query);
			var queryVector = new GeneralVector (data.Terms.Count);

			queryVector.InitToZero ();

			foreach (string term in queryTerms) 
			{
				if (data.Terms.Contains (term)) 
				{
					queryVector [data.Terms [term].RowColumnIndex] += 1;
				}
			}

			double[] theta;

			queryVector.RescaleToUnitInterval ();
			if (queryVector.ToList.Max() > 0) 
			{
				theta = new double[data.Documents.Count];

				var queryUk = queryVector.Multiply (data.Uk);
				var q = queryUk.Multiply (data.SkInverse);

				foreach (Document document in data.Documents.GetIndexedResources()
					.Select(r => r as Document)) 
				{
					var docVector = new GeneralMatrix (data.Terms.Count, 1);
					for (int k = 0; k < data.Terms.Count; k++) 
					{
						docVector .Array[k] [0] = data.TermDoc.Array [k] [document.RowColumnIndex];
					}

					var docTranspose = docVector.Transpose ();
					var transposeUk = docTranspose.Multiply (data.Uk);
					var d = transposeUk.Multiply (data.SkInverse);

					theta [document.RowColumnIndex] = 0;
					theta [document.RowColumnIndex] = new GeneralVector (q.Array [0])
						.UnitaryDotProduct (new GeneralVector (d.Array [0]));
				}
				var rankedDocs = theta.Select((rank, i) =>
					{
						var documentName = data.Documents.GetIndexedResources()
							.Where(r => r.RowColumnIndex == i).First().Value;
						return new KeyValuePair<double, string>(rank, documentName);
					});
				documents = rankedDocs
					.OrderBy (kvp => kvp.Key)
					.Select (p => p.Value)
					.ToArray ();
			} 
			else
			{
				return false;
			}

			return true;
		}

		public OutputDataSet Index()
		{
			foreach (string path in Paths)
			{
				TermExtractor.UpdateFromFile (path, Generator);
			}

			var localCoefficients = new GeneralMatrix (TermExtractor.Terms.Count, TermExtractor.Documents.Count);

			var rows = TermExtractor.Terms.GetIndexedResources ();
			var columns = TermExtractor.Documents.GetIndexedResources ();

			AssignLocalWeights (localCoefficients, rows, columns);



			double[] termWeights = new double[rows.Count];
			double[] docWeights = new double[columns.Count];

			AssignTermWeights (localCoefficients, rows, termWeights);

			AssignDocWeights (localCoefficients, termWeights, docWeights);



			var termDoc = new GeneralMatrix (rows.Count, columns.Count);

			SetElementwiseProduct (localCoefficients, rows.Count, columns.Count, termWeights, docWeights, termDoc);



			var decomposition = new SingularValueDecomposition (termDoc);

			int rank = columns.Count * RankScaleFactor > 0.0d
				? (int)(columns.Count * RankScaleFactor)
				: columns.Count;

			var sk = new GeneralMatrix (rank, rank);

			SetValuesFromDiagonalMatrix (decomposition, rank, sk);

			var inv = sk.Inverse();

			var u = decomposition.GetU ();

			var uk = new GeneralMatrix (u.Array.Length, rank);
			AssignReduced (rank, u, uk);

			var outputData = new OutputDataSet {
				Uk = uk,
				SkInverse = inv,
				TermDoc = termDoc,
				Terms = TermExtractor.Terms,
				Documents = TermExtractor.Documents
			};

			return outputData;
		}



		static void AssignLocalWeights (GeneralMatrix localCoefficients, List<IUniqueResource> rows, List<IUniqueResource> columns)
		{
			foreach (var row in rows) {
				foreach (var column in columns) {
					localCoefficients.Array [row.RowColumnIndex] [column.RowColumnIndex] = Count ((TermRelationship)row, (Document)column);
				}
			}
		}

		static void AssignTermWeights (GeneralMatrix localCoefficients, List<IUniqueResource> rows, double[] termWeights)
		{
			for (int i = 0; i < localCoefficients.Array.Length; i++) {
				int sum = 0;
				for (int j = 0; j < localCoefficients.Array [i].Length; j++) {
					if (localCoefficients.Array [i] [j] > 0) {
						sum += 1;
					}
				}
				if (sum > 0) {
					termWeights [i] = Math.Log (rows.Count / (double)sum);
				}
				else {
					termWeights [i] = 0;
				}
			}
		}

		static void AssignDocWeights (GeneralMatrix localCoefficients, double[] termWeights, double[] docWeights)
		{
			for (int j = 0; j < localCoefficients.Array [0].Length; j++) {
				double sum = 0;
				for (int i = 0; i < localCoefficients.Array.Length; i++) {
					sum += (termWeights [i] * localCoefficients.Array [i] [j]) * (termWeights [i] * localCoefficients.Array [i] [j]);
				}
				docWeights [j] = 1 / Math.Sqrt (sum);
			}
		}

		static void SetElementwiseProduct (GeneralMatrix localCoefficients, int rowCount, int columnCount, double[] termWeights, double[] docWeights, GeneralMatrix termDoc)
		{
			for (int i = 0; i < rowCount; i++) {
				for (int j = 0; j < columnCount; j++) {
					termDoc.Array [i] [j] = localCoefficients.Array [i] [j] * termWeights [i] * docWeights [j];
				}
			}
		}

		static void SetValuesFromDiagonalMatrix (SingularValueDecomposition decomposition, int rank, GeneralMatrix sk)
		{
			for (int i = 0; i < rank; i++) {
				for (int j = 0; j < rank; j++) {
					sk.Array [i] [j] = decomposition.S.Array [i] [j];
				}
			}
		}

		static void AssignReduced (int rank, GeneralMatrix u, GeneralMatrix uk)
		{
			for (int i = 0; i < u.Array.Length; i++) {
				for (int j = 0; j < rank; j++) {
					uk.Array [i] [j] = u.Array [i] [j];
				}
			}
		}

		public static int Count(TermRelationship term, Document doc)
		{
			return term.Documents.Documents.Sum (d => d.Id != doc.Id ? 0 : d.Lines.Lines.Count ()); 
		}
	}
}

