using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MinimalLSA
{
	public interface ISourceTermGenerator
	{
		string[] GetTerms (string sourceTextLine);
		string[] GetTokens (string sourceTextLine);
		bool TryGetStems (string token, out string[] stems);
		List<string> StopWords { get; set; }
		Regex TokenPattern { get; set; }
		Regex WordMatchPattern { get; set; }
		string WordReplacePattern { get; set; }
	}
}

