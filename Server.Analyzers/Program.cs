using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatermelonAna {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			//new FormMain().ShowDialog();
			//HelloCV.TestConvexHall();
			//HelloCV.TestPreprocess();
			HelloCV.TestHsvPicker();
			loop();
		}

		public static void loop() {
			while (true) {
				Application.DoEvents();
			}
		}
	}
}
