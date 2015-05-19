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
		for(int index=0; index < InnerVector.n; index++)
		{
			set(index, 0d);
		}
	}
	
	public void rescaleToUnitInterval()
	{
		
		double max = 0d;
		for(int index=0; index < InnerVector.n; index++)
		{
			max = (Math.abs(get(index)) > Math.abs(max))
			? get(index)
			: max; 
		}
		
		if(max != 0d)
		{
			for(int index=0; index < InnerVector.n; index++)
			{
				set(index, InnerVector.get(0,  index)/max);
			}
		}
	}
	
	public double getMagnitude()
	{
		return InnerVector.normF();
	}
	
	public double unitaryDotProduct(GeneralVector operand)
	{
		double total = 0d;
		
		for(int index=0; index < InnerVector.n; index++)
		{
			total += get(index) * operand.get(index);
		}
		return total/(getMagnitude()*operand.getMagnitude());
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
