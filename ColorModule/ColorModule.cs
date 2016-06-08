using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageAnalyzer;
using System.IO;

namespace ColorModule {
	public class ColorModule : IAnalyzeModule {
		Bitmap image;

		private Dictionary<int, int> CollectColors(){
			Dictionary<int, int> usedColors = new Dictionary<int, int>();

			for (int i = 0; i < image.Size.Width; i++) {
				for (int j = 0; j < image.Size.Height; j++) {
					
					int color = image.GetPixel(i, j).ToArgb();
					if (usedColors.ContainsKey(color))
						usedColors[color]++;
					else
						usedColors.Add(color, 1);
				}
			}

				return usedColors;
		}

		private ImageProperty GetCountColors (Dictionary<int, int> usedColors) {
			return new ImageProperty() {
				Name = "countColors",
				Type = "INT",
				Value = usedColors.Count().ToString()
			};
		}

		private IEnumerable<ImageProperty> GetMostUsedColors (Dictionary<int, int> usedColors) {
			List<ImageProperty> properties= new List<ImageProperty>();
			var propertyNames = new string[]{"first", "second", "third"};

			var colors = usedColors.OrderByDescending(elem => elem.Value).Take(propertyNames.Count()).ToDictionary(elem => elem.Key, elem => elem.Value);

			for(int i=0; i < colors.Count(); i++){
				properties.Add(new ImageProperty() {
					Name = propertyNames[i] + "MostUsedColor",
					Type = "INT_RGB",
					Value = colors.ElementAt(i).Key.ToString()
				});
			}

			return properties;
		}

		public async Task<ImageInfo> Analyze (byte[] image) {
			
			this.image = new Bitmap(new MemoryStream(image));

			var usedColors = CollectColors();
			List<ImageProperty> imageProps = new List<ImageProperty>();
			var countProperty = GetCountColors(usedColors);
			var mostUsedColorsProperies = GetMostUsedColors(usedColors);

			ImageInfo imageInfo = new ImageInfo() {
				Module = ModuleName
			};



			imageInfo.Properties = mostUsedColorsProperies.Concat(new List<ImageProperty>(){ countProperty });

			return imageInfo;
		}


		public string ModuleName {
			get {
				return "ColorModule";
			}
		}
	}
}