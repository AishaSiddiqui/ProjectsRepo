using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserWebApplication.Models
{
    public class UserModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string User_Name { get; set; }
        public string User_Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string User_Email { get; set; }
    }
}
