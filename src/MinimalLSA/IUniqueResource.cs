using System;

namespace MinimalLSA
{
	public interface IUniqueResource
	{
		Guid Id { get; set; }
		string Value { get; set; }
		int RowColumnIndex { get; set; }
	}
}

