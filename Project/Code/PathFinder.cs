using System;
using System.Collections.Generic;

namespace QuCrAv {
	/// <summary>
	/// Traveling Sales Person
	/// </summary>
	public class PathFinder {
		public static Path shortestPath = new Path();
		static Random random = new Random();

		static int generationID = 0;

		const int exponentialDeadRange = 15;
		const int pathCount = 1000;
		const int maxGeneration = 100;
		const int goalDistance = 79865;

		List<Path> paths = new List<Path>();
		
		Path populationPath;
		double mutationRate = 0.01;
		
		public void start() {
			for (int i = 0;i < pathCount;i++) {
				paths.Add(new Path());
			}

			while (shortestPath.distance > goalDistance && generationID < maxGeneration) {
				generationID++;

				calculateFitness();
				normalizeFitness();
				nextGeneration();

				if (generationID % 10 == 0)
					Console.WriteLine($"Generation {generationID} of {maxGeneration}:\tDistance: {shortestPath.distance}m");
			}

			Console.Write("Addresses:\nhttps://www.google.be/maps/dir/Avento/");
			foreach (var item in shortestPath.order) {
				Console.Write(item.address + "/");
			}
			Console.Write("Avento/");
		}

		void calculateFitness() {
			int bestPopulationPathDistance = int.MaxValue;
			var a = Point.points;
			foreach (Path path in paths) {
				path.order.Insert(0,				Point.avento);
				path.order.Insert(path.order.Count, Point.avento);
				double d = path.calculateDistance();
				path.order.Remove(Point.avento);
				path.order.Remove(Point.avento);

				if (d < shortestPath.distance) {
					shortestPath = path;
				}
				if (d < bestPopulationPathDistance) {
					populationPath = path;
				}

				path.fitness = 1 / (Math.Pow(d, exponentialDeadRange) + 1);
			}
		}


		void normalizeFitness() {
			double sumFitness = 0;
			

			foreach (Path path in paths) {
				sumFitness += path.fitness;
			}
			foreach (Path path in paths) {
				path.fitness = path.fitness / sumFitness;
			}
		}

		void nextGeneration() {
			List<Path> newPaths = new List<Path>();
			foreach (Path path in paths) {
				Path listA = pickOne(paths);
				Path listB = pickOne(paths);
				Path order = crossOver(listA, listB);
				newPaths.Add(mutate(order));
			}
			paths = newPaths;
		}

		Path mutate(Path path) {
			foreach (Point point in Point.points) {
				if (random.NextDouble() < mutationRate) {
					int indexA = random.Next(Point.points.Count);
					int indexB = (indexA + 1) % Point.points.Count;
					swap(path.order, indexA, indexB);
				}
			}

			var newPath = path;
			return newPath;
		}

		void swap(List<Point> order, int indexA, int indexB) {
			var temp = order[indexA];
			order[indexA] = order[indexB];
			order[indexB] = temp;
		}

		Path crossOver(Path path, Path listB) {
			int start = random.Next(Point.points.Count);
			int end = random.Next(start+1,Point.points.Count);
			Path newPath = new Path(path.order.GetRange(start, end - start).ToArray());
			

			foreach (Point point in listB.order) {
				if (!newPath.order.Contains(point))
					newPath.order.Add(point);
			}

			return newPath;
		}

		Path pickOne(List<Path> paths) {
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