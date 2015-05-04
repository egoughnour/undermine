using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Porter;

namespace MinimalLSA
{
	public abstract class SourceTermGeneratorBase : ISourceTermGenerator
	{

		public StemmerWrapper Stemmer;

		public SourceTermGeneratorBase(List<string> stopWords, Regex tokenPattern,
			Regex wordMatchPattern, string wordReplacePattern = @" $1$2")
		{
			StopWords = stopWords;
			TokenPattern = tokenPattern;
			WordMatchPattern = wordMatchPattern;
			WordReplacePattern = wordReplacePattern;
			Stemmer = new StemmerWrapper ();
		}

		#region ISourceTermGenerator implementation

		public virtual bool TryGetStems (string token, out string[] stems)
		{
			stems = null;
			try
			{
				var spaceSeparated = WordMatchPattern.Replace (token, WordReplacePattern);
				using(var memStream = new MemoryStream(Encoding.ASCII.GetBytes(spaceSeparated)))
				{
					memStream.Position = 0;
					Stemmer.StopWords = StopWords;
					if (Stemmer.TryProcessStream (memStream)) {
						stems = Stemmer.Stems.ToArray ();
					}
					else
					{
						return false;
					}

				}
			}
			catch 
			{
				return false;
			}
			return true;
		}

		public List<string> StopWords { get; set;}

		public Regex TokenPattern { get; set; }

		public Regex WordMatchPattern { get; set; }

		public string WordReplacePattern { get; set; }


		public virtual string[] GetTerms (string sourceTextLine)
		{
			var terms = new List<string> ();
			foreach (string token in GetTokens(sourceTextLine)) 
			{
				string[] stems = default(string[]);
				if (TryGetStems (token, out stems)) 
				{
					terms.AddRange (stems);
				} 
				else 
				{
					terms.Add (token);
				}
			}
			return terms.ToArray ();
		}

		public virtual string[] GetTokens (string sourceTextLine)
		{
			return TokenPattern.Split (sourceTextLine);
		}
		#endregion
	}
}

