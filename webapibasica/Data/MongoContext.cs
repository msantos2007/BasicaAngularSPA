using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace webapibasica.Data
{
    public class MongoContext
    {
        public readonly MongoClient _client;
        public readonly IMongoDatabase _database;
        public readonly GridFSBucket _bucket;
        public MongoContext()//(IOptions<DatabaseSettings> dbOptions)
        {
            //var settings = dbOptions.Value;
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("tesdb");
            _bucket = new GridFSBucket(_database);
        }
    }
}