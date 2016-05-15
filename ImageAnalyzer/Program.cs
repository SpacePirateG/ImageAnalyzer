using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;



namespace ImageAnalyzer
{
	class Program
	{
		static ImageProvider imageProvider;
		static List<IAnalyzeModule> analyzeModules = new List<IAnalyzeModule>();

		static string[] getModulesPaths()
		{

			List<String> paths = new List<string>(){
				@"c:\Projects\ImageAnalyzer\bin\Debug\SizeModule.dll"
			};

			return paths.ToArray<String>();
		}

		static void Main(string[] args)
		{
			imageProvider = new ImageProvider();
			//TestDB().Wait();
			loadAllModules();
			runModules();
			while (true) {}
		}

		//static private async Task TestDB () {

		//	var moduleName = "testStrangeModule";
		//	Image img = await imageProvider.getNextRawImage(moduleName);
		//	Console.WriteLine(img);

		//	ImageInfo imageInfo = new ImageInfo(){
		//		Module = moduleName
		//	};

		//	List<ImageProperty> imageProps = new List<ImageProperty>(){
		//		new ImageProperty(){
		//			Name = "xa",
		//			Type = TypeEnum.DOUBLE,
		//			Value = "0.11"
		//		},
		//		new ImageProperty(){
		//			Name = "control",
		//			Type = TypeEnum.BOOL,
		//			Value = "true"
		//		}
		//	};

		//	imageInfo.Properties = imageProps;

		//	await imageProvider.addImageInfo(img, imageInfo);
		//	Console.WriteLine("good");
		//}


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
			while (true) {
				Image image = await imageProvider.getNextRawImage(module.ModuleName);

				byte[] raw = Convert.FromBase64String(image.Data);
				ImageInfo imageInfo =await module.Analyze(raw);

				await imageProvider.addImageInfo(image, imageInfo);
			}
		}

	}

}