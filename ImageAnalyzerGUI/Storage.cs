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
			_profiles.Indexes.DropAll();
			_profiles.Indexes.CreateOne(Builders<Profile>.IndexKeys.Ascending("url"), new CreateIndexOptions() { Unique = true });
			_modules = _database.GetCollection<Module>("modules");
			_modules.Indexes.DropAll();
			_modules.Indexes.CreateOne(Builders<Module>.IndexKeys.Ascending("path"), new CreateIndexOptions() { Unique = true });
			_images = _database.GetCollection<Image>("images");
		}

		public void AddModule (Module module) {
			try {
				_modules.InsertOne(module);
			}
			catch (MongoWriteException ex) {}
		}

		public void AddProfiles (List<Profile> profiles) {
			foreach (var profile in profiles) {
				try {
					_profiles.InsertOne(profile);
				}
				catch (MongoWriteException ex) {}
			}
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
