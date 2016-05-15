using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ImageAnalyzer {
	public class Image {

		public ObjectId Id {
			get;
			set;
		}

		public Int32 __V {
			get;
			set;
		}

		public string Data {
			get;
			set;
		}

		public IEnumerable<ImageInfo> Info {
			get;
			set;
		}

		//public Image (string data, IEnumerable<ImageInfo> info)
		//{
		//	this.Data = data;
		//	this.Info = info;
		//}
	}
}
