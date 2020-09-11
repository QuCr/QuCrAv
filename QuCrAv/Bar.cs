﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HttpClientStatus;
using Newtonsoft.Json.Linq;

namespace QuCrAv {
	public class Point {
		public static List<Point> bars = new List<Point>();
		public int id;
		public string name;
		public int ordered;
		public string street;
		public int postalCode;
		public string city;
		public string province;
		public string country;
		public double? longitude = null;
		public double? latitude = null;

		public string address => $"{street}, {postalCode} {city}";

		static Point() {
			new Point("Ankertje aan de Dijle", 10, "Vismarkt 20", 2800, "Mechelen", "Antwerpen", "Belgium");
			new Point("42Brasso & 42Saga", 8, "Kiliaanstraat 2", 2570, "Duffel", "Antwerpen", "Belgium");
			new Point("Cafe-Feestzaal Gildenhuis REET", 20, "Eikenstraat 13", 2840, "Reet", "Antwerpen", "Belgium");
			new Point("Bizarr Lier", 8, "Berlarij 97", 2500, "Lier", "Antwerpen", "Belgium");
			new Point("Boerke Naas vzw", 6, "Zwanenstraat 70", 2560, "Nijlen", "Antwerpen", "Belgium");
			new Point("‘T Biezke", 12, "Hovestraat 17", 2650, "Edegem", "Antwerpen", "Belgium");
			new Point("Brouwershuis", 18, "Ferdinand Maesstraat 60", 2550, "Kontich", "Antwerpen", "Belgium");
			new Point("Buffalo Cafe", 20, "Kioskplaats 111", 2660, "Antwerpen", "Antwerpen", "Belgium");
			new Point("Cafe De Heidebloem", 8, "Lierbaan 23", 2580, "Putte", "Antwerpen", "Belgium");
			new Point("Café Den Baron", 16, "Strijdersstraat 57", 2650, "Edegem", "Antwerpen", "Belgium");
			new Point("Café Den Bromfiets", 8, "Harentstraat 51", 2820, "Bonheiden", "Antwerpen", "Belgium");
			new Point("Cafe Tramhalt", 8, "Aarschotsebaan 345-351", 2590, "Berlaar", "Antwerpen", "Belgium");
			new Point("Klinkaert", 9, "Industriezone Z1B 70", 2850, "Boom", "Antwerpen", "Belgium");
			new Point("Den Blauwe Neus", 15, "Hollebeekstraat 56", 2840, "Rumst", "Antwerpen", "Belgium");
			new Point("De Vijfhoek", 8, "Mechelsesteenweg 248", 2860, "Sint-Katelijne-Waver", "Antwerpen", "Belgium");
		
	}

		internal void generateLatitudeLngitde() {
			JToken token = Program.getJTokenfromURL(
				"https://maps.googleapis.com/maps/api/geocode/json" +
				"?address=" + address +
				"&key=AIzaSyA62NKcfzfhHJakBTUscjrWsN_OCmtMzWs"
			);

			JToken a = token["results"];
			JToken b = a[0];
			JToken c = b["geometry"];
			JToken location = c["location"];
			latitude = location["lat"].ToObject<double?>();
			longitude = location["lng"].ToObject<double?>();
		}

		public Point(string name, int ordered, string street, int postalCode, string city, string province, string country) {
			this.id = bars.Count;
			this.name = name;
			this.ordered = ordered;
			this.street = street;
			this.postalCode = postalCode;
			this.city = city;
			this.province = province;
			this.country = country;

			bars.Add(this);
		}

		public bool getLongitudeLatide(bool output) {
			HttpClient httpClient = new HttpClient {
				BaseAddress = new Uri("http://nominatim.openstreetmap.org/")
			};
			
			httpClient.DefaultRequestHeaders.Add("User-Agent", "QuCrAvento");

			Task<HttpResponseMessage> task = httpClient.GetAsync( $"search?q={address}&format=json" );
			
			HttpResponseMessage responseMessage = task.Result;
			HttpContent content = responseMessage.Content;
			Task<string> result = content.ReadAsStringAsync();

			if (output) Console.Write(string.Format("{0,55}", address));


			JObject Jobject = JObject.Parse("{data: " + result.Result + "}");

			foreach (JToken location in Jobject["data"]) {
				if (location["lat"] != null)
					latitude = location["lat"].Value<double>();
				if (location["lat"] != null)
					longitude = location["lon"].Value<double>();
				if (output) Console.WriteLine("{0,20},{1};", location["lat"], location["lon"]);
				return true;
			}

			if (output) Console.WriteLine();

			return false;
		}
	}
}
