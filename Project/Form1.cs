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
			int scaleY = 600;
			int scaleX = -1800;
			int transY1 = -3;
			int transX1 = -50;
			int transY2 = -700;
			int transX2 = 2250;

			List<Point> showingPoints = TSP.topPath.order;

			var g = e.Graphics;
			Pen pen = new Pen(Color.Red);
			for (int i = 0;i < showingPoints.Count - 1;i++) {
				g.DrawLine(pen,
					((float)showingPoints[i].latitude + transX1) * scaleX + transX2,
					((float)showingPoints[i].longitude + transY1) * scaleY + transY2,
					((float)showingPoints[i + 1].latitude + transX1) * scaleX + transX2,
					((float)showingPoints[i + 1].longitude + transY1) * scaleY + transY2
				);
			}
			for (int i = 0;i < showingPoints.Count;i++) {
				var centerX = ((float)showingPoints[i].latitude + transX1) * scaleX + transX2;
				var centerY = ((float)showingPoints[i].longitude + transY1) * scaleY + transY2;
				var radius = 10f;
				g.DrawEllipse(pen, 
					centerX - radius, 
					centerY - radius, 
					radius + radius, 
					radius + radius);
				g.DrawString(
					showingPoints[i].id.ToString(),
					new Font("Arial", 20),
					new SolidBrush(Color.PaleVioletRed),
					new PointF(centerX,centerY)
				);
			}

			pen.Dispose();
			e.Graphics.Dispose();
		}
	}
}
