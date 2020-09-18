using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using static QuCrAv.PathFinder;

namespace QuCrAv {
    class Program {
        /// <summary> Enum for choosing which method is used for calculating the distance </summary>
        const Factor factor = Factor.BEST;

        /// <summary> Used for storing literally cost-expensive HTTP requests  </summary>
        static Dictionary<string, JToken> cache = new Dictionary<string, JToken>();

        /// <summary> Path to cache file </summary>
        /// <remarks> ../../ is used so that the cache file is on the same level as this file. </remarks>
        static string cachePath = "../../Data/cache.json";

        private static bool cacheUsed = false;
        static bool cacheRewritten = false;
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

            opdracht1(true, true);
            opdracht2(true, true);
            opdracht3(true, true);
            extraOpdr(true, true);

            #region logs
            if (cacheUsed) Console.WriteLine("USED CACHE");
            if (cacheRewritten) {
                Console.WriteLine("REWROTE CACHE");
                File.WriteAllText(cachePath, JsonConvert.SerializeObject(cache, Formatting.Indented));
            }
            #endregion

            Console.ReadKey();
        }

        static void extraOpdr(bool execute = true, bool print = false) {
            if (execute) {
                Console.WriteLine("Extra opdracht 1: gebruiken van Google Maps lengtes, duurt langer");
                MessageBox.Show("Deze opdracht duurt langer, omdat deze overal de lengtes van hemzelf " +
                    "moet lezen op dezelfde plaats!"); 
                
                PathFinder pathFinder1 = new PathFinder(print, "GM Distance", PathFinder.Factor.GOOGLEMAPS, 0, 200);
                Application.Run(form = new Form1(pathFinder1));
            }
        }

        static void opdracht1(bool execute = true, bool print = false) {
            if (execute) {
                if (print)
                    Console.WriteLine("[");
                Point.mainPoint.getLatLong();
                if (print) Console.WriteLine("\t[ {1,-11}, {2,-11}, {3,2} ],",
                    Point.mainPoint.address,
                    Point.mainPoint.latitude.ToString().Replace(',', '.'),
                    Point.mainPoint.longitude.ToString().Replace(',', '.'),
                    Point.mainPoint.id);
                foreach (Point point in Point.points) {
                    point.getLatLong();
                    if (print) Console.WriteLine("\t[ {1,-11}, {2,-11}, {3,2} ],",
                        point.address,
                        point.latitude.ToString().Replace(',', '.'),
                        point.longitude.ToString().Replace(',', '.'),
                        point.id);
                }
                if (print) Console.WriteLine("]");
            } else Console.WriteLine("Opdracht 1 werd niet uitgevoerd, daardoor konden de coordinaten niet opgehaald worden!");
        }

        static void opdracht2(bool execute = true, bool print = false) {
            if (execute) {
                PathFinder pathFinder = new PathFinder(print, "Opdracht 2", factor, 68173);
                Application.Run(form = new Form1(pathFinder));
            }
        }

        static void opdracht3(bool execute = true, bool print = false) {
            if (execute) {
                PathFinder pathFinder = new PathFinder(print, "Opdracht 3", factor, 95873, 90);
                Application.Run(form = new Form1(pathFinder));
            }
        }

        /// <summary> Gets the JSON data from the cache or the given URI. If URI is not cached, get it online! </summary>
        public static JToken getJSONfromURL(string uri) {
            try {
                if (cache.ContainsKey(uri.Replace(APIKEY, "apiKey"))) {
                    cacheUsed = true;
                    return cache[uri.Replace(APIKEY, "apiKey")];
                } else {
                    HttpClient httpClient = new HttpClient { BaseAddress = new Uri(uri) };
                    string json = httpClient.GetStringAsync("").Result;

                    //Issue #1: 
                    JObject Jobject = JObject.Parse("{data: " + json + "}");

                    cacheRewritten = true;
                    cache.Add(uri.Replace(APIKEY, "apiKey"), Jobject["data"]);

                    if (Jobject["error_message"] != null)
                        throw new ArgumentException("API sleutel is geen geldige sleutel.");

                    return Jobject["data"];
                }
            }
            catch (FileNotFoundException exception) {
                throw new FileNotFoundException(
                    "key.txt moet toegevoegd worden en een geldige Google API sleutel hebben!",
                    exception
                );
            }
        }
    }
}