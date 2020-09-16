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
				new SolidBrush(Color.PaleVioletRed),
				new PointF(20, 20)
			);

			Pen redPen = new Pen(Color.Red);
			Pen bluePen = new Pen(Color.Blue);

			for (int i = 0;i < showingPoints.Count - 1;i++) {
				float X1, Y1;
				g.DrawLine(redPen,
					X1 = ((float)showingPoints[i].longitude + transX1) * scaleX + transX2,
					Y1 = ((float)showingPoints[i].latitude + transY1) * scaleY + transY2,
					((float)showingPoints[i + 1].longitude + transX1) * scaleX + transX2,
					((float)showingPoints[i + 1].latitude + transY1) * scaleY + transY2
				);
			}

			g.DrawLine(bluePen,
					((float)showingPoints[0].longitude + transX1) * scaleX + transX2,
					((float)showingPoints[0].latitude + transY1) * scaleY + transY2,
					((float)Point.avento.longitude + transX1) * scaleX + transX2,
					((float)Point.avento.latitude + transY1) * scaleY + transY2
			);
			g.DrawLine(bluePen,
					((float)showingPoints.Last().longitude + transX1) * scaleX + transX2,
					((float)showingPoints.Last().latitude + transY1) * scaleY + transY2,
					((float)Point.avento.longitude + transX1) * scaleX + transX2,
					((float)Point.avento.latitude + transY1) * scaleY + transY2
			);



			var centerX = ((float)Point.avento.longitude + transX1) * scaleX + transX2;
			var centerY = ((float)Point.avento.latitude + transY1) * scaleY + transY2;
			var radius = 2f;
			g.DrawEllipse(bluePen,
					centerX - radius,
					centerY - radius,
					radius + radius,
					radius + radius);

			for (int i = 0;i < showingPoints.Count;i++) {
				centerX = ((float)showingPoints[i].longitude + transX1) * scaleX + transX2;
				centerY = ((float)showingPoints[i].latitude + transY1) * scaleY + transY2;
				radius = 2f;
				g.DrawEllipse(redPen, 
					centerX - radius, 
					centerY - radius, 
					radius + radius, 
					radius + radius);
				if (showingPoints[i].id == 4) centerX += 20;
				g.DrawString(
					showingPoints[i].id.ToString(),
					new Font("Arial", 20),
					new SolidBrush(Color.Black),
					new PointF(centerX - 20, centerY - 20)
				);
			}
		}
	}
}
