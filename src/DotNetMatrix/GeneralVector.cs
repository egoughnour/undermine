using System;
using System.Linq;
using System.Collections.Generic;

namespace DotNetMatrix
{
	public class GeneralVector
	{
		private RowVector InnerVector;

		public GeneralVector (int Length)
		{
			InnerVector = new RowVector (1, Length);
		}

		public GeneralVector(double [] array)
			: this(array.Length)
		{
			array.Select ((e, i) => this [i] = e); 
		}

		public double Magnitude
		{
			get
			{
				return InnerVector.NormF ();
			}
		}

		public double this[int index]
		{
			get
			{
				return InnerVector [0] [index];
			}

			set
			{
				InnerVector [0] [index] = value;
			}
		}

		public void RescaleToUnitInterval()
		{
			if (InnerVector [0].Max() > 0) {
				for (int i = 0; i < InnerVector [0].Count; i++)
					InnerVector [0] [i] = InnerVector [0] [i] / InnerVector [0].Max();
			}
		}

		public void InitToZero()
		{
			InnerVector [0].Select (e => e = 0);
		}

		public double UnitaryDotProduct(GeneralVector operand)
		{
			double total = 0;
			this.ToList
				.Select((e, i) => total +=
					((e * operand [i]) /
						(this.Magnitude * operand.Magnitude)));
			return total;
		}

		public List<double> ToList
		{
			get
			{
				return InnerVector [0];
			}
		}

		public GeneralMatrix Multiply (GeneralMatrix matrix)
		{
			return InnerVector.Multiply (matrix);
		}
	}
}

