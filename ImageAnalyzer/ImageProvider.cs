using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace ImageAnalyzer {
	class ImageProvider {

			private IMongoClient _client;
			private IMongoDatabase _database;
			private IMongoCollection<Image> _collection;

			public ImageProvider () {
				initClassMaps();

				_client = new MongoClient();
				_database = _client.GetDatabase("work");
				_collection = _database.GetCollection<Image>("images");

			}

			private void initClassMaps () {
				BsonClassMap.RegisterClassMap<ImageProperty>(cm => {
					cm.AutoMap();
					cm.MapMember(c => c.Name).SetElementName("name");
					cm.MapMember(c => c.Type).SetElementName("type");
					cm.MapMember(c => c.Value).SetElementName("value");
				});


				BsonClassMap.RegisterClassMap<ImageInfo>(cm => {
					cm.AutoMap();
					cm.MapMember(c => c.Module).SetElementName("module");
					cm.MapMember(c => c.Properties).SetElementName("properties");
				});

				BsonClassMap.RegisterClassMap<Image>(cm => {
					cm.AutoMap();
					cm.MapMember(c => c.Data).SetElementName("data");
					cm.MapMember(c => c.Info).SetElementName("info");
				});
			}

			public async Task<Image> getNextRawImage (String moduleName) {
			
			var builder = Builders<Image>.Filter;
			var filter = builder.Where(elem => !elem.Info.Any(info => info.Module == moduleName));
			Image result = await _collection.Find(filter).FirstOrDefaultAsync();

			return result;
		}

			public async Task addImageInfo (Image image, ImageInfo imageInfo) {
				var filter = Builders<Image>.Filter.Eq("_id", image.Id);
				var update = Builders<Image>.Update.Push(img => img.Info, imageInfo);

				var result = await _collection.UpdateOneAsync(filter, update);
			}
		
	}
}
