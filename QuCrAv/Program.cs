using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace QuCrAv {
	class Program {
		/// <summary> Used for storing literally cost-expensive HTTP requests  </summary>
		static Dictionary<string, JToken> cache = new Dictionary<string, JToken>();

		/// <summary> Path to cache file </summary>
		/// <remarks> ../../../ is used so that the cache file is on the same level as this file. </remarks>
		static string cachePath = "../../../cache.json";
		static bool cacheUsed = false;
		static bool cacheRewrite = false;
		public static string APIKEY => File.ReadAllText("../../../key.txt");

		static Program() {
			if (File.Exists(cachePath)) {
				Console.WriteLine("READ CACHE");
				cache = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(File.ReadAllText(cachePath));
			}
		}

		static void Main(string[] args) { 
			opdracht1(true, false);
			opdracht2(true, false);

			/*int a = 0;
			int b = 15 * 15 * 15;
			foreach (Point point1 in Point.points) {
				foreach (Point point2 in Point.points) {
					foreach (Point point3 in Point.points) {
						int d1 = 0, d2 = 0, d3 = 0;
						bool b1, b2, b3;
						if (b2 = point2 != point3) d1 = new Route(point2, point3).distance;
						if (b3 = point1 != point3) d2 = new Route(point1, point3).distance;
						if (b1 = point1 != point2) d3 = new Route(point1, point2).distance;

						if ((d1 * d2 * d3) != 0) {
							if ((d2 + d3) < d1) { Console.WriteLine($"{a}: [*{point1.id},{point2.id},{point3.id}]"); a++; }
							if ((d1 + d3) < d2) { Console.WriteLine($"{a}: [{point1.id},*{point2.id},{point3.id}]"); a++; }
							if ((d1 + d2) < d3) { Console.WriteLine($"{a}: [{point1.id},{point2.id},*{point3.id}]"); a++; }

						}
					}
				}
			}*/

			int[] iii = new int[15] { 4,
				10,
				8,
				11,
				5,
				4,
				2,
				6,
				1,
				9,
				7,
				12,
				3,
				13,
				0
			};
			//Console.Write("Building T, Schaliënhoevedreef 20, 2800 Mechelen/");
			for (int i = 0;i < iii.Length;i++) {
				Console.Write(Point.points[i].postalCode + "\n/");
			}
			//Console.Write("Building T, Schaliënhoevedreef 20, 2800 Mechelen/");

			/*Console.Write("optimize:true|");
			foreach (Point point in Point.points) {
				Console.Write($"{point.address}|");
			}
			Console.Write("\n");*/

			#region logs
			if (cacheUsed) Console.WriteLine("USED CACHE");
			if (cacheRewrite) {
				Console.WriteLine("REWROTE CACHE");
				File.WriteAllText(cachePath, JsonConvert.SerializeObject(cache, Newtonsoft.Json.Formatting.Indented));
			}
			#endregion
			
			Console.ReadKey();
		}

		private static void opdracht1(bool execute = true, bool print = false) {
			if (execute) {
				if (print) Console.WriteLine("\t{0,-50}{1,-15}{2,-15}", "[ADDRESS]", "[LATITUDE]", "[LONGIITUDE]");
				foreach (Point bar in Point.points) {
					bar.generateLatitudeLngitde();
					if (print) Console.WriteLine("\t{0,-50}{1,-15}{2,-15}", bar.address, bar.latitude, bar.longitude);
				}
			}
		}

		private static void opdracht2(bool execute = true, bool print = false) {
			if (execute) {
				foreach (Point origin in Point.points) {
					foreach (Point destination in Point.points) {
						if (origin != destination) { 
							new Route(origin, destination);
						}
					}

					IEnumerable<int> routeDistance = from route in Route.routes
													 select route.distance;
					IEnumerable<int> routeDuration = from route in Route.routes
													 select route.duration;
					
					if (print) Console.WriteLine($"\t[{origin.address}] [{routeDistance.Sum()}m/{routeDistance.Min()}m/{routeDistance.Max()}m/{(int)routeDistance.Average()}s/{(int)Variance(routeDistance)}s] [{routeDuration.Sum()}s/{routeDuration.Min()}s/{routeDuration.Max()}s/{(int)routeDuration.Average()}s/{(int)Variance(routeDistance)}s]");
				
				}
			}
		}

		private static double Variance(IEnumerable<int> values) {
			double standardDeviation = 0;

			if (values.Any()) {
				// Compute the average.     
				double avg = values.Average();

				// Perform the Sum of (value-avg)_2_2.      
				double sum = values.Sum(d => Math.Pow(d - avg, 2));

				// Put it all together.      
				standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
			}

			return standardDeviation;
		}

		/// <summary> Gets the JSON data from the cache or the given URI. If URI is not cached, get it online! </summary>
		public static JToken getJSONfromURL(string uri) {
			if (cache.ContainsKey(uri.Replace(APIKEY, "APIKEY"))) {
				cacheUsed = true;
				return cache[uri.Replace(APIKEY, "APIKEY")];
			} else {
				HttpClient httpClient = new HttpClient { BaseAddress = new Uri(uri) };
				string json = httpClient.GetStringAsync("").Result;

				//Issue #1: 
				JObject Jobject = JObject.Parse("{data: " + json + "}");

				cacheRewrite = true;
				cache.Add(uri.Replace(APIKEY, "APIKEY"), Jobject["data"]);


				return Jobject["data"];
			}
		} 
	}
}