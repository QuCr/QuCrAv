using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

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
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());

			opdracht1(true, false);
			opdracht2(true, true);

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
				if (print) Console.WriteLine("[");
				foreach (Point bar in Point.points) {
					bar.getLatLong();
					if (print) Console.WriteLine("[{1,-15},{2,-15},{3,0}],",
						bar.address,
						(bar.latitude - 51).ToString().Replace(',', '.'),
						(bar.longitude - 4).ToString().Replace(',', '.'),
						bar.id);
				}
				if (print) Console.WriteLine("]");
			}
		}

		private static void opdracht2(bool execute = true, bool print = false) {
			if (execute) {
				new TSP().start();
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