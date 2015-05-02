using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CStore
{
	public class Statement
	{
		List<ICodeElement> Tokens;
		Dictionary<int,Term> Terms;

		public Statement (TermStore store, string sourceDocument)
		{
			Terms = store.TermsByID (sourceDocument);
			Tokens = new List<ICodeElement> ();
		}

		public bool TryAppendTermByID(int id)
		{
			if (Terms.ContainsKey (id))
			{
				if (Tokens.Count > 0) 
				{
					Term toAdd;
					if (Tokens.Contains (Terms [id]))
					{
						toAdd = (Term)Terms [id].Clone ();
					}
					else
					{
						toAdd = Terms [id];
					}
					toAdd.NextElement = null;
					((CodeElementBase)Tokens [Tokens.Count - 1]).NextElement = toAdd;
					Tokens.Add (toAdd);
				}
				else
				{
					Tokens.Add (Terms [id]);
				}
				return true;
			}
			else 
			{
				return false;
			}
		}

		public bool TryAppendCodeElement(ICodeElement element)
		{
			var elementInstance = element as CodeElementBase;
			if (elementInstance == null)
				return false;
			try
			{
				var toAdd = elementInstance.Clone();
				toAdd.NextElement = null;
				Tokens.Add(toAdd);
				return true;
			}
			catch
			{
				return false;
			}

		}

		public string ToSource
		{
			get
			{
				return string.Concat(Tokens.Select(t => 
					{
						var e = t as CodeElementBase;
						return e.Literal;
					}));
			}
		}
	}
}

