using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserWebApplication.Models
{
    public class UserContext : DbContext
    {
        public const string CONNECTION_STRING_NAME = "mongodb://localhost:27017";
        public const string DATABASE_NAME = "UserManagement";
        public const string USER_COLLECTION_NAME = "Users";
        public MongoClient CLIENT = new MongoClient();
        // This is ok... Normally, they would be put into 
        // an IoC container. 
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static UserContext()
        {
            // var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            _client = new MongoClient();
            _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<UserModel> Users
        {
            get { return _database.GetCollection<UserModel>(USER_COLLECTION_NAME); }
        }
       
    }
}
