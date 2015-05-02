using System;

namespace CStore
{
	public class Keyword : CodeElementBase
	{
		public Keyword (string value)
			: base(value)
		{

		}

		public override CodeElementBase Clone()
		{
			var kw = new Keyword (String.Copy (Literal));
			return kw;
		}
	}
}

