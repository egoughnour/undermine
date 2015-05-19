package jlsa;

import java.util.ArrayList;
import java.io.IOException;
import java.nio.file.*;
import java.nio.*;

public abstract class DocumentProcessorBase implements IDocumentProcessor {

	protected ArrayList<ITermDocument> TDList;
	protected IPreprocessor Preprocessor;
	protected boolean DoPreprocess;
	
	public DocumentProcessorBase(IPreprocessor preprocessor)
	{
		Preprocessor = preprocessor;
	}
	
	@Override
	public ArrayList<ITermDocument> getTermDocumentList()
	{
		return TDList;
	}

	@Override
	public void setTermDocumentList(ArrayList<ITermDocument> value)
	{
		TDList = value;
	}

	@Override
	public void updateFromFile(String path, ISourceTermGenerator generator)
	{
		ArrayList<String> stems = new ArrayList<String>();
		try {
			Files.readAllLines(Paths.get(path)).forEach(l -> generator.TryGetStems(
					(MustPreprocess()
					? Preprocessor.preprocess(l, generator.getStopWords())
					: l), stems
					));
		} catch (IOException e) {
			// TODO do something on error
			e.printStackTrace();
		}
		
	}

	public boolean MustPreprocess()
	{
		return DoPreprocess = (Preprocessor != null);
	}

}
