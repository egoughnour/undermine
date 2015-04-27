using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Porter;

namespace Porter.Test
{
	[TestFixture]
	public class StemmerTest
	{
		string InputTextPath;
		string StopWordPath;
		List<string> StopWords;

		[TestFixtureSetUp]
		public void Initialize ()
		{
			InputTextPath =
				@"../../InputWords.txt";
			StopWordPath = 
				@"../../StopWords.txt";
			StopWords = File.ReadAllLines (StopWordPath).ToList ();
		}

		[Test]
		public void TestStemmer()
		{
			int stopWordCount =  0;
			int contentWordCount = 0;
		for (int i = 0; i < 1; i++)
			try {
			FileStream _in = new FileStream( InputTextPath, FileMode.Open, FileAccess.Read );
			char[] w = new char[501];
			try {
				while(true) {
					int ch = _in.ReadByte();
					if ( Char.IsLetter((char) ch)) {
						var s = new Stemmer();
						int j = 0;
						while(true) {
							ch = Char.ToLower((char) ch);
							w[j] = (char) ch;
							if (j < 500)
								j++;
							ch = _in.ReadByte();
							if (!Char.IsLetter((char) ch)) {
								for (int c = 0; c < j; c++)
									s.add(w[c]);
								s.stem(StopWords);
								if(!s.IsStopWord){
									contentWordCount++;
								}
								else{
									stopWordCount++;
								}
								break;
							}
						}
					}
					if (ch < 0)
						break;
				}
			} catch (IOException ) {
					Assert.Fail("error reading " + InputTextPath);
				break;
			}
		} catch (FileNotFoundException ) {
					Assert.Fail ("file " + InputTextPath + " not found");
			break;
		}
			int expectedStopWordCount = 8;
			Assert.AreEqual (expectedStopWordCount, stopWordCount);
	}

	}
}

