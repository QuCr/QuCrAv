using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace QuCrAv {
    public class Point {
        public static List<Point> points = new List<Point>();
        /// <summary> Main points are used for the start and end location </summary>
        /// <remarks> In this case: Avento </remarks>
        public static Point mainPoint;

        public int id;
        public string name;
        public int cost;
        public string street;
        public int postalCode;
        public string city;
        public string province;
        public string country;
        public double longitude;
        public double latitude;

        /// <summary> Used in URLs </summary>
        public string address => $"{street}, {postalCode} {city}";

        static Point() {
            mainPoint = new Point(0, true, "Avento", 0, "Building T, Schaliënhoevedreef 20", 2800, "Mechelen", "Antwerpen", "Belgium");
            new Point(1, false, "Buffalo Cafe", 20, "Kioskplaats 111", 2660, "Antwerpen", "Antwerpen", "Belgium");
            new Point(2, false, "Café Den Baron", 16, "Strijdersstraat 57", 2650, "Edegem", "Antwerpen", "Belgium");
            new Point(3, false, "Boerke Naas vzw", 6, "Zwanenstraat 70", 2560, "Nijlen", "Antwerpen", "Belgium");
            new Point(4, false, "‘T Biezke", 12, "Hovestraat 17", 2650, "Edegem", "Antwerpen", "Belgium");
            new Point(5, false, "Bizarr Lier", 8, "Berlarij 97", 2500, "Lier", "Antwerpen", "Belgium");
            new Point(6, false, "Brouwershuis", 18, "Ferdinand Maesstraat 60", 2550, "Kontich", "Antwerpen", "Belgium");
            new Point(7, false, "Cafe-Feestzaal Gildenhuis REET", 20, "Eikenstraat 13", 2840, "Reet", "Antwerpen", "Belgium");
            new Point(8, false, "42Brasso & 42Saga", 8, "Kiliaanstraat 2", 2570, "Duffel", "Antwerpen", "Belgium");
            new Point(9, false, "Klinkaert", 9, "Industriezone Z1B 70", 2850, "Boom", "Antwerpen", "Belgium");
            new Point(10, false, "Cafe Tramhalt", 8, "Aarschotsebaan 345-351", 2590, "Berlaar", "Antwerpen", "Belgium");
            new Point(11, false, "Den Blauwe Neus", 15, "Hollebeekstraat 56", 2840, "Rumst", "Antwerpen", "Belgium");
            new Point(12, false, "De Vijfhoek", 8, "Mechelsesteenweg 248", 2860, "Sint-Katelijne-Waver", "Antwerpen", "Belgium");
            new Point(13, false, "Cafe De Heidebloem", 8, "Lierbaan 23", 2580, "Putte", "Antwerpen", "Belgium");
            new Point(14, false, "Ankertje aan de Dijle", 10, "Vismarkt 20", 2800, "Mechelen", "Antwerpen", "Belgium");
            new Point(15, false, "Café Den Bromfiets", 8, "Harentstraat 51", 2820, "Bonheiden", "Antwerpen", "Belgium");
        }

        public Point(int id, bool mainPoint, string name, int cost, string street, int postalCode, string city, string province, string country) {
            this.id = id;
            this.name = name;
            this.cost = cost;
            this.street = street;
            this.postalCode = postalCode;
            this.city = city;
            this.province = province;
            this.country = country;

            //Main point is not added because it's not a regular point
            //Mainly because it may not be shuffled in the order
            if (!mainPoint) points.Add(this);
        }

        public Point(double longitude, double latitude) {
            this.longitude = longitude;
            this.latitude = latitude;
        }

        public PointF toPointF() {
            return new PointF((float)latitude, (float)longitude);
        }

        public void getLatLong() {
            JToken token = Program.getJSONfromURL("https://maps.googleapis.com/maps/api/geocode/json" + "?address=" + address + "&key=" + Program.APIKEY);

            if (token["error_message"] != null) throw new ArgumentException("API sleutel is geen geldige sleutel.");

            JToken location = token["results"][0]["geometry"]["location"];
            latitude = location["lat"].ToObject<double>();
            longitude = location["lng"].ToObject<double>();
        }

        public override string ToString() {
            return $"#{id} ({cost})";
        }
    }
}