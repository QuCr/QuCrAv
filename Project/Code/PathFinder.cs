using System;
using System.Collections.Generic;

namespace QuCrAv {
	public class PathFinder {
		public enum Profile : int {
            /// <summary> Uses Haversine's formula to calculate the distance </summary>
            /// <remarks> This method uses 'as the crow flies' distances </remarks>			
            /// <remarks> This method is far faster, because it calculates its distances </remarks>
            HAVERSINE = 0,
			/// <summary> Uses Google Maps API to get the distance </summary>
			/// <remarks> This method uses the roadways </remarks>
			/// <remarks> This method is far slower, because it reads its distances </remarks>
			GOOGLEMAPS = 1,
            /// <summary> To see the best results </summary>
            BEST = 2,
            /// <summary> For debugging </summary>
            DEBUG = 3
        }

        /// <summary> Says how the distances will be calculated </summary>
        public string title;

        /// <summary> Says how the distances will be calculated </summary>
        public Profile profile;
		public Path shortestPath;
		public float capacity;
		public int goalDistance;

		List<Path> paths = new List<Path>();
		Path populationPath;

		int generationID = 0;
		static Random random = new Random();

		int[] exponentialDeadRange = new int[4] { 15, 10, 15, 1 };
		int[] pathCount = new int[4] { 2000, 500, 5000, 2 };
		int[] maxGenerations = new int[4] { 100, 50, 1000, 1 };

		public PathFinder(bool print, string title, Profile profile, int goalDistance = 0, float capacity = int.MaxValue) {
			this.goalDistance = goalDistance;
			this.capacity = capacity;
			this.profile = profile;
			this.title = title;

			shortestPath = new Path(this);

			for (int i = 0;i < pathCount[(int)profile];i++) {
				paths.Add(new Path(this));
			}

			while (shortestPath.distance > goalDistance + 1 && generationID < maxGenerations[(int)profile]) {
				generationID++;

				calculateFitness();
				normalizeFitness();
				paths = nextGeneration();

				if (print && (generationID % 10 == 0 || profile == Profile.GOOGLEMAPS))
					Console.WriteLine($"Generation {generationID} of {maxGenerations[(int)profile]}:\tDistance: {shortestPath.distance}m");
			}

			if (print) {
				shortestPath.prepareCalculationDistance(); 
				Console.Write("\nURL\n***\nhttps://www.google.be/maps/dir/");
				foreach (Point item in shortestPath.order) {
					Console.Write(item.address + "/");
				}
				Console.WriteLine("\n\n");
			}
		}

		private void calculateFitness() {

			int bestPathOfGenerationDistance = int.MaxValue;

			foreach (Path path in paths) {
				double distance;

                path.prepareCalculationDistance();

                if (profile == Profile.HAVERSINE || profile == Profile.BEST || profile == Profile.DEBUG)
					distance = path.calculateHaversineDistance();
				else if (profile == Profile.GOOGLEMAPS)
					distance = path.getDistanceFromGoogleMaps(profile);
                else throw new Exception("Enum handler case is not handled");

                path.cleanupCalculationDistance();

				
				if (distance < shortestPath.distance)			shortestPath = path;
				if (distance < bestPathOfGenerationDistance)	populationPath = path;
				
				path.fitness = 1 / (Math.Pow(distance, exponentialDeadRange[(int)profile]) + 1);
			}
		}


		private void normalizeFitness() {
			double sumFitness = 0;
			
			foreach (Path path in paths)
				sumFitness += path.fitness;
			foreach (Path path in paths)
				path.fitness = path.fitness / sumFitness;
		}

		private List<Path> nextGeneration() {
			List<Path> newPaths = new List<Path>();
            foreach (Path path in paths) {
				//Puts a piece of a path into another one
				Path pathA = chooseOne(paths);
				Path pathB = chooseOne(paths);

				int start = random.Next(Point.points.Count);
				int end = random.Next(start + 1, Point.points.Count);

				Path newPath = new Path(this, pathA.order.GetRange(start, end - start).ToArray());
				foreach (Point point in pathB.order) {
					if (!newPath.order.Contains(point))
						newPath.order.Add(point);
				}
				newPaths.Add(newPath);
            }
			return newPaths;
		}

		/// <summary> Picks a single path </summary>
		/// <remarks> Probability of a path being picked is based on its normalized fitness. </remarks>
		private Path chooseOne(List<Path> paths) {
			int index = 0;
			double random = PathFinder.random.NextDouble();

			while (random > 0) {
				random = random - paths[index++].fitness;
			}

			return paths[index - 1];
		}
	}
}