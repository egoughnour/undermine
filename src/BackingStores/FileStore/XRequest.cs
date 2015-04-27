using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace FileStore
{
	public class XRequest
	{
		private string Key;

		public RequestedData Type{ get; set; }

		public string Id { get; set; }

		public string Word { get; set; }

		public Language LanguageCode { get; set; }

		public PartOfSpeech POS { get; set; }

		const string RequestBase = @"https://babelnet.io/v1/";

		public int Timeout = 90000;

		public static string Verb(RequestedData dataToReturn) 
		{
			return "get" + dataToReturn;
		}

		public object Response { get; set; }

		public XRequest (string key, string id=null, string word=null, Language language=Language.Unspecified, PartOfSpeech pos=PartOfSpeech.Unspecified)
		{
			Key = key;
			Id = id;
			Word = word;
			LanguageCode = language;
			POS = pos;
		}

		public XRequest(FileInfo keyFile, string id=null, string word=null, Language language=Language.Unspecified, PartOfSpeech pos=PartOfSpeech.Unspecified)
			: this (File.ReadAllText (keyFile.FullName), id, word, language, pos)
		{

		}

		public bool HasRequiredParameters
		{
			get 
			{
				switch (Type) 
				{
				case RequestedData.Edges:
				case RequestedData.Synset:
					return Id != null;
				case RequestedData.Senses:
				case RequestedData.SynsetIds:
					return Word != null && LanguageCode != Language.Unspecified; 
				default:
					return Word != null && LanguageCode != Language.Unspecified && POS != PartOfSpeech.Unspecified;
				}
			}
		}

		public string RequestURL
		{
			get
			{
				string rqString = RequestBase + Verb(Type) + "?";
				bool firstParameter = true;
				if (Word != null) {
					rqString += ("word=" + Word);
					firstParameter = false;
				}
				if (Id != null) {
					rqString += (
						(firstParameter ? string.Empty : @"&")
						+ "id=" + Id);
					firstParameter = false;
				}
				if (LanguageCode != Language.Unspecified) {
					rqString += (
						(firstParameter ? string.Empty : @"&")
						+ "lang=" + LanguageCode);
					firstParameter = false;
				}
				if (POS != PartOfSpeech.Unspecified) {
					rqString += (
						(firstParameter ? string.Empty : @"&")
						+ "pos=" + POS);
					firstParameter = false;
				}
				rqString += (
					(firstParameter ? string.Empty : @"&")
					+ "key=" + Key);
				return rqString;
			}
		}

		public void GetResponse(FileResponseStore knowledgeBase)
		{
			XElement cachedResponse;
			if (!knowledgeBase.TryGetResponse (RequestURL, out cachedResponse)) 
			{
				string jsonResponse = InvokeRequest (RequestURL);

				cachedResponse = knowledgeBase.UpdateStore (RequestURL, jsonResponse);
			}
			Response = cachedResponse;
		}

		string InvokeRequest (string requestURL)
		{
			var request = (HttpWebRequest)WebRequest.Create(requestURL);
			request.Timeout = Timeout;
			request.Method = "GET";
			var response = request.GetResponse ();
			var utf8 = Encoding.GetEncoding ("utf-8");
			var reader = new StreamReader (response.GetResponseStream (), utf8);
			var flatResponse = reader.ReadToEnd ();
			reader.Close ();
			response.Close ();
			return flatResponse;
		}
	}
}

