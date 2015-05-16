package Jama;
import Jama.util.*;
import java.util.*;
public class RowVector extends Matrix {
	public RowVector(int m, int n)
	{	
		super(m, n);
	}
	
	public RowVector(int m, int n, double s)
	{
		super(m,n,s);
	}
	
	public RowVector(double[][] A)
	{
		super(A);
	}
	
	public RowVector(double[][] A, int m, int n)
	{
		super(A,m,n);
	}
	
	public RowVector(double[] vals, int m)
	{
		super(vals, m);
	}
	
	public ArrayList<Double> get(int index)
	{
		double[] row = this.getMatrix(index,index,0,m-1).getArray()[0];
		ArrayList<Double> outputList =  new ArrayList<Double>();
		for(int i=0; i < m; i++)
		{
			outputList.add(row[i]);
		}
	
	    return outputList;
	}
}	

