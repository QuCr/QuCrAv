using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuCrAv {
	public class Bar {
		public static List<Bar> bars = new List<Bar>();
		public string Name;
		public int ordered;
		public string street;
		public int postalCode;
		public string city;
		public string province;
		public string country;
		public double? longitude = null;
		public double? latitude = null;

		public string address => $"{street}, {postalCode} {city}";

		static Bar() {
			new Bar("Ankertje aan de Dijle", 10, "Vismarkt 20", 2800, "Mechelen", "Antwerpen", "Belgium");
			new Bar("42Brasso & 42Saga", 8, "Kiliaanstraat 2", 2570, "Duffel", "Antwerpen", "Belgium");
			new Bar("‘T Biezke", 12, "Hovestraat 17", 2650, "Edegem", "Antwerpen", "Belgium");
			new Bar("Cafe-Feestzaal Gildenhuis REET", 20, "Eikenstraat 13", 2840, "Reet", "Antwerpen", "Belgium");
			new Bar("Bizarr Lier", 8, "Berlarij 97", 2500, "Lier", "Antwerpen", "Belgium");
			new Bar("Boerke Naas vzw", 6, "Zwanenstraat 70", 2560, "Nijlen", "Antwerpen", "Belgium");
			new Bar("Brouwershuis", 18, "Ferdinand Maesstraat 60", 2550, "Kontich", "Antwerpen", "Belgium");
			new Bar("Buffalo Cafe", 20, "Kioskplaats 111", 2660, "Antwerpen", "Antwerpen", "Belgium");
			new Bar("Cafe De Heidebloem", 8, "Lierbaan 23", 2580, "Putte", "Antwerpen", "Belgium");
			new Bar("Café Den Baron", 16, "Strijdersstraat 57", 2650, "Edegem", "Antwerpen", "Belgium");
			new Bar("Café Den Bromfiets", 8, "Harentstraat 51", 2820, "Bonheiden", "Antwerpen", "Belgium");
			new Bar("Cafe Tramhalt", 8, "Aarschotsebaan 345-351", 2590, "Berlaar", "Antwerpen", "Belgium");
			new Bar("Klinkaert", 9, "Industriezone Z1B 70", 2850, "Boom", "Antwerpen", "Belgium");
			new Bar("Den Blauwe Neus", 15, "Hollebeekstraat 56", 2840, "Rumst", "Antwerpen", "Belgium");
			new Bar("De Vijfhoek", 8, "Mechelsesteenweg 248", 2860, "Sint-Katelijne-Waver", "Antwerpen", "Belgium");
		
	}

		public Bar(string name, int ordered, string street, int postalCode, string city, string province, string country) {
			this.Name = name;
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
