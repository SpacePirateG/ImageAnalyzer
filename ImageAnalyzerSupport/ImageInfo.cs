using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ImageAnalyzer {
	public class ImageInfo {
		public ObjectId Id {
			get;
			set;
		}

		public string Module {
			get;
			set;
		}

		public IEnumerable<ImageProperty> Properties {
			get;
			set;
		}

		//public ImageInfo (string module, IEnumerable<ImageProperty> properties)
		//{
		//	this.Module = module;
		//	this.Properties = properties;
		//}
	}
}
