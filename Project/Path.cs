using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace QuCrAv {
	public class Path {
		static int pathCount = -1;

		public List<Point> order = new List<Point>();
		public double distance = double.PositiveInfinity;
		public double fitness;
		public int ID;

		static Random random = new Random();

		public Path() {
			order.AddRange(Point.points);
			shuffle(order);

			 calculateDistance();

			ID = pathCount++;
			//Console.WriteLine($"Route #{routes.Count} " + $"({origin.latitude.ToString().Replace(',', '.')},{origin.longitude.ToString().Replace(',', '.')} -> {destination.latitude.ToString().Replace(',', '.')} , {destination.longitude.ToString().Replace(',', '.')} ) [{distance}m]");
		}
		public Path(params Point[] points) {
			order.AddRange(points);
			shuffle(order);

			calculateDistance();

			ID = pathCount++;
			//Console.WriteLine($"Route #{routes.Count} " + $"({origin.latitude.ToString().Replace(',', '.')},{origin.longitude.ToString().Replace(',', '.')} -> {destination.latitude.ToString().Replace(',', '.')} , {destination.longitude.ToString().Replace(',', '.')} ) [{distance}m]");
		}

		/// <summary> gebruikt Haversine's formule </summary>
		/// <see cref="https://en.wikipedia.org/wiki/Haversine_formula"/>
		/// <returns> afstand tussen twee punten op een bol (Aarde) </returns>
		public double calculateDistance() {
			double pathDistance = 0;
			//Console.WriteLine(order[0].latitude.Value);

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
				pathDistance += 6371000 * c;
			}

			return distance = pathDistance;
		}

		static void shuffle<T>(List<T> array) {
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
