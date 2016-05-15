using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ImageAnalyzer;

namespace ImageAnalyzerGUI {
	class Storage {
		private IMongoClient _client;
		private IMongoDatabase _database;
		private IMongoCollection<Profile> _profiles;
		private IMongoCollection<Module> _modules;

		public Storage () {

			_client = new MongoClient();
			_database = _client.GetDatabase("work");
			_profiles = _database.GetCollection<Profile>("profiles");
			_modules = _database.GetCollection<Module>("modules");
		}

		public void AddModule (Module module) {
			_modules.InsertOne(module);
		}

		public void AddProfiles (List<Profile> profiles) {
			_profiles.InsertMany(profiles);
		}
	}
}
