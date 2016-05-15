using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace ImageAnalyzer {
	class Storage {

		private IMongoClient _client;
		private IMongoDatabase _database;
		private IMongoCollection<Image> _images;
		private IMongoCollection<Module> _modules;

		public Storage () {
			//initClassMaps();

			_client = new MongoClient();
			_database = _client.GetDatabase("work");
			_images = _database.GetCollection<Image>("images");
			_modules = _database.GetCollection<Module>("modules");

		}

		public List<String> GetModulesPaths(){
			
			List<string> modulesPaths = _modules.Find(new BsonDocument())
				.ToList()
				.Select(module => module.Path)
				.ToList();

			return modulesPaths;
		}

		public async Task<Image> getNextRawImage (String moduleName) {
		
		var builder = Builders<Image>.Filter;
		var filter = builder.Where(elem => !elem.Info.Any(info => info.Module == moduleName));
		Image result = await _images.Find(filter).FirstOrDefaultAsync();

		return result;
		}

		public async Task addImageInfo (Image image, ImageInfo imageInfo) {
			var filter = Builders<Image>.Filter.Eq("_id", image.Id);
			var update = Builders<Image>.Update.Push(img => img.Info, imageInfo);

			var result = await _images.UpdateOneAsync(filter, update);
		}
		
	}
}
