using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace QuCrAv {
	public class Route {
		public static List<Route> routes = new List<Route>();
		public List<Point> points = new List<Point>(Point.points.Count * Point.points.Count - Point.points.Count);
		string url => $"https://maps.googleapis.com/maps/api/directions/json?origin={origin.address}&destination={destination.address}&key={Program.APIKEY}";


		public string distanceText;
		public int distance;
		public string durationText;
		public int duration;

		public Point origin => points[0];
		public Point destination => points[points.Count-1];
		public int count => points.Count;

		public Route(params Point[] points) {
			this.points.AddRange(points);
			routes.Add(this);

			/*JToken token = Program.getJSONfromURL(url);

			JToken leg = token["routes"][0]["legs"][0];
			distanceText = leg["distance"]["text"].ToString();
			distance = int.Parse(leg["distance"]["value"].ToString());
			durationText = leg["duration"]["text"].ToString();
			duration = int.Parse(leg["duration"]["value"].ToString());*/

			distance = calculateDistance();


			//Console.WriteLine($"Route #{routes.Count} " + $"({origin.id} -> {destination.id}) [{distance}] [{duration}] [{distance / duration * 3.6} km/u]");
		}

		private int getDistance() {
			throw new NotImplementedException();
		}
	}
}
