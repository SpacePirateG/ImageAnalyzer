using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageAnalyzer {
	[BsonIgnoreExtraElements]
	public class Module {
		public ObjectId Id {
			get;
			set;
		}

		public string Path {
			get;
			set;
		}
	}
}
