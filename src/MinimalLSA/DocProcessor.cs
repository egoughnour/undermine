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
		bool doPreprocess;
		Func<string,string> PreprocessLine;

		public DocProcessor (Func<string, string> preprocessor = null)
		{
			doPreprocess = preprocessor != null;
			if (doPreprocess) {
				PreprocessLine = preprocessor;
			}
			Terms = new GuidSet ();
			Documents = new GuidSet ();
		}

		public void UpdateFromFile(string path, TermGenerator generator)
		{
			string[] fileLines = File.ReadAllLines (path);

			for(int i=0; i < fileLines.Length; i++)
			{
				if (doPreprocess) {
					fileLines [i] = PreprocessLine (fileLines [i]);
				}
				ProcessLine (path, fileLines [i], i, generator);
			}
		}

		void ProcessLine (string path, string LineText, int lineNumber, TermGenerator generator)
		{
			TermRelationship term;
			Document doc;
			LineCollection lines;
			bool termExists = false;

			string docName = Path.GetFileName (path);

			int termIndex = 0;
			foreach (string word in generator.GetTerms(LineText))
			{
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