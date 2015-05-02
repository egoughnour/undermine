using System;

namespace CStore
{
	public class SingleSpace : CodeElementBase
	{
		public SingleSpace()
			: base(" ")
		{

		}


		public override CodeElementBase Clone()
		{
			return new SingleSpace ();
		}
	}
}

