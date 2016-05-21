using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ImageAnalyzer {
	[BsonIgnoreExtraElements]
	public class Profile {
		public ObjectId Id {
			get;
			set;
		}

		[BsonElement("url")]
		public String Url {
			get;
			set;
		}

		[BsonElement("state")]
		[BsonRepresentation(BsonType.String)]
		public StateEnum State {
			get;
			set;
		}
	}

	public enum StateEnum {
		FREE,
		LOCK,
		DONE
	}
}


