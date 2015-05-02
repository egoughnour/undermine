using System;

namespace CStore
{
	public class CodeElementBase : ICodeElement
	{
		public CodeElementBase NextElement{ get; set;}

		public CodeElementBase (string value)
		{
			Literal = value;
		}

		protected string Literal{ get; set;}

		#region ICodeElement implementation

		public ICodeElement Next ()
		{
			return NextElement; 
		}

		public bool HasNext ()
		{
			return NextElement != null;
		}

		public virtual CodeElementBase Clone()
		{
			var element = new CodeElementBase (String.Copy (Literal));
			element.NextElement = null;
			return element;
		}

		#endregion
	}
}

