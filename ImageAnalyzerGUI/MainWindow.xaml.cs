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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Configuration;
using ImageAnalyzer;

namespace ImageAnalyzerGUI {
	public partial class MainWindow : Window {

		private Process _crawler;
		private Process _analyzer;
		private Storage storage;
		public MainWindow () {
			InitializeComponent();
			storage = new Storage();
		}

		private void Browse_Click (object sender, RoutedEventArgs e) {
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".dll";
			dlg.Filter = "DLL Files|*.dll";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true) {
				ModulePath.Text = dlg.FileName;
			}
		}

		private void Start_Click (object sender, RoutedEventArgs e) {
			_crawler = new Process();

			_crawler.StartInfo.FileName = "node.exe";
			_crawler.StartInfo.Arguments = ConfigurationManager.AppSettings["crawlerPath"];
			_crawler.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			_crawler.Start();

			_analyzer = new Process();

			_analyzer.StartInfo.FileName = ConfigurationManager.AppSettings["analyzerPath"];
			_analyzer.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			_analyzer.Start();
		}

		private void AddPModule_Click (object sender, RoutedEventArgs e) {
			string path = ModulePath.Text;
			if (path == String.Empty || path.IndexOf(".dll") != path.Length - 4)
				return;
			
			path = System.IO.Path.GetFullPath(path);

			Module module = new Module() {
				Path = path
			};

			storage.AddModule(module);

			_analyzer.Kill();
			_analyzer.Start();
		}

		private void AddProfiles_Click (object sender, RoutedEventArgs e) {
			string profiles = Profiles.Text;
			if (profiles == String.Empty)
				return;

			var profileStrings = profiles.Split(new String[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
			List<Profile> profilesList = new List<Profile>();

			foreach (var url in profileStrings) {
				profilesList.Add(new Profile() {
					Url = url,
					State = "FREE"
				});
			}

			storage.AddProfiles(profilesList);
		}

		private void Stop_Click (object sender, RoutedEventArgs e) {
			if (_crawler != null && _analyzer != null) {
				_crawler.Kill();
				_analyzer.Kill();
				_crawler = null;
				_analyzer = null;
			}

		}
	}
}
