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

		[BsonElement("type")]
		[BsonRepresentation(BsonType.String)]
		public TypeEnum Type {
			get;
			set;
		}

		[BsonElement("value")]
		public string Value {
			get;
			set;
		}
	}

	public enum TypeEnum {
		INT,
		STRING,
		DOUBLE,
		BOOL
	}
}
