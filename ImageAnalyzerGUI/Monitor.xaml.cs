using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace ImageAnalyzerGUI {
	/// <summary>
	/// Логика взаимодействия для Monitor.xaml
	/// </summary>
	public partial class Monitor : Window {
		
		private Task task;
		private bool stopState = false;
		public Monitor (IEnumerable<string> moduleNames) {
			InitializeComponent();

			Modules.ItemsSource = moduleNames;

			task = new Task(Watch);
			task.Start();
		}

		public void Watch () {
			Storage storage = new Storage(); 
			while (!stopState) {
				object selectedItem = null;
				Modules.Dispatcher.BeginInvoke(new Action(() => selectedItem = Modules.SelectedItem)).Wait();

				if (selectedItem!=null) {
					var moduleName = selectedItem.ToString();
					var countImg = storage.GetImagesCount().ToString();
					var countAnalyzed = storage.GetAnalyzedCount(moduleName).ToString();

					ImgCount.Dispatcher.BeginInvoke(new Action(() => ImgCount.Text = countImg));
					AnalyzedCount.Dispatcher.BeginInvoke(new Action(() => AnalyzedCount.Text = countAnalyzed));
				}

				Thread.Sleep(200);
			}
			
		}

		private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
			stopState = true;
		} 
	}
}
