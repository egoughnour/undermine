using System;
using System.Xml.Serialization;

namespace MinimalLSA
{
	public class Document : IUniqueResource
	{
		public int RowColumnIndex { get; set; }

		public Guid Id { get; set; }
		//the document file name
		public string Value { get; set; }

		public string Path { get; set; }

		public LineCollection Lines { get; set; }
	}

	[XmlRoot("Documents")]
	public class DocumentCollection
	{
		[XmlElement("Document")]
		public Document[] Documents { get; set; }
	}
}

