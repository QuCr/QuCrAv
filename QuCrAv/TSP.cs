using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuCrAv {
	/// <summary>
	/// Traveling Sales Person
	/// </summary>

	public class TSP {
		int exponentialDeadRange = 8;
		int pathCount = 100;

		List<Path> paths = new List<Path>();
		
		Path topPath = new Path();
		Path bestPopulationPath;
		static Random random = new Random();
		private double mutationRate = 0.01;

		static int iterationId = 0;

		public void start() {
			for (int i = 0;i < pathCount;i++) {
				Path path = new Path();
				paths.Add(path);
			}

			for (;iterationId < 10000000;iterationId++) {

				calculateFitness();
				normalizeFitness();
				nextGeneration();

				if (iterationId % 10000 == 00)
					Console.WriteLine(iterationId + " =====================================================================");
			}

			Console.Read();
		}

		public void calculateFitness() {
			int bestPopulationPathDistance = int.MaxValue;

			foreach (Path path in paths) {
				double d = path.calculateDistance();

				if (d < topPath.distance) {
					topPath = path;
				}
				if (d < bestPopulationPathDistance) {
					bestPopulationPath = path;
				}

				path.fitness = 1 / (Math.Pow(d, exponentialDeadRange) + 1);
			}
		}


		public void normalizeFitness() {
			double sumFitness = 0;
			

			foreach (Path path in paths) {
				sumFitness += path.fitness;
			}
			foreach (Path path in paths) {
				path.fitness = path.fitness / sumFitness;


			}
			
			foreach (var item in topPath.order) {
				Console.Write("{0,3}", item.id);
			}
			Console.WriteLine("\t{0}m", topPath.distance);
			Console.ReadKey();
		}

		public void nextGeneration() {
			List<Path> newPaths = new List<Path>();
			foreach (Path path in paths) {
				Path listA = pickOne(paths);
				Path listB = pickOne(paths);
				Path order = crossOver(listA, listB);
				newPaths.Add(mutate(order));
			}
			paths = newPaths;
		}

		private Path mutate(Path path) {
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

		private void swap(List<Point> order, int indexA, int indexB) {
			var temp = order[indexA];
			order[indexA] = order[indexB];
			order[indexB] = temp;
		}

		private Path crossOver(Path path, Path listB) {
			int start = random.Next(Point.points.Count);
			int end = random.Next(start+1,Point.points.Count);
			Path newPath = new Path(path.order.GetRange(start, end - start).ToArray());
			

			foreach (Point point in listB.order) {
				if (!newPath.order.Contains(point))
					newPath.order.Add(point);
			}

			newPath.calculateDistance();
			return newPath;
		}

		private Path pickOne(List<Path> paths) {
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
