using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Threading;


namespace ImageAnalyzer
{
	class Program
	{
		static Storage storage;
		static List<IAnalyzeModule> analyzeModules = new List<IAnalyzeModule>();

		static string[] getModulesPaths()
		{

			List<String> paths = storage.GetModulesPaths();

			return paths.ToArray<String>();
		}

		static void Main(string[] args)
		{
			storage = new Storage();
			loadAllModules();
			runModules();
			while (true) {}
		}


		static private void loadAllModules() {
			string[] modulesPaths = getModulesPaths();

			foreach (string modulePath in modulesPaths) {
				Assembly assembly = Assembly.LoadFile(modulePath);
				foreach (var type in assembly.ExportedTypes) {
					if (typeof(IAnalyzeModule).IsAssignableFrom(type))
						analyzeModules.Add((IAnalyzeModule)Activator.CreateInstance(type));
				}
			}
		}

		static private void runModules () {
			foreach(var module in analyzeModules) {
				new Task(() =>{
 					while(true){
						execModule(module).Wait();
					}
				}).Start();
			}
		}

		static public async Task execModule (IAnalyzeModule module) {
			Storage localStorage = new Storage();
			while (true) {
				Image image = await localStorage.getNextRawImage(module.ModuleName);

				byte[] raw = Convert.FromBase64String(image.Data);
				ImageInfo imageInfo =await module.Analyze(raw);

				await localStorage.addImageInfo(image, imageInfo);
				Console.WriteLine(module.ModuleName + " done: " + image.Id);
				//Thread.Sleep(5000);
			}
		}

	}

}