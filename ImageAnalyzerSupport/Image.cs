using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageAnalyzer {
	[BsonIgnoreExtraElements]
	public class Image {

		public ObjectId Id {
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
	}
}
