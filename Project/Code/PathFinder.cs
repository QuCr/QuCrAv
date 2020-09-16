using System;
using System.Collections.Generic;

namespace QuCrAv {
	/// <summary>
	/// Traveling Sales Person
	/// </summary>
	public static class PathFinder {
		public static Path shortestPath = new Path();
		static List<Path> paths = new List<Path>();
		static Path populationPath;

		public static float capacity;
		static int generationID = 0;
		static Random random = new Random();

		const int exponentialDeadRange = 15;
		const int pathCount = 1000;
		const int maxGeneration = 100;
		public const int goalDistance = 95872;

		static public void start(bool print, float capacity = int.MaxValue) {
			PathFinder.capacity = capacity;

			for (int i = 0;i < pathCount;i++) {
				paths.Add(new Path());
			}

			while (shortestPath.distance > goalDistance && generationID < maxGeneration) {
				generationID++;

				calculateFitness();
				normalizeFitness();
				nextGeneration();

				if (print && generationID % 10 == 0)
					Console.WriteLine($"Generation {generationID} of {maxGeneration}:\tDistance: {shortestPath.distance}m");
			}

			if (print) {
				shortestPath.prepareCalculationDistance(); 
				Console.WriteLine("Indices: " + string.Join(", ", shortestPath.order));
				Console.Write("Addresses:\nhttps://www.google.be/maps/dir/");
				foreach (var item in shortestPath.order) {
					Console.Write(item.address + "/");
				}
				Console.WriteLine("I9IUHYGHUYU: " + shortestPath.distance);
				shortestPath.order.Reverse();
				Console.WriteLine("I9IUHYGHUYU: " + shortestPath.distance);
				foreach (var item in shortestPath.order) {
					Console.Write(item.address + "/");
				}
			}
		}

		static void calculateFitness() {
			int bestPopulationPathDistance = int.MaxValue;
			var a = Point.points;
			foreach (Path path in paths) {
				
				double distance = path.calculateDistance();




				if (distance < shortestPath.distance) {
					shortestPath = path;
				}
				if (distance < bestPopulationPathDistance) {
					populationPath = path;
				}

				path.fitness = 1 / (Math.Pow(distance, exponentialDeadRange) + 1);
			}
		}


		static void normalizeFitness() {
			double sumFitness = 0;


			foreach (Path path in paths) {
				sumFitness += path.fitness;
			}
			foreach (Path path in paths) {
				path.fitness = path.fitness / sumFitness;
			}
		}

		static void nextGeneration() {
			List<Path> newPaths = new List<Path>();
			foreach (Path path in paths) {
				Path listA = pickOne(paths);
				Path listB = pickOne(paths);
				Path order = crossOver(listA, listB);
				newPaths.Add(order);
			}
			paths = newPaths;
		}

		static Path crossOver(Path path, Path listB) {
			int start = random.Next(Point.points.Count);
			int end = random.Next(start + 1, Point.points.Count);
			Path newPath = new Path(path.order.GetRange(start, end - start).ToArray());


			foreach (Point point in listB.order) {
				if (!newPath.order.Contains(point))
					newPath.order.Add(point);
			}

			return newPath;
		}

		static Path pickOne(List<Path> paths) {
			int index = 0;
			double r = random.NextDouble();

			while (r > 0) {
				r = r - paths[index].fitness;
				index++;
			}
			return paths[index - 1];
		}
	}
}