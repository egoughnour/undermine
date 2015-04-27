namespace FileStore
{
public enum RequestedData
{
	Edges, //id and key required
	Senses, //word, lang, key required
	Synset, //id and key required
	SynsetIds, //word, lang, key required
	SynsetIdsFromWikipediaTitle //word, lang, POS, key required
}

public enum PartOfSpeech
{
	ADJECTIVE, 
	ADVERB,
	ARTICLE, 
	CONJUNCTION, 
	DETERMINER, 
	INTERJECTION, 
	NOUN, 
	PREPOSITION, 
	PRONOUN, 
	VERB,
	Unspecified
}

public enum Language
{
	EN,
	Unspecified
}
}