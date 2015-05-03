using System;
using System.IO;
using System.Linq;
using DotNetMatrix;
using System.Collections.Generic;

namespace MinimalLSA
{
	public class OutputDataSet
	{
		public GeneralMatrix Uk { get; set; }
		public GeneralMatrix SkInverse { get; set; }
		public GeneralMatrix TermDoc { get; set; }
		public GuidSet Terms { get; set; }
		public GuidSet Documents { get; set; }
	}

}

