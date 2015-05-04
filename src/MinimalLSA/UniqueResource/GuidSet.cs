using System;
using System.Linq;
using System.Collections.Generic;

namespace MinimalLSA
{
	public class GuidSet
	{
		private Dictionary<Guid,string> ReferenceSet;
		private Dictionary<Guid,IUniqueResource> GuidLookup;
		private Dictionary<string,IUniqueResource> ValueLookup;
		public GuidSet ()
		{
			ReferenceSet = new Dictionary<Guid, string> ();
			GuidLookup = new Dictionary<Guid, IUniqueResource> ();
			ValueLookup = new Dictionary<string, IUniqueResource> ();
		}

		public bool Contains(IUniqueResource resource)
		{
			return ReferenceSet.ContainsKey (resource.Id);
		}

		public bool Contains(string value)
		{
			return ReferenceSet.ContainsValue (value);
		}

		public void AddResource(IUniqueResource resource)
		{
			resource.RowColumnIndex = -1;
			ReferenceSet.Add (resource.Id, resource.Value);
			GuidLookup.Add (resource.Id, resource);
			ValueLookup.Add (resource.Value, resource);
		}

		public IUniqueResource this[Guid lookupValue]
		{
			get 
			{
				return GuidLookup [lookupValue];
			}

			set
			{
				value.Id = lookupValue;
				GuidLookup[lookupValue] = value;
				ValueLookup [value.Value] = value;
				ReferenceSet [value.Id] = value.Value; 
			}
		}

		public IUniqueResource this[string lookupValue]
		{
			get 
			{
				return ValueLookup [lookupValue];
			}

			set
			{
				value.Value = lookupValue;
				GuidLookup[value.Id] = value;
				ValueLookup [value.Value] = value;
				ReferenceSet [value.Id] = value.Value; 
			}
		}

		public int Count
		{
			get { return ReferenceSet.Count; }
		}

		public List<IUniqueResource> GetIndexedResources()
		{
			if (!GuidLookup.Values.Any (r => r.RowColumnIndex < 0)) 
			{
				//no resources without an index, so just return them
				return GuidLookup.Values.OrderBy (r => r.RowColumnIndex).ToList ();
			}
			if (!GuidLookup.Values.Any (r => r.RowColumnIndex > -1)) 
			{
				//no indices are assigned yet, so just assign them arbitrarily
				GuidLookup.Values.Select ((v, i) => v.RowColumnIndex = i);
				return GuidLookup.Values.ToList ();
			}
			int index = 0;
			while (GuidLookup.Values.Any (v => v.RowColumnIndex < 0)) 
			{  //modify the fewest possible to enforce index uniqueness
				if (GuidLookup.Values.Any (r => r.RowColumnIndex == index)) {
					//there are some with this index, so skip one and assign the rest a negative index
					GuidLookup.Values.Skip (1).Select (v => v.RowColumnIndex = -1);
				} 
				else 
				{
					//nothing is assigned to this index, so get the first with an index < 0
					GuidLookup.Values.First (f => f.RowColumnIndex < 0).RowColumnIndex = index;
				}
				index++;
			}
			return GuidLookup.Values.OrderBy (r => r.RowColumnIndex).ToList ();
		}
	}
}

