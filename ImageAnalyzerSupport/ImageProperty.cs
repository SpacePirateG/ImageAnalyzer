using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageAnalyzer {
	public class ImageProperty {

		public ObjectId Id {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		[BsonRepresentation(BsonType.String)]
		public TypeEnum Type {
			get;
			set;
		}

		public string Value {
			get;
			set;
		}

		//public ImageProperty(string name, string type, string value)
		//{
		//	this.Name = name;
		//	this.Type = type;
		//	this.Value = value;
		//}
	}

	public enum TypeEnum {
		INT,
		STRING,
		DOUBLE,
		BOOL
	}
}
