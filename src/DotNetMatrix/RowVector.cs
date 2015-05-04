using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMatrix
{
	public class RowVector : GeneralMatrix
	{
		public RowVector (int m, int n)
			: base (m, n)
		{
		}

		public RowVector(int m, int n, double s)
			: base(m,n,s)
		{
		}

		public RowVector(double[][] A)
			: base(A)
		{
		}
			
		public RowVector(double[][] A, int m, int n)
			: base(A,m,n)
		{
		}

		/// <summary>Construct a matrix from a one-dimensional packed array</summary>
		/// <param name="vals">One-dimensional array of doubles, packed by columns (ala Fortran).
		/// </param>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <exception cref="System.ArgumentException">   Array length must be a multiple of m.
		/// </exception>

		public RowVector (double[] vals, int m)
			: base (vals, m)
		{
		}

		//this is a vector of row vectors.  So the index is into 
		//the outer vector and returns a vector (a row)
		public List<double> this[int index]
		{
			get
			{
				var row = new List<double> ();
				for (int i = 0; i < Array [0].Length; i++) row.Add(Array[0][i]);
				return row;
			}
		}
	}
}

