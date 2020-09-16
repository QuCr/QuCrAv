using System;
using System.Collections.Generic;

namespace QuCrAv {
	public class Path {
		int pathID;
		int pathCount = 0;

		public List<Point> order = new List<Point>();
		public double distance = double.PositiveInfinity;
		public double fitness;

		static Random random = new Random();

		public Path() {
			pathID = pathCount++;
			order.AddRange(Point.points);
			shuffle(order);

			calculateDistance();
		}

		public Path(params Point[] points) {
			order.AddRange(points);
			shuffle(order);

			calculateDistance();
		}

		/// <summary> gebruikt Haversine's formule </summary>
		/// <see cref="https://en.wikipedia.org/wiki/Haversine_formula"/>
		/// <returns> afstand tussen twee punten op een bol (Aarde) </returns>
		public double calculateDistance() {
			double pathDistance = 0;
			int radius = 6371000;

			for (int i = 0;i < order.Count - 1;i++) {
				Point origin = order[i];
				Point destination = order[i+1];

				double lat1 = origin.latitude * Math.PI / 180;
				double lon1 = destination.latitude * Math.PI / 180;
				double lat2 = (destination.latitude - origin.latitude) * Math.PI / 180;
				double lon2 = (destination.longitude - origin.longitude) * Math.PI / 180;
				double a = Math.Sin(lat2 / 2) * Math.Sin(lat2 / 2) +
						   Math.Cos(lat1) * Math.Cos(lon1) *
						   Math.Sin(lon2 / 2) * Math.Sin(lon2 / 2);
				double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
				pathDistance += radius * c;
			}

			return distance = pathDistance;
		}

		void shuffle<T>(List<T> array) {
			int n = array.Count;
			for (int i = 0;i < (n - 1);i++) {
				int r = i + random.Next(n - i);
				T t = array[r];
				array[r] = array[i];
				array[i] = t;
			}
		}
	}
}
