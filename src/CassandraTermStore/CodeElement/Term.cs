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

		//term_id, word, document_id, document_name
		//row.GetValue<int>(0)

		public Term (Row termData)
			: this(termData.GetValue<string>(1))
		{
			TermID = termData.GetValue<int>(0);
			DocumentName = termData.GetValue<string>(3);
			DocumentID = termData.GetValue<int>(2);
		}

		public Term(string value)
			: base(value)
		{

		}

		public override CodeElementBase Clone()
		{
			var t = new Term (String.Copy(Literal));
			t.TermID = TermID;
			t.DocumentName = String.Copy(DocumentName);
			t.DocumentID = DocumentID;
			return t;
		}
	}
}

