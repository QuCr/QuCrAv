using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuCrAv {
	public partial class AventoForm : Form {
		PathFinder pathFinder;

		public AventoForm(PathFinder pathFinder) {
			this.pathFinder = pathFinder;
			InitializeComponent();

			Text = pathFinder.title;
		}

		private void Form1_Paint(object sender, PaintEventArgs e) {
			List<Point> points = pathFinder.shortestPath.order;
			Graphics g = e.Graphics;
			Width = 900;
			Height = 550;

			///TEXT IN TOP LEFT CORNER
			g.DrawString(
				pathFinder.title,
				new Font("Arial", 50, FontStyle.Underline),
				new SolidBrush(Color.Black),
				new PointF(20, 10)
			);
			g.DrawString(
				pathFinder.shortestPath.distance.ToString(".m"),
				new Font("Arial", 40),
				new SolidBrush(pathFinder.shortestPath.distance < pathFinder.goalDistance + 1? Color.Green : Color.Red),
				new PointF(500, 20)
			);
			g.DrawString(
				pathFinder.shortestPath.distance < pathFinder.goalDistance + 1?
				"Kortste pad" : "Kan nog korter",
				new Font("Arial", 20),
				new SolidBrush(pathFinder.shortestPath.distance < pathFinder.goalDistance + 1? Color.Green : Color.Red),
				new PointF(550, 80)
			);
			g.DrawString(
				string.Join("\n", pathFinder.shortestPath.order),
				new Font("Consolas", 10),
				new SolidBrush(Color.Black),
				new PointF(20, 100)
			);


			///DRAWING OF THE MAP
			Pen redPen = new Pen(Color.Red);        //Used for a regular line between two points
			Pen greenPen = new Pen(Color.Green);    //Used for points going to the main point
			Pen bluePen = new Pen(Color.Blue);      //Used for points coming from the main point

			int transY1 = -50; int transX1 = -3;    //Used for translating and scaling the points
			int transX2 = -2500; int transY2 = 2500;//on the map, so that they are visible and not
			int scaleX = 2000; int scaleY = -2000;  //out of bounds.

			//Draws the line between points
			for (int i = 0;i < points.Count - 1;i++) {
				Pen pen = redPen;
				if (points[i] == Point.mainPoint) pen = bluePen;
				if (i != 0 && points[i + 1] == Point.mainPoint) pen = greenPen;

				g.DrawLine(pen,
					((float)points[i].longitude + transX1) * scaleX + transX2,
					((float)points[i].latitude + transY1) * scaleY + transY2,
					((float)points[i + 1].longitude + transX1) * scaleX + transX2,
					((float)points[i + 1].latitude + transY1) * scaleY + transY2
				);
			}

			//Drawing of the points
			float centerX = ((float)Point.mainPoint.longitude + transX1) * scaleX + transX2;
			float centerY = ((float)Point.mainPoint.latitude + transY1) * scaleY + transY2;

			//The mainpoint is marked with a blue circle
			float radius = 3f;
			g.DrawEllipse(bluePen,
				centerX - radius,
				centerY - radius,
				radius + radius,
				radius + radius
			);

			//The regular points are marked with a red circle
			for (int i = 0;i < points.Count;i++) {
				centerX = ((float)points[i].longitude + transX1) * scaleX + transX2;
				centerY = ((float)points[i].latitude + transY1) * scaleY + transY2;
				radius = 4f;
				g.DrawEllipse(redPen,
					centerX - radius,
					centerY - radius,
					radius + radius,
					radius + radius);

				//Offset of text for some points that are really close
				if (points[i].id == 4) centerX += 30;
				if (points[i].id == 2) centerX -= 10;

				g.DrawString(
					points[i].id.ToString(),
					new Font("Arial", 20),
					new SolidBrush(Color.Red),
					new PointF(centerX - 20, centerY - 20)
				);
				g.DrawString(
					points[i].cost.ToString(),
					new Font("Arial", 20),
					new SolidBrush(Color.Blue),
					new PointF(centerX - 20, centerY - 50)
				);
			}
		}
	}
}