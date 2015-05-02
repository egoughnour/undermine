using System;
using Cassandra;

namespace CStore
{
	public interface ICodeElement
	{
		ICodeElement Next();
		bool HasNext();
	}

}

