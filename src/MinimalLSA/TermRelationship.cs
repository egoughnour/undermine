using System;
using System.Xml.Serialization;

namespace MinimalLSA
{
	[XmlRoot("Term")]
	public class TermRelationship : IUniqueResource
	{
		public int RowColumnIndex { get; set; }

		public Guid Id { get; set; }

		//the literal value of the term in the documents
		public string Value { get; set; }

		public DocumentCollection Documents { get; set; }
	}
}

