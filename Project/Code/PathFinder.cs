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

		public static void start(bool print, float capacity = int.MaxValue) {
			PathFinder.capacity = capacity;

			for (int i = 0;i < pathCount;i++) {
				paths.Add(new Path());
			}

			while (shortestPath.distance > goalDistance && generationID < maxGeneration) {
				generationID++;

				calculateFitness();
				normalizeFitness();
				paths = nextGeneration();

				if (print && generationID % 10 == 0)
					Console.WriteLine($"Generation {generationID} of {maxGeneration}:\tDistance: {shortestPath.distance}m");
			}

			if (print) {
				shortestPath.prepareCalculationDistance(); 
				Console.Write("\n\nhttps://www.google.be/maps/dir/");
				foreach (Point item in shortestPath.order) {
					Console.Write(item.address + "/");
				}
			}
		}

		private static void calculateFitness() {
			int bestPopulationPathDistance = int.MaxValue;
			List<Point> a = Point.points;
			foreach (Path path in paths) {
				double distance = path.calculateHaversineDistance();
				
				if (distance < shortestPath.distance)		shortestPath = path;
				if (distance < bestPopulationPathDistance)	populationPath = path;
				
				path.fitness = 1 / (Math.Pow(distance, exponentialDeadRange) + 1);
			}
		}


		private static void normalizeFitness() {
			double sumFitness = 0;
			
			foreach (Path path in paths)
				sumFitness += path.fitness;
			foreach (Path path in paths)
				path.fitness = path.fitness / sumFitness;
		}

		private static List<Path> nextGeneration() {
			List<Path> newPaths = new List<Path>();
			foreach (Path path in paths)
				newPaths.Add(crossOver());
			return newPaths;
		}

		private static Path crossOver() {
			Path listA = pickOne(paths);
			Path listB = pickOne(paths);

			int start = random.Next(Point.points.Count);
			int end = random.Next(start + 1, Point.points.Count);

			Path newPath = new Path(listA.order.GetRange(start, end - start).ToArray());

			foreach (Point point in listB.order) {
				if (!newPath.order.Contains(point))
					newPath.order.Add(point);
			}

			return newPath;
		}

		private static Path pickOne(List<Path> paths) {
			int index = 0;
			double random = PathFinder.random.NextDouble();

			while (random > 0) {
				random = random - paths[index++].fitness;
			}

			return paths[index - 1];
		}
	}
}