using NUnit.Framework;
using System;
using System.Xml.Serialization;
using MinimalLSA;
using System.IO;

namespace MinimalLSA.Test
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TermSerializationTest()
		{
			var term = new TermRelationship ();

			term.Id = Guid.NewGuid ();
			term.Value = "variableName";
			term.Documents = new DocumentCollection ();
			term.Documents.Documents = new Document[]
			{
				new Document
				{
					Id = Guid.NewGuid(),
					Value = "ClassDefn.cs",
					Path = string.Format("AnyOldPath{0}ClassDefn.cs"
						, Path.DirectorySeparatorChar),
					Lines = new LineCollection()
				}
			};
			var doc = term.Documents.Documents [0];

			doc.Lines.Lines = new Line[] { 
				new Line (){ Number = 16, Id = Guid.NewGuid(), TermIndex = 1 },
				new Line (){ Number = 34, Id = Guid.NewGuid(), TermIndex = 3 }
			};

			string tempPath = string.Format ("tmp{0}tmp.xml", Path.DirectorySeparatorChar);
			Directory.CreateDirectory ("tmp");

			TermRelationship newTerm;

			var serializer = new XmlSerializer (typeof(TermRelationship));

			using (StreamWriter sw = new StreamWriter(tempPath))
			{
				serializer.Serialize (sw, term);
			}
			using (StreamReader sr = new StreamReader (tempPath))
			{
				newTerm = serializer.Deserialize (sr) as TermRelationship;
			}

			Assert.AreEqual(term.Documents.Documents[0].Lines.Lines[1].Number,
				newTerm.Documents.Documents[0].Lines.Lines[1].Number);
		}
	}
}

