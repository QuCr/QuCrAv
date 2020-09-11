using System;
using QuCrAv;
using ScrapySharp.Network;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace HttpClientStatus {
	class Program {
		static ScrapingBrowser _browser = new ScrapingBrowser();
		static Dictionary<string, JToken> cache = new Dictionary<string, JToken>();

		static string cachePath = "../../../cache.json";
		static bool cacheUsed = false;
		static bool cacheRewrite = false;

		static Program() {
			if (File.Exists(cachePath)) {
				Console.WriteLine("Cache is read");
				cache = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(File.ReadAllText(cachePath));
			}
		}

		static void Main(string[] args) {
			opdracht1();


			if (cacheUsed) Console.WriteLine("Cache is used");
			if (cacheRewrite) {
				Console.WriteLine("Cache is rewritten");
				File.WriteAllText(cachePath, JsonConvert.SerializeObject(cache, Newtonsoft.Json.Formatting.Indented));
			}

			//Opdracht 2
			opdracht2();

			Console.ReadKey();
		}

		private static void opdracht1(bool execute = true, bool print = false) {
			if (execute) {
				if (print) Console.WriteLine("{0,-50}{1,-15}{2,-15}", "address", "latitude", "longitude");
				foreach (Point bar in Point.bars) {
					bar.generateLatitudeLngitde();
					if (print) Console.WriteLine("{0,-50}{1,-15}{2,-15}", bar.address, bar.latitude, bar.longitude);
				}
			}
		}

		private static void opdracht2(bool execute = true, bool print = false) {
			if (execute) {
				//https://maps.googleapis.com/maps/api/directions/json?origin=51.0267325,4.47645&destination=51.0958877,4.5059106&key=AIzaSyA62NKcfzfhHJakBTUscjrWsN_OCmtMzWs
			}
		}

		public static JToken getJTokenfromURL(string uri) {
			if (cache.ContainsKey(uri)) {
				cacheUsed = true;
				return cache[uri];
			} else {
				HttpClient httpClient = new HttpClient { BaseAddress = new Uri(uri) };
				string json = httpClient.GetStringAsync("").Result;

				//Issue #1: 
				JObject Jobject = JObject.Parse("{data: " + json + "}");

				cacheRewrite = true;
				cache.Add(uri, Jobject["data"]);


				return Jobject["data"];
			}
		} 
	}
}