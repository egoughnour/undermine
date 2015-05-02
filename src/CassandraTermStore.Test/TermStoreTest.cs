using NUnit.Framework;
using Cassandra;
using System;

namespace CStore.Test
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TermCloneTest()
		{

		}

		[Test]
		public void TestCase ()
		{
			var ts = new TermStore ();
			var statement = new Statement (ts, "frmMain.cs");
			//var uk = new DotNetMatrix.GeneralMatrix (u.Array.Length, svd_rank);
			//  General  : 444
			//  Matrix   : 445
			//  Dot      : 446
			//  Net      : 447
			//  uk       : 448
			//  u        : 449
			//  Array    : 450
			//  Length   : 451
			//  svd_rank : 452
			statement.TryAppendCodeElement (new Keyword ("var"));
			statement.TryAppendCodeElement (new SingleSpace ());
			statement.TryAppendTermByID (448);
			statement.TryAppendCodeElement (new SingleSpace ());
			statement.TryAppendCodeElement (new OtherToken ("="));
			statement.TryAppendCodeElement (new SingleSpace ());
			statement.TryAppendCodeElement (new Keyword ("new"));
			statement.TryAppendCodeElement (new SingleSpace ());
			statement.TryAppendTermByID (446);
			statement.TryAppendTermByID (447);
			statement.TryAppendTermByID (445);
			statement.TryAppendCodeElement (new OtherToken ("."));
			statement.TryAppendTermByID (444);
			statement.TryAppendTermByID (445);
			statement.TryAppendCodeElement (new SingleSpace ());
			statement.TryAppendCodeElement (new OtherToken ("("));
			statement.TryAppendTermByID (449);
			statement.TryAppendCodeElement (new OtherToken ("."));
			statement.TryAppendTermByID (450);
			statement.TryAppendCodeElement (new OtherToken ("."));
			statement.TryAppendTermByID (451);
			statement.TryAppendCodeElement (new OtherToken (","));
			statement.TryAppendCodeElement (new SingleSpace ());
			statement.TryAppendTermByID (452);
			statement.TryAppendCodeElement (new OtherToken (")"));
			statement.TryAppendCodeElement (new OtherToken (";"));

			var reconstructedSource = statement.ToSource;
			string expected = @"var uk = new DotNetMatrix.GeneralMatrix (u.Array.Length, svd_rank);";
			StringAssert.AreEqualIgnoringCase (expected, reconstructedSource);
		}
	}
}

