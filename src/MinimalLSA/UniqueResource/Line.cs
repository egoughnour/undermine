using System;
using System.Xml.Serialization;

namespace MinimalLSA
{
	public class Line : IUniqueResource
	{
		public int RowColumnIndex { get; set; }

		public Guid Id { get; set; }

		//the literal text of the line
		public string Value { get; set; }
		public int Number { get; set; }
		public int TermIndex { get; set; }
	}

	[XmlRoot("Lines")]
	public class LineCollection
	{
		[XmlElement("Line")]
		public Line[] Lines { get; set; }
	}
}

