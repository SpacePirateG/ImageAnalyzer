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
using System.Reflection;
using System.Text.RegularExpressions;

namespace ImageAnalyzerGUI {
	public partial class MainWindow : Window {

		private Process _crawler;
		private Process _analyzer;
		private Storage _storage;
		private List<string> _moduleNames;
		private Window _monitor;
		private Regex profilePattern = new Regex(ConfigurationManager.AppSettings["profilePattern"]);
		public MainWindow () {
			InitializeComponent();
			_storage = new Storage();
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
			if (_crawler != null || _analyzer != null)
				return;
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

			ImageAnalyzer.Module module = new ImageAnalyzer.Module() {
				Path = path
			};

			_storage.AddModule(module);

			if (_analyzer != null) {
				_analyzer.Kill();
				_analyzer.Start();
			}

		}

		private void AddProfiles_Click (object sender, RoutedEventArgs e) {
			string profiles = Profiles.Text;
			if (profiles == String.Empty)
				return;

			var profileStrings = profiles.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<Profile> profilesList = new List<Profile>();

			foreach (var url in profileStrings) {
				if (isValidProfile(url)) {
					profilesList.Add(new Profile() {
						Url = url,
						State = StateEnum.FREE
					});
				}
			}

			_storage.AddProfiles(profilesList);
			Profiles.Text = String.Empty;
		}

		private bool isValidProfile (string profile) {
			return profilePattern.IsMatch(profile);
		}

		private void Stop_Click (object sender, RoutedEventArgs e) {
			if (_crawler != null && !_crawler.HasExited)
				_crawler.Kill();
			_crawler = null;

			if (_analyzer != null && !_analyzer.HasExited) 
				_analyzer.Kill();
			_analyzer = null;
		}

		private void Monitor_Click (object sender, RoutedEventArgs e) {

			if (_moduleNames == null) {
				_moduleNames = new List<string>();
				List<String> modulePaths = _storage.GetModulesPaths();

				foreach (string modulePath in modulePaths) {
					Assembly assembly = Assembly.LoadFile(modulePath);
					foreach (var type in assembly.ExportedTypes) {
						if (typeof(IAnalyzeModule).IsAssignableFrom(type))
							_moduleNames.Add(((IAnalyzeModule)Activator.CreateInstance(type)).ModuleName);
					}
				}
			}

			if (_monitor != null) {
				_monitor.Focus();
			} else {
				_monitor = new Monitor(_moduleNames);
				_monitor.Closing += OnMonitorWindowClosing;
				_monitor.Show();
			}
			
		}

		public void OnMonitorWindowClosing (object sender, System.ComponentModel.CancelEventArgs e) {
			_monitor = null;
		}

		private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
			if (_monitor != null) {
				_monitor.Close();
				_monitor = null;
			}
		}

		private void Information_Click (object sender, RoutedEventArgs e) {
			new Information().ShowDialog();
		}
	}
}
