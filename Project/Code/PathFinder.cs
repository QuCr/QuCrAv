using System;
using System.Collections.Generic;

namespace QuCrAv {
	public class PathFinder {
		public enum Factor : int {
            /// <summary> Uses Haversine's formula to calculate the distance </summary>
            /// <remarks> This method uses 'as the crow flies' distances </remarks>			
            /// <remarks> This method is far faster, because it calculates its distances </remarks>
            HAVERSINE = 0,
			/// <summary> Uses Google Maps API to get the distance </summary>
			/// <remarks> This method uses the roadways </remarks>
			/// <remarks> This method is far slower, because it reads its distances </remarks>
			GOOGLEMAPS = 1,
            /// <summary> To see the best results </summary>
            BEST = 2
        }

        /// <summary> Says how the distances will be calculated </summary>
        public string title;

        /// <summary> Says how the distances will be calculated </summary>
        public Factor factor;
		public Path shortestPath;
		public float capacity;
		public int goalDistance;

		List<Path> paths = new List<Path>();
		Path populationPath;

		int generationID = 0;
		static Random random = new Random();

		int[] exponentialDeadRange = new int[3] { 15, 10, 15 };
		int[] pathCount = new int[3] { 2000, 1000, 5000 };
		int[] maxGeneration = new int[3] { 100, 50, 1000 };

		public PathFinder(bool print, string title, Factor factor, int goalDistance = 0, float capacity = int.MaxValue) {
			this.goalDistance = goalDistance;
			this.capacity = capacity;
			this.factor = factor;
			this.title = title;

			shortestPath = new Path(this);


			for (int i = 0;i < pathCount[(int)factor];i++) {
				paths.Add(new Path(this));
			}

			while (shortestPath.distance > goalDistance && generationID < maxGeneration[(int)factor]) {
				generationID++;

				calculateFitness();
				normalizeFitness();
				paths = nextGeneration();

				if (print && (generationID % 10 == 0 || factor == Factor.GOOGLEMAPS))
					Console.WriteLine($"Generation {generationID} of {maxGeneration[(int)factor]}:\tDistance: {shortestPath.distance}m");
			}

			if (print) {
				shortestPath.prepareCalculationDistance(); 
				Console.Write("\n\nhttps://www.google.be/maps/dir/");
				foreach (Point item in shortestPath.order) {
					Console.Write(item.address + "/");
				}
				Console.WriteLine("\n\n");
			}
		}

		private void calculateFitness() {
			int bestPopulationPathDistance = int.MaxValue;
			List<Point> a = Point.points;
			foreach (Path path in paths) {
				double distance;
				if (factor == Factor.HAVERSINE || factor == Factor.BEST)
					distance = path.calculateHaversineDistance();
				else if (factor == Factor.GOOGLEMAPS)
					distance = path.getDistanceFromGoogleMaps(factor);
				else throw new Exception("Enum handler case is not handled");
				
				if (distance < shortestPath.distance)		shortestPath = path;
				if (distance < bestPopulationPathDistance)	populationPath = path;
				
				path.fitness = 1 / (Math.Pow(distance, exponentialDeadRange[(int)factor]) + 1);
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
			for (int i = 0; i < paths.Count; i++) {
				//Puts a piece of a path into another one
				Path listA = chooseOne(paths);
                Path listB = chooseOne(paths);

                int start = random.Next(Point.points.Count);
                int end = random.Next(start + 1, Point.points.Count);

                Path newPath = new Path(this, listA.order.GetRange(start, end - start).ToArray());
                foreach (Point point in listB.order) {
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