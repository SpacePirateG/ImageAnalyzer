﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageAnalyzer {
	[BsonIgnoreExtraElements]
	public class ImageInfo {

		public string Module {
			get;
			set;
		}

		public IEnumerable<ImageProperty> Properties {
			get;
			set;
		}
	}
}
