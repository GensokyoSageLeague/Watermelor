using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatermelonAna {
	class HelloCV {
		public static void TestHsvPicker() {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Multiselect = false;
			if (!(ofd.ShowDialog() == DialogResult.OK)) {
				return;
			}

			Image<Hsv, Byte> hsvIm = new Image<Hsv, byte>(ofd.FileName);
			ImageViewer imview1 = new ImageViewer(hsvIm);
			imview1.Show();

			Image<Gray, Byte> hsvIm_Colorized = hsvIm.InRange(new Hsv(35, 43, 46), new Hsv(77, 255, 255));
			ImageViewer imview2 = new ImageViewer(hsvIm_Colorized);
			imview2.Show();

			Image<Gray, Byte> hsvIm_Cannized = hsvIm_Colorized.Canny(20, 40);
			ImageViewer imview3 = new ImageViewer(hsvIm_Cannized);
			imview3.Show();

			CircleF[] circles = CvInvoke.HoughCircles(hsvIm_Cannized, HoughType.Gradient, 1, 50);
			Image<Hsv, Byte> hsvIm_Fin = hsvIm.Clone();
			foreach (var circle in circles) {
				hsvIm_Fin.Draw(circle, new Hsv(0, 240, 200), 4);
			}
			ImageViewer imview4 = new ImageViewer(hsvIm_Fin);
			imview4.Show();

		}

		public static void TestPreprocess() {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Multiselect = false;
			if (!(ofd.ShowDialog() == DialogResult.OK)) {
				return;
			}
			//简单图像处理
			Mat img = CvInvoke.Imread(ofd.FileName, ImreadModes.Unchanged);
			if (img.IsEmpty) {
				Console.WriteLine("can not load the image \n");
			}
			CvInvoke.Imshow("Image", img);
			Mat grayImg = new Mat();
			//转换为灰度图像
			CvInvoke.CvtColor(img, grayImg, ColorConversion.Rgb2Gray);
			CvInvoke.Imshow("Gray Image", grayImg);
			//sobel
			Mat sobelImg = new Mat();
			CvInvoke.Sobel(grayImg, sobelImg, grayImg.Depth, 1, 0);
			//使用canny算子查找边缘
			Mat cannyImg = new Mat();
			CvInvoke.Canny(grayImg, cannyImg, 20, 40);
			CvInvoke.Imshow("Canny Image", cannyImg);
			CvInvoke.WaitKey(0);
		}

		public static void TestConvexHall() {
			#region Create some random points
			Random r = new Random();
			PointF[] pts = new PointF[200];
			for (int i = 0; i < pts.Length; i++) {
				pts[i] = new PointF((float)(100 + r.NextDouble() * 400), (float)(100 + r.NextDouble() * 400));
			}
			#endregion

			Mat img = new Mat(600, 600, DepthType.Cv8U, 3);
			img.SetTo(new MCvScalar(255.0, 255.0, 255.0));
			//Draw the points 
			foreach (PointF p in pts)
				CvInvoke.Circle(img, Point.Round(p), 3, new MCvScalar(0.0, 0.0, 0.0));

			//Find and draw the convex hull

			Stopwatch watch = Stopwatch.StartNew();
			PointF[] hull = CvInvoke.ConvexHull(pts, true);
			watch.Stop();
			CvInvoke.Polylines(
			   img,
#if NETFX_CORE
   Extensions.ConvertAll<PointF, Point>(hull, Point.Round),
#else
   Array.ConvertAll<PointF, Point>(hull, Point.Round),
#endif
   true, new MCvScalar(255.0, 0.0, 0.0));

			Emgu.CV.UI.ImageViewer.Show(img, String.Format("Convex Hull Computed in {0} milliseconds", watch.ElapsedMilliseconds));
		}
	}
}
