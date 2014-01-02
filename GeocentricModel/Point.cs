using System;
using System.Linq;

namespace GeocentricModel
{
	public class Point
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Point(double x = 0, double y = 0)
		{
			this.X = x;
			this.Y = y;
		}
	}
}