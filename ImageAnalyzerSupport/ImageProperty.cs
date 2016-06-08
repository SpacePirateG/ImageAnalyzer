using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageAnalyzer {
	[BsonIgnoreExtraElements]
	public class ImageProperty {

		[BsonElement("name")]
		public string Name {
			get;
			set;
		}

		[BsonElement("value")]
		public BsonValue Value {
			get;
			set;
		}
	}
}
