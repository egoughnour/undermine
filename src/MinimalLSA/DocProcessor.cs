using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using Porter;


namespace MinimalLSA
{

	public class DocProcessor
	{
		public GuidSet Terms;
		public GuidSet Documents;
		Regex TokenPattern;
		Regex WordSplitPattern;
		StemmerWrapper Stemmer;

		public List<string> StopWords { get; set; }


		public DocProcessor ()
		{
			Terms = new GuidSet ();
			Documents = new GuidSet ();
			TokenPattern = new Regex(@"[\s]|(?=\p{P})");
			WordSplitPattern = new Regex (@"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])");
			Stemmer = new StemmerWrapper ();
			StopWords = new List<string> ();
		}

		public void UpdateFromFile(string path)
		{
			string[] fileLines = File.ReadAllLines (path);

			for(int i=0; i < fileLines.Length; i++)
			{
				ProcessLine (path, fileLines [i], i);
			}
		}

		void ProcessLine (string path, string LineText, int lineNumber)
		{
			TermRelationship term;
			Document doc;
			LineCollection lines;
			bool termExists = false;

			string docName = Path.GetFileName (path);

			int termIndex = 0;
			foreach (string token in TokenPattern.Split (LineText)) {
				string[] stems;
				if (TryGetStemsFromToken (token, out stems)) {
					foreach (string word in stems) {
						var line = new Line {
							Id = Guid.NewGuid (),
							Value = LineText,
							Number = lineNumber,
							TermIndex = termIndex
						};

						if (Terms.Contains (word)) {
							term = Terms [word] as TermRelationship;
							termExists = true;
						} else {
							term = new TermRelationship {
								Id = Guid.NewGuid (),
								Value = word,
								Documents = new DocumentCollection ()
							};
							Terms.AddResource (term);
							term.Documents.Documents = new Document[] { };
						}
						if (termExists && term.Documents.Documents.Any (d => d.Value == docName)) {
							doc = term.Documents.Documents.First (d => d.Value == docName);
							lines = doc.Lines;
							if (!Documents.Contains (docName)) {
								Documents.AddResource (doc);
							}
						} else {
							doc = new Document {
								Id = Guid.NewGuid (),
								Value = docName,
								Path = path,
								Lines = new LineCollection ()
							};

							if (!Documents.Contains (docName)) {
								Documents.AddResource (doc);
							}

							lines = doc.Lines;
							lines.Lines = new Line[] { };

							var docs = term.Documents.Documents.ToList ();
							docs.Add (doc);
							term.Documents.Documents = docs.ToArray ();
						}

						var lineList = lines.Lines.ToList ();
						lineList.Add (line);
						lines.Lines = lineList.ToArray ();
						termExists = false;
						termIndex++;
					}

				}
			}
		}

		bool TryGetStemsFromToken (string token, out string[] stems)
		{
			stems = null;
			try
			{
				var spaceSeparated = WordSplitPattern.Replace (token, @" $1$2");
				using(var memStream = new MemoryStream(Encoding.ASCII.GetBytes(spaceSeparated)))
				{
					memStream.Position = 0;
					Stemmer.StopWords = StopWords;
					if (Stemmer.TryProcessStream (memStream)) {
						stems = Stemmer.Stems.ToArray ();
					}
					else
					{
						return false;
					}

				}
			}
			catch 
			{
				return false;
			}
			return true;
		}


	}
}