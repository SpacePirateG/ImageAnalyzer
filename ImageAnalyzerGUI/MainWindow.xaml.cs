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

namespace ImageAnalyzerGUI {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Process _crawler;
		private Process _analyzer;
		public MainWindow () {
			InitializeComponent();
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

			_crawler.StartInfo.FileName = ConfigurationManager.AppSettings["analyzerPath"];
			_crawler.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			_crawler.Start();
		}

		private void AddPModule_Click (object sender, RoutedEventArgs e) {

		}

		private void AddProfiles_Click (object sender, RoutedEventArgs e) {

		}
	}
}
