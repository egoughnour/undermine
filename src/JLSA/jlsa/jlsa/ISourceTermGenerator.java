package jlsa;

import java.util.*;
import java.util.regex.*;

public interface ISourceTermGenerator 
{
	Pattern getTokenPattern();
	void setTokenPattern(Pattern value);
	
	Pattern getWordPattern();
	void setWordPattern(Pattern value);
	
	ArrayList<String> getStopWords();
	void setStopWords(ArrayList<String> value);
	
	boolean TryGetStems(String token, ArrayList<String> stems);
	String[] GetTerms(String sourceText);
	String[] GetTokens(String sourceText);
	
}
