using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ImageAnalyzer;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace ImageAnalyzerGUI {
	class Storage {
		private IMongoClient _client;
		private IMongoDatabase _database;
		private IMongoCollection<Profile> _profiles;
		private IMongoCollection<Module> _modules;
		private IMongoCollection<Image> _images;

		public Storage () {

			_client = new MongoClient();
			_database = _client.GetDatabase("work");
			_profiles = _database.GetCollection<Profile>("profiles");
			_modules = _database.GetCollection<Module>("modules");
			_images = _database.GetCollection<Image>("images");
		}

		public void AddModule (Module module) {
			_modules.InsertOne(module);
		}

		public void AddProfiles (List<Profile> profiles) {
			_profiles.InsertMany(profiles);
		}

		public List<String> GetModulesPaths () {

			List<string> modulesPaths = _modules.Find(new BsonDocument())
				.ToList()
				.Select(module => module.Path)
				.ToList();

			return modulesPaths;
		}

		public int GetImagesCount () {
			return (int)_images.Count(new BsonDocument());
		}

		public int GetAnalyzedCount (string moduleName) {
			var builder = Builders<Image>.Filter;
			var filter = builder.Where(elem => elem.Info.Any(info => info.Module == moduleName));
			int count = (int)_images.Count(filter);
			return count;
		}
	}
}
