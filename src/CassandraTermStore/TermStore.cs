using System;
using Cassandra;
using System.Collections.Generic;
using System.Linq;

namespace CStore
{
	public class TermStore
	{
		private static Cluster LocalCluster;
		private static ISession DBSession; 
		static TermStore()
		{
			LocalCluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
			DBSession = LocalCluster.Connect("semantic");
		}

		public TermStore ()
		{
		}

		public Dictionary<int,Term> TermsByID (string fromDocument)
		{
			var command = "select * from terms where document_name='{0}'";
			var rows = DBSession.Execute (string.Format (command, fromDocument));
			var toReturn = new Dictionary<int,Term> ();
			foreach (Row row in rows) 
			{
				toReturn [Int32.Parse ((string)row ["term_id"])] = new Term (row);
			}
			return toReturn;
		}


	}
}