package jlsa;

import java.util.*;

public interface IDocumentProcessor 
{
	public ArrayList<ITermDocument> getTermDocumentList();
	public void setTermDocumentList(ArrayList<ITermDocument> value);
	
	public void updateFromFile(String path, ISourceTermGenerator generator);
	
}
