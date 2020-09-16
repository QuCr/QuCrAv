using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuCrAv {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Paint(object sender, PaintEventArgs e) {
			List<Point> showingPoints = PathFinder.shortestPath.order;
			Graphics g = e.Graphics;
			Height = 600;
			Width = 1000;

			///TEXT IN TOP LEFT CORNER
			g.DrawString(
				PathFinder.shortestPath.distance.ToString(".m"),
				new Font("Arial", 20),
				new SolidBrush(PathFinder.shortestPath.distance < PathFinder.goalDistance ? Color.Green : Color.Red),
				new PointF(20, 20)
			);
			g.DrawString(
				string.Join("\n", PathFinder.shortestPath.order),
				new Font("Consolas", 10),
				new SolidBrush(Color.Black),
				new PointF(20, 60)
			);


			///DRAWING OF THE MAP
			Pen redPen = new Pen(Color.Red);        //Used for a regular line between two points
			Pen greenPen = new Pen(Color.Green);    //Used for points going to the main point
			Pen bluePen = new Pen(Color.Blue);      //Used for points coming from the main point

			int transY1 = -50; int transX1 = -3;    //Used for translating and scaling the points
			int transX2 = -2500; int transY2 = 2500;    //on the map, so that they are visible and not
			int scaleX = 2000; int scaleY = -2000; //out of bounds.

			//Draws the line between points
			for (int i = 0;i < showingPoints.Count - 1;i++) {
				Pen pen = redPen;
				if (showingPoints[i] == Point.mainPoint) pen = bluePen;
				if (i != 0 && showingPoints[i + 1] == Point.mainPoint) pen = greenPen;

				g.DrawLine(pen,
					((float)showingPoints[i].longitude + transX1) * scaleX + transX2,
					((float)showingPoints[i].latitude + transY1) * scaleY + transY2,
					((float)showingPoints[i + 1].longitude + transX1) * scaleX + transX2,
					((float)showingPoints[i + 1].latitude + transY1) * scaleY + transY2
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
			for (int i = 0;i < showingPoints.Count;i++) {
				centerX = ((float)showingPoints[i].longitude + transX1) * scaleX + transX2;
				centerY = ((float)showingPoints[i].latitude + transY1) * scaleY + transY2;
				radius = 4f;
				g.DrawEllipse(redPen,
					centerX - radius,
					centerY - radius,
					radius + radius,
					radius + radius);

				//Offset of text for some points that are really close
				if (showingPoints[i].id == 4) centerX += 30;
				if (showingPoints[i].id == 2) centerX -= 10;

				g.DrawString(
					showingPoints[i].id.ToString(),
					new Font("Arial", 20),
					new SolidBrush(Color.Red),
					new PointF(centerX - 20, centerY - 20)
				);
				g.DrawString(
					showingPoints[i].cost.ToString(),
					new Font("Arial", 20),
					new SolidBrush(Color.Blue),
					new PointF(centerX - 20, centerY - 50)
				);
			}
		}
	}
}