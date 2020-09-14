using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace QuCrAv {

	/// <summary> Represents a cafe </summary>
	public class Point {
		public static List<Point> points = new List<Point>();
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

		/// <summary> Used in URLs </summary>
		public string address => $"{street}, {postalCode} {city}";

		static Point() {
			new Point("Ankertje aan de Dijle", 10, "Vismarkt 20", 2800, "Mechelen", "Antwerpen", "Belgium");
			new Point("‘T Biezke", 12, "Hovestraat 17", 2650, "Edegem", "Antwerpen", "Belgium");
			new Point("42Brasso & 42Saga", 8, "Kiliaanstraat 2", 2570, "Duffel", "Antwerpen", "Belgium");
			new Point("Cafe-Feestzaal Gildenhuis REET", 20, "Eikenstraat 13", 2840, "Reet", "Antwerpen", "Belgium");
			new Point("Bizarr Lier", 8, "Berlarij 97", 2500, "Lier", "Antwerpen", "Belgium");
			new Point("Boerke Naas vzw", 6, "Zwanenstraat 70", 2560, "Nijlen", "Antwerpen", "Belgium");
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

		public void getLatLong() {
			JToken token = Program.getJSONfromURL(
				"https://maps.googleapis.com/maps/api/geocode/json" +
				"?address=" + address +
				"&key=" + Program.APIKEY
			);

			if (token["error_message"] != null)
				throw new ArgumentException("API sleutel is geen geldige sleutel.");

			JToken location = token["results"][0]["geometry"]["location"];
			latitude = location["lat"].ToObject<double?>();
			longitude = location["lng"].ToObject<double?>();
		}

		public Point(string name, int ordered, string street, int postalCode, string city, string province, string country) {
			this.id = points.Count + 1;
			this.name = name;
			this.ordered = ordered;
			this.street = street;
			this.postalCode = postalCode;
			this.city = city;
			this.province = province;
			this.country = country;

			points.Add(this);
		}
	}
}