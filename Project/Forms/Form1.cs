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
			int transY1 = -50;
			int transX1 = -3;

			int scaleX = 2000;
			int scaleY = -2000;
			int transX2 = -2500;
			int transY2 = 2500;

			List<Point> showingPoints = PathFinder.shortestPath.order;

			Height = 600;
			Width = 1000;

			var g = e.Graphics;
			g.DrawString(
				PathFinder.shortestPath.distance.ToString(".m"),
				new Font("Arial", 20),
				new SolidBrush(PathFinder.shortestPath.distance < PathFinder.goalDistance ? Color.Green : Color.Red),
				new PointF(20, 20)
			);
			g.DrawString(
				string.Join(", ", PathFinder.shortestPath.order),
				new Font("Consolas", 10),
				new SolidBrush(Color.Black), 
				new PointF(20, 60)
			);
			Pen redPen = new Pen(Color.Red);
			Pen bluePen = new Pen(Color.Blue);
			Pen greenPen = new Pen(Color.Green);

			for (int i = 0;i < showingPoints.Count - 1;i++) {
				Pen pen = redPen;
				if (showingPoints[i] == Point.mainPoint) pen = bluePen;
				if (i != 0 && showingPoints[i+1] == Point.mainPoint) pen = greenPen;
				g.DrawLine(
					pen,
					((float)showingPoints[i].longitude + transX1) * scaleX + transX2,
					((float)showingPoints[i].latitude + transY1) * scaleY + transY2,
					((float)showingPoints[i + 1].longitude + transX1) * scaleX + transX2,
					((float)showingPoints[i + 1].latitude + transY1) * scaleY + transY2
				);
			}



			var centerX = ((float)Point.mainPoint.longitude + transX1) * scaleX + transX2;
			var centerY = ((float)Point.mainPoint.latitude + transY1) * scaleY + transY2;
			var radius = 3f;
			g.DrawEllipse(bluePen,
					centerX - radius,
					centerY - radius,
					radius + radius,
					radius + radius);

			for (int i = 0;i < showingPoints.Count;i++) {
				centerX = ((float)showingPoints[i].longitude + transX1) * scaleX + transX2;
				centerY = ((float)showingPoints[i].latitude + transY1) * scaleY + transY2;
				radius = 4f;
				g.DrawEllipse(redPen, 
					centerX - radius, 
					centerY - radius, 
					radius + radius, 
					radius + radius);
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
