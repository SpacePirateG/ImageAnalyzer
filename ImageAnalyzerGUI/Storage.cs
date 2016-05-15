using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ImageAnalyzer;
using MongoDB.Bson.Serialization;

namespace ImageAnalyzerGUI {
	class Storage {
		private IMongoClient _client;
		private IMongoDatabase _database;
		private IMongoCollection<Profile> _profiles;
		private IMongoCollection<Module> _modules;

		public Storage () {

			initClassMaps();

			_client = new MongoClient();
			_database = _client.GetDatabase("work");
			_profiles = _database.GetCollection<Profile>("profiles");
			_modules = _database.GetCollection<Module>("modules");
		}

		private void initClassMaps () {
			BsonClassMap.RegisterClassMap<Profile>(cm => {
				cm.AutoMap();
				cm.MapMember(c => c.State).SetElementName("state");
				cm.MapMember(c => c.Url).SetElementName("url");
			});


			BsonClassMap.RegisterClassMap<Module>(cm => {
				cm.AutoMap();
				cm.MapMember(c => c.Path).SetElementName("path");
			});
		}

		public void AddModule (Module module) {
			_modules.InsertOne(module);
		}

		public void AddProfiles (List<Profile> profiles) {
			_profiles.InsertMany(profiles);
		}
	}
}
