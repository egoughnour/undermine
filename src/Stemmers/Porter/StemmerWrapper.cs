using System;
using System.IO;
using System.Collections.Generic;

namespace Porter
{
	public class StemmerWrapper
	{
		public List<string> StopWords 
		{
			get;  
			set;
		}

		public List<string> Stems 
		{
			get;
			private set;
		}

		public StemmerWrapper ()
		{
			StopWords = new List<string> ();
			Stems = new List<string> ();
		}

		public bool TryProcessStream (Stream stream)
		{
			try
			{
				char[] w = new char[501];
				try
				{
					while (true)
					{
						int ch = stream.ReadByte ();
						if (Char.IsLetter ((char)ch))
						{
							var s = new Stemmer ();
							int j = 0;
							while (true) 
							{
								ch = Char.ToLower ((char)ch);
								w [j] = (char)ch;
								if (j < 500)
									j++;
								ch = stream.ReadByte ();
								if (!Char.IsLetter ((char)ch))
								{
									for (int c = 0; c < j; c++)
										s.add (w [c]);
									s.stem (StopWords);
									if (!s.IsStopWord) 
									{
										Stems.Add(s.ToString());
									} 
									break;
								}
							}
						}
						if (ch < 0)
							break;
					}
				} 
				catch 
				{
					return false;
				}
			} 
			catch
			{
				return false;
			}
			return true;
		}

	}

}