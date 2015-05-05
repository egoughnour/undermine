using NUnit.Framework;
using System;
using System.Collections.Generic;
using MinimalLSA;
using System.IO;
using System.Text;
using System.Linq;

namespace MinimalLSA.Test
{ 	[TestFixture]
	public class StemmingTest
	{
		string filteredSource;
		int wsCount;
		string[] Terms;

		[Test]
		public void GeneratorTest()
		{
			string rawSource = File.ReadAllText (string.Format
				(@"..{0}..{0}..{0}MinimalLSA{0}TermGeneration{0}SourceTermGeneratorBase.cs", Path.DirectorySeparatorChar));
			StringBuilder sb = new StringBuilder (rawSource.Length);

			wsCount = 0;

			bool wasWhiteSpace = false;

			foreach (char c in rawSource) 
			{
				if (wasWhiteSpace && !Char.IsLetter (c)) {
					continue;
				} else if (!Char.IsLetter (c)) {
					sb.Append (" ");
					wsCount++;
					wasWhiteSpace = true;
				} else {
					wasWhiteSpace = false;
					sb.Append (c);
				}
			}
			Console.WriteLine (filteredSource = sb.ToString ());
			Assert.GreaterOrEqual (rawSource.Length, sb.Length);
		}

		[Test]
		public void Test_02()
		{
			//terms are exploded PHP-style from Camel- or Pascal-Case words
			//thus there should be more terms than single spaces in the filtered
			//document
			var generator = new TermGenerator (new List<string> ());
			Terms = generator.GetTerms (filteredSource);
			Assert.GreaterOrEqual (Terms.Length, wsCount);
		}

		[Test]
		public void Test_03()
		{
			Func<string,string> preprocessingStep = (l => 
				{
					StringBuilder sb = new StringBuilder(l.Length);
					bool wasWhiteSpace = false;

					foreach (char c in l) 
					{
						if (wasWhiteSpace && !Char.IsLetter (c)) {
							continue;
						} else if (!Char.IsLetter (c)) {
							sb.Append (" ");
							wsCount++;
							wasWhiteSpace = true;
						} else {
							wasWhiteSpace = false;
							sb.Append (c);
						}
					}
					return sb.ToString();
				});
			var processor = new DocProcessor (preprocessingStep);
			var generator = new TermGenerator (new List<string> ());
			string inputFilePath = string.Format
				(@"..{0}..{0}..{0}MinimalLSA{0}TermGeneration{0}SourceTermGeneratorBase.cs", Path.DirectorySeparatorChar);

			processor.UpdateFromFile (inputFilePath, generator);
			bool sourceTermExists = processor.Terms.Contains ("sourc");
			bool expected = true;
			Assert.AreEqual (expected, sourceTermExists);
		}

		[Test]
		public void Test_04()
		{
			//set up objects of interest
			Func<string,string> preprocessingStep = (l => 
				{
					StringBuilder sb = new StringBuilder(l.Length);
					bool wasWhiteSpace = false;

					foreach (char c in l) 
					{
						if (wasWhiteSpace && !Char.IsLetter (c)) {
							continue;
						} else if (!Char.IsLetter (c)) {
							sb.Append (" ");
							wsCount++;
							wasWhiteSpace = true;
						} else {
							wasWhiteSpace = false;
							sb.Append (c);
						}
					}
					return sb.ToString();
				});
			var processor = new DocProcessor (preprocessingStep);
			var generator = new TermGenerator (new List<string> ());

			//get all source files in solution
			var baseDirectory = new DirectoryInfo (string.Format (@"..{0}..{0}..{0}", Path.DirectorySeparatorChar));
			foreach (string sourcePath in baseDirectory
				.EnumerateFiles ("*.cs", SearchOption.AllDirectories)
				.Select(fi => fi.FullName)) 
			{
				processor.UpdateFromFile (sourcePath, generator);
			}

			var handler = new MathHandler (string.Format (@"..{0}..{0}..{0}", Path.DirectorySeparatorChar),
				              "cs", preprocessingStep);

			var output = handler.Index ();

			var expected = processor.Terms;

			Assert.AreEqual (expected, output.Terms);
		}
	}
}

