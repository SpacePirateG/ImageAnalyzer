﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageAnalyzer;
using System.Drawing;
using System.IO;

namespace SizeModule {
	public class SizeModule: IAnalyzeModule {

		Bitmap image;

		public async Task<ImageInfo> Analyze (byte[] image) {
			this.image = new Bitmap(new MemoryStream(image));

			ImageInfo imageInfo = new ImageInfo(){
				Module = ModuleName
			};

			List<ImageProperty> imageProps = new List<ImageProperty>(){
				new ImageProperty(){
					Name = "width",
					Value = this.image.Width
				},
				new ImageProperty(){
					Name = "height",
					Value = this.image.Height
				}
			};

			imageInfo.Properties = imageProps;

			return imageInfo;
		}


		public string ModuleName {
			get {
				return "SizeModule";
			}
		}
	}
}
