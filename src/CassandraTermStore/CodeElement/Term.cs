using System;
using Cassandra;

namespace CStore
{
	public class Term : CodeElementBase
	{
		public string Text
		{ 
			get{ return Literal; }

			private set { Literal = value; }
		}

		public int TermID { get; private set;}

		public string DocumentName { get; private set;}

		public int DocumentID { get; private set;}

		public Term (Row termData)
			: this((string)termData ["word"])
		{
			TermID = Int32.Parse ((string)termData ["term_id"]);
			DocumentName = (string)termData ["document_name"];
			DocumentID = Int32.Parse ((string)termData ["document_id"]);
		}

		public Term(string value)
			: base(value)
		{

		}

		public override CodeElementBase Clone()
		{
			var t = new Term (String.Copy(Literal));
			t.TermID = TermID;
			t.DocumentName = String.Copy(t.DocumentName);
			t.DocumentID = DocumentID;
			return t;
		}
	}
}

