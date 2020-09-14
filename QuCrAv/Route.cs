using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace QuCrAv {
	public class Route {
		public static double[][] distanceMatrix;
		public static List<Route> routes = new List<Route>();
		public static int N = Point.points.Count;

		public List<Point> points = new List<Point>(N*N*N);
		public double distance;
		public Point origin => points[0];
		public Point destination => points[points.Count - 1];

		static Route() {
			N = Point.points.Count;
			distanceMatrix = new double[N][];
			for (int routeID = 0;routeID < N; routeID++) {
				distanceMatrix[routeID] = new double[N];
			}
		}

		public Route(params Point[] points) {
			this.points.AddRange(points);
			routes.Add(this);

			distance = calculateDistance();
			
			Console.WriteLine($"Route #{routes.Count} " + $"({origin.latitude},{origin.longitude} -> {destination.latitude},{destination.longitude}) [{distance}m]");
		}

		/// <summary> gebruikt Haversine's formule </summary>
		/// <see cref="https://en.wikipedia.org/wiki/Haversine_formula"/>
		/// <returns> afstand tussen twee punten op een bol (Aarde) </returns>
		public double calculateDistance() {
			double lat1 = origin.latitude.Value * Math.PI / 180; 
			double lon1 = destination.latitude.Value * Math.PI / 180;
			double lat2 = (destination.latitude.Value - origin.latitude.Value) * Math.PI / 180;
			double lon2 = (destination.longitude.Value - origin.longitude.Value) * Math.PI / 180;

			double a = Math.Sin(lat2 / 2) * Math.Sin(lat2 / 2) +
					   Math.Cos(lat1) * Math.Cos(lon1) *
					   Math.Sin(lon2 / 2) * Math.Sin(lon2 / 2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return 6371000 * c;
		}
	}
}
