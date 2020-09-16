using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Drawing;

namespace QuCrAv {
	class Program {
		/// <summary> Used for storing literally cost-expensive HTTP requests  </summary>
		static Dictionary<string, JToken> cache = new Dictionary<string, JToken>();

		/// <summary> Path to cache file </summary>
		/// <remarks> ../../../ is used so that the cache file is on the same level as this file. </remarks>
		static string cachePath = "../../Data/cache.json";
		static bool cacheUsed = false;
		static bool cacheRewrite = false;
		public static string APIKEY => File.ReadAllText("../../Data/key.txt");

		public static Form form;

		static Program() {
			if (File.Exists(cachePath)) {
				Console.WriteLine("READ CACHE");
				cache = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(File.ReadAllText(cachePath));
			}
		}
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			opdracht1(true, false);
			opdracht2(false, true);
			opdracht3(true, true);

			Application.Run(form = new Form1());
			
			#region logs
			if (cacheUsed) Console.WriteLine("USED CACHE");
			if (cacheRewrite) {
				Console.WriteLine("REWROTE CACHE");
				File.WriteAllText(cachePath, JsonConvert.SerializeObject(cache, Formatting.Indented));
			}
			#endregion

			Console.ReadKey();
		}
		static void opdracht1(bool execute = true, bool print = false) {
			if (execute) {
				if (print) Console.WriteLine("[");
				Point.mainPoint.getLatLong();
				if (print) Console.WriteLine("[{1,-15},{2,-15},{3,0}],",
					Point.mainPoint.address,
					(Point.mainPoint.latitude - 51).ToString().Replace(',', '.'),
					(Point.mainPoint.longitude - 4).ToString().Replace(',', '.'),
					Point.mainPoint.id);
				foreach (Point point in Point.points) {
					point.getLatLong();
					if (print) Console.WriteLine("[{1,-15},{2,-15},{3,0}],",
						point.address,
						(point.latitude - 51).ToString().Replace(',', '.'),
						(point.longitude - 4).ToString().Replace(',', '.'),
						point.id);
				}
				if (print) Console.WriteLine("]");
			}
		}

		static void opdracht2(bool execute = true, bool print = false) {
			if (execute) {
				PathFinder.start(print);
			}
		}

		static void opdracht3(bool execute = true, bool print = false) {
			if (execute) {
				 PathFinder.start(print, 90);
			}
		}

		/// <summary> Gets the JSON data from the cache or the given URI. If URI is not cached, get it online! </summary>
		public static JToken getJSONfromURL(string uri) {
			try {
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
			} catch (FileNotFoundException exception) {
				throw new FileNotFoundException(
					"key.txt moet toegevoegd worden en een geldige Google API sleutel hebben!",
					exception
				);
			}
		}
	}
}