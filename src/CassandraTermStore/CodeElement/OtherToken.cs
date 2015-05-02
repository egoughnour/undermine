using System;

namespace CStore
{
	public class OtherToken : CodeElementBase
	{
		public OtherToken (string value)
			: base(value)
		{
		}

		public override CodeElementBase Clone()
		{
			var ot = new OtherToken (String.Copy (Literal));
			return ot;
		}
	}
}

