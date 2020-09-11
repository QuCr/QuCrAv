using System;
using System.Collections.Generic;
using System.Text;

namespace QuCrAv {
	public class Route {
		int rank;
		List<Point> points = new List<Point>();

		int distance;
		int duration;

		Point origin => points[0];
		Point destination => points[points.Count];
	}
}
