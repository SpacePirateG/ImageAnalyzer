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
using System.Configuration;
using System.Reflection;
using ImageAnalyzer;

namespace ImageAnalyzerGUI {
	/// <summary>
	/// Логика взаимодействия для Monitor.xaml
	/// </summary>
	public partial class Monitor : Window {
		
		private Task task;
		private bool stopState = false;
		public Monitor () {
			InitializeComponent();

			task = new Task(Watch);
			task.Start();
		}

		private static bool UpdateModuleNames(Storage storage, Dictionary<string, string> moduleNames){
			List<String> modulePaths = storage.GetModulesPaths();
			bool changed = modulePaths.Count != moduleNames.Count;

			foreach (string modulePath in modulePaths) {
				if (moduleNames.Keys.Contains(modulePath))
					continue;

				Assembly assembly = Assembly.LoadFile(modulePath);
				foreach (var type in assembly.ExportedTypes) {
					if (typeof(IAnalyzeModule).IsAssignableFrom(type)) {
						moduleNames.Add(modulePath, ((IAnalyzeModule)Activator.CreateInstance(type)).ModuleName);
						changed = true;
					}
				}

			}

			return changed;
		}

		public void Watch () {
			Storage storage = new Storage();
			Dictionary<string, string> moduleNames = new Dictionary<string, string>();

			while (!stopState) {
				bool modulesChanged = UpdateModuleNames(storage, moduleNames);
				if(modulesChanged)
					Modules.Dispatcher.BeginInvoke(new Action(() => {
						Modules.ItemsSource = moduleNames.Values;
						Modules.Items.Refresh();
					})).Wait();
				
				object selectedItem = null;
				Modules.Dispatcher.BeginInvoke(new Action(() => selectedItem = Modules.SelectedItem)).Wait();

				if (selectedItem!=null) {
					var moduleName = selectedItem.ToString();
					var countAnalyzed = storage.GetAnalyzedCount(moduleName).ToString();

					AnalyzedCount.Dispatcher.BeginInvoke(new Action(() => AnalyzedCount.Text = countAnalyzed));
				}

				var countImg = storage.GetImagesCount().ToString();
				ImgCount.Dispatcher.BeginInvoke(new Action(() => ImgCount.Text = countImg));

				Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["monitorInterval"]));
			}
			
		}

		private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
			stopState = true;
		} 
	}
}
