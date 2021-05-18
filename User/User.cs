using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
    public class User
    {
        public string userId { get; set; }
        public string accountType { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string organisationId { get; set; }

        public User(string userId, string accountType, string email, string name, string organisationId)
        {
            this.userId = userId;
            this.accountType = accountType;
            this.email = email;
            this.name = name;
            this.organisationId = organisationId;
        }

        public User()
        {

        }

        public static User deserialiseAsUser(Dictionary<string, AttributeValue> userItm)
        {
            var user = new User();

            user.userId = userItm["userId"].S;
            user.accountType = userItm["accountType"].S;
            user.email = userItm["email"].S;
            user.name = userItm["name"].S;
            user.organisationId = userItm["organisationId"].S;

            return user;
        }
    }
}
