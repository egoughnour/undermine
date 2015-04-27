using System;
using System.Xml.Linq;
//using System.Xml.XPath;

namespace FileStore
{
	public interface IResponseStore<in R, in S, out T, U> 
	{
		T UpdateStore (R key, S dataToCache);
		bool TryGetResponse (R keyToLookup, out U cachedData);

	}
}

