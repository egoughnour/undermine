using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json;
using System.IO;

namespace FileStore
{
	public class FileResponseStore : IResponseStore<string, string, XElement, XElement>
	{
		const string elementName = "Response";
		XDocument store;
		public string BackingPath { get; private set; }
		public FileResponseStore (string storePath)
		{
			if (File.Exists (storePath)) {
				store = XDocument.Load (storePath);
			} else {
				store = XDocument.Parse (@"<Responses></Responses>");
				store.Save (storePath);
			}
			BackingPath = storePath;
		}

		const string requestURL = @"//{0}[@RequestURL='{1}']";
			
		#region IKnowledgeBaseStore implementation

		/// <summary>
		/// Tries to get response.
		/// </summary>
		/// <returns><c>true</c>, if get response was found, <c>false</c> otherwise.</returns>
		/// <param name="keyToLookup">The XPath to look up the response.</param>
		/// <param name="cachedData">Cached data.</param>
		public bool TryGetResponse (string keyToLookup, out XElement cachedData)
		{
			if (store.Root.HasElements) {
				cachedData = store.Root.XPathSelectElement (string.Format (requestURL, elementName, keyToLookup));
				return cachedData != null;
			}
			cachedData = null;
			return false;
		}

		/// <summary>
		/// Updates the store.
		/// </summary>
		/// <returns>The store.</returns>
		/// <param name="key">the request URL</param>
		/// <param name="dataToCache">Data to cache.</param>
		public XElement UpdateStore (string key, string dataToCache)
		{
			string correctedJson = @"{""Response"":" + dataToCache + "}";
			XDocument response = JsonConvert.DeserializeXNode (correctedJson, "Response", true);
			//XDocument response = JsonConvert.DeserializeXNode (dataToCache, "Response", true);
			XElement responseRoot = response.Root;
			responseRoot.Add (new XAttribute ("RequestURL", key));
			store.Root.Add (responseRoot);
			store.Save (BackingPath);
			return responseRoot;
		}

		#endregion
	}
}

