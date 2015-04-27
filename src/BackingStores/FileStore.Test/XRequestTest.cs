using System;
using System.Xml;
using System.Xml.Linq;
using FileStore;
using NUnit.Framework;
using System.IO;

namespace FileStore.Test
{
	[TestFixture]
	public class XRequestTest
	{
		string WordToLookUp;
		string Key;
		Language RequestedLanguage;
		FileResponseStore FileKB;
		RequestedData RequestType;

		[TestFixtureSetUp]
		public void  Initialize ()
		{
			WordToLookUp = "book";
			FileKB = new FileResponseStore (@"../../DataStore.xml");
			Key = Environment.GetEnvironmentVariable("BABEL_NET_KEY");
			RequestedLanguage = Language.EN;
			RequestType = RequestedData.Senses;
		}

		[Test]
		[Category("FirstSet")]
		public void XRequiredParameterProperty()
		{
			var request = new XRequest(Key, word: WordToLookUp, language: RequestedLanguage);
			request.Type = RequestType;
			var expected = true;
			Assert.AreEqual (expected, request.HasRequiredParameters);
		}

		[Test]
		[Category("FirstSet")]
		public void XRequestURL()
		{
			var request = new XRequest(Key, word: WordToLookUp, language: RequestedLanguage);
			request.Type = RequestType;
			string expected = 
				@"https://babelnet.io/v1/getSenses?word=book&lang=EN&key=" + Key; 
			Assert.AreEqual (expected, request.RequestURL);
		}

		[Test]
		[Category("FirstSet")]
		public void XNonNullResponse()
		{
			var request = new XRequest(Key, word: WordToLookUp, language: RequestedLanguage);
			request.Type = RequestType;
			request.GetResponse (FileKB);
			Assert.IsNotNull (request.Response);
		}

		[Test]
		public void XFileKBContainsUrlKey()
		{
			var request = new XRequest(Key, word: WordToLookUp, language: RequestedLanguage);
			request.Type = RequestType;
			XElement response;
			if (FileKB.TryGetResponse (request.RequestURL, out response)) {
				Assert.IsNotNull (response);
			} else if (!File.Exists (FileKB.BackingPath)) {
				Assert.Inconclusive ();
			} else {
				Assert.Fail ("@File at backing path, '{0}' exists, but response was not persisted.", FileKB.BackingPath);
			}
		}
	}
}

