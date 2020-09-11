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

namespace HttpClientStatus {
	class Program {
		static ScrapingBrowser _browser = new ScrapingBrowser();
		static string url = @"https://www.google.com/maps/dir/Vismarkt+20,+2800+Mechelen/Kiliaanstraat+2,+2570+Duffel/";

		static void Main(string[] args) {

			//var a = getJTokenfromURL("https://maps.googleapis.com/maps/api/directions/json", "?origin=Vismarkt%2020,%202800%20Mechelen&destination=Kiliaanstraat%202,%202570%20Duffel&key=AIzaSyA62NKcfzfhHJakBTUscjrWsN_OCmtMzWs")
				;
			//Console.ReadKey();

			//return;

			//Opdracht 1
			foreach (Bar bar in Bar.bars) {
				//bar.getLongitudeLatide(false);
				JToken token = getJTokenfromURL(
					"https://maps.googleapis.com/maps/api/geocode/json" +
					"?address=Vismarkt%2020,%202800%20Mechelen" +
					"&key=AIzaSyA62NKcfzfhHJakBTUscjrWsN_OCmtMzWs"
				);


			}

			//Opdracht 2
			foreach (Bar bar in Bar.bars) {
				Console.Write(bar.address + '\n');
			}

			Console.ReadKey();
		}

		static JToken getJTokenfromURL(string uri) {
			HttpClient httpClient = new HttpClient { BaseAddress = new Uri(uri) };
			string json = httpClient.GetStringAsync("").Result;
			
			//De 
			JObject Jobject = JObject.Parse("{data: " + json + "}");
			return Jobject["data"];
		}
	}
}