package Jama;

import java.util.ArrayList;
import java.util.Collections;

public class GeneralVector
{
	RowVector InnerVector;
	
	public GeneralVector(int Length)
	{
		InnerVector = new RowVector(1,Length);
	}
	
	public GeneralVector(double [] array)
	{	
		this(array.length);
		for(int i=0; i < array.length; i++)
		{
			InnerVector.set(0, i, array[i]);
		}
	}
	
	public double get(int index)
	{	
		return InnerVector.get(0, index);
	}
	
	public void set(int index, double value)
	{
		InnerVector.set(0, index, value);
	}
	
	public void initToZero()
	{
		InnerVector.get(0).forEach(e -> e = 0d);
	}
	
	public void rescaleToUnitInterval()
	{
		double max = Collections.max(InnerVector.get(0));
		if(max != 0d)
		{
			InnerVector.get(0).forEach(e -> e= e/max);
		}
	}
	
	public double getMagnitude()
	{
		return InnerVector.normF();
	}
	
	public double unitaryDotProduct(GeneralVector operand)
	{
		double[] total = {0d};
		int[] index = {0};
		toList().forEach(e -> total[0] +=
				((e * operand.get(index[0]++))/
					(getMagnitude() * operand.getMagnitude())));
		return total[0];
	}

	public ArrayList<Double> toList()
	{
		return InnerVector.get(0);
	}
	
	public Matrix Multiply(Matrix matrix)
	{
		return InnerVector.arrayTimes(matrix);
	}
}	
