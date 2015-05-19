package Jama;

import static org.junit.Assert.*;

import java.util.ArrayList;
import java.util.Arrays;

import junit.framework.Assert;

import org.junit.Test;

public class GeneralVectorTest {

	@Test
	public final void testSet() {
		GeneralVector gv = new GeneralVector(3);
		gv.set(0, 3);
		gv.set(1, -1.2);
		gv.set(2, 5.5);
		double expected = -1.2;
		assertEquals(expected, gv.get(1), 0.01d);
	}

	@Test
	public final void testInitToZero() {
		GeneralVector gv = new GeneralVector(3);
		gv.set(0, 3);
		gv.set(1, -1.2);
		gv.set(2, 5.5);
		gv.initToZero();
		double expected = 0;
		assertEquals(expected, gv.get(1), 0.01d);
	}

	@Test
	public final void testRescaleToUnitInterval() {
		GeneralVector gv = new GeneralVector(3);
		gv.set(0, 3);
		gv.set(1, -1.2);
		gv.set(2, 5.5);
		gv.rescaleToUnitInterval();
		double expected = 1d;
		assertEquals(expected, gv.get(2), 0.01d);
	}

	@Test
	public final void testGetMagnitude() {
		GeneralVector gv = new GeneralVector(3);
		gv.set(0, 3);
		gv.set(1, -1.2);
		gv.set(2, 5.5);
		double expected = 6.378d;
		assertEquals(expected, gv.getMagnitude(), 0.01d);
	}

	@Test
	public final void testUnitaryDotProduct() {
		GeneralVector gv = new GeneralVector(3);
		gv.set(0, 3);
		gv.set(1, -1.2);
		gv.set(2, 5.5);
		double expected = 1d;
		assertEquals(expected, gv.unitaryDotProduct(gv), 0.01d);
	}

	/*
	@Test
	public final void testToList() {
		GeneralVector gv = new GeneralVector(3);
		gv.set(0, 3);
		gv.set(1, -1.2);
		gv.set(2, 5.5);
		
		ArrayList<Double> expected = new ArrayList<Double>();
		expected.add(3d);
		expected.add(-1.2);
		expected.add(5.5);
		for(int i=0; i < 3; i++)
		{
			assertEquals(expected.get(i), gv.get(i));
		}
	}
	*/

}
