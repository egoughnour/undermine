using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MinimalLSA
{
	public class TermGenerator : SourceTermGeneratorBase, ISourceTermGenerator
	{
		public TermGenerator (List<string> stopWords,
			Regex tokenPattern = null,
			Regex wordMatchPattern = null,
			string wordReplacePattern = @" $1$2")
			: base(stopWords, tokenPattern, wordMatchPattern, wordReplacePattern)
		{
			if(tokenPattern == null)
			{
				TokenPattern = new Regex(@"[\s]|(?=\p{P})");
			}
			if (wordMatchPattern == null) 
			{
				WordMatchPattern = new Regex (@"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])");
			}
		}
	}
}

