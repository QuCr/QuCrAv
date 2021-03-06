﻿/*
    AventoForm
        PathFinder
            Path
                methodes:
                    Haversine
                    GoogleMaps
                calculateFitness
                prepareCalculationDistance
                cleanupCalculationDistance
                normalizeFitness
                    nextGeneration
                    2x chooseOne
            showShortestPath
*/

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
        public static Profile profile = Profile.HAVERSINE;

        /// <summary> Used for storing literally cost-expensive HTTP requests  </summary>
        static Dictionary<string, JToken> cache = new Dictionary<string, JToken>();

        /// <summary> Path to cache file </summary>
        /// <remarks> ../../ is used so that the cache file is on the same level as this file. </remarks>
        static string cachePath = "../../Data/cache.json";

        private static bool cacheUsed = false;
        static bool cacheRewritten = false;
        public static string APIKEY => File.ReadAllText("../../Data/key");

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Console.WriteLine("Using profile: " + profile.ToString());
            opdracht1(execute: true, print: true);  //Lat en Lon opvragen
            opdracht2(execute: true, print: true);  //Kortste route
            opdracht3(execute: true, print: true);  //Capaciteit van 90 drankjes
            extraOpdr(execute: true, print: true);  //Echte lengtes worden gebruikt, geen lengtes in vogelvlucht
            
            #region logs
            if (cacheUsed) Console.WriteLine("USED CACHE");
            if (cacheRewritten) {
                Console.WriteLine("REWROTE CACHE");
                File.WriteAllText(cachePath, JsonConvert.SerializeObject(cache, Formatting.Indented));
            }
            #endregion

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static Program() {
            if (File.Exists(cachePath)) {
                Console.WriteLine("READ CACHE");
                cache = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(File.ReadAllText(cachePath));
            }
        }

        static void opdracht1(bool execute = true, bool print = false) {
            if (execute) {
                if (print) Console.WriteLine("\n\nOpdracht 1\n**********\n[");
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
                if (print) Console.WriteLine("\n\nOpdracht 2\n**********");
                Application.Run(new AventoForm(
                    new PathFinder(print, "Opdracht 2", profile, 68142))
                );
            }
        }

        static void opdracht3(bool execute = true, bool print = false) {
            if (execute) {
                if (print) Console.WriteLine("\n\nOpdracht 3\n**********");
                Application.Run(new AventoForm(
                    new PathFinder(print, "Opdracht 3", profile, 95873, 90)
                ));
            }
        }

        static void extraOpdr(bool execute = true, bool print = false) {
            if (execute) {
                if (print) Console.WriteLine("\n\nOpdracht 4\n**********");
                if (print) {
                    Console.WriteLine("gebruiken van Google Maps lengtes, duurt langer");
                    MessageBox.Show("Deze opdracht duurt langer, omdat deze overal de lengtes van hemzelf " +
                        "moet lezen op dezelfde plaats!");
                }

                Application.Run(new AventoForm(
                    new PathFinder(print, "GM Distance", Profile.GOOGLEMAPS, 68173)
                ));
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