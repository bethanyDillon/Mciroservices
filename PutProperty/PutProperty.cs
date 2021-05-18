using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using Newtonsoft.Json.Linq;
using Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutProperty
{
    public class PutProperty
    {
        public static async Task<Property.Property> CreateNewProperty(Dictionary<string, object> input)
        {
            var property = new Property.Property();
            var propertyId = Guid.NewGuid().ToString();

            try
            {
                var dynamoDbClient = DynamoDBClientCreator.CreateClient();

                var item = new Dictionary<string, AttributeValue>();

                item.Add("propertyId", new AttributeValue(propertyId));
                item.Add("address", new AttributeValue(input["address"].ToString()));
                item.Add("formsGenerated", new AttributeValue() { BOOL = Boolean.Parse(input["formsGenerated"].ToString()) });
                item.Add("formsSigned", new AttributeValue() { BOOL = Boolean.Parse(input["formsSigned"].ToString()) });
                item.Add("installComplete", new AttributeValue() { BOOL = Boolean.Parse(input["installComplete"].ToString()) });
                item.Add("installDate", new AttributeValue(input["installDate"].ToString()));
                item.Add("installers", new AttributeValue() { L = ((JArray)input["installers"]).Select(s => new AttributeValue(s.Value<string>())).ToList() });
                item.Add("installManager", new AttributeValue(input["installManager"].ToString()));
                item.Add("measureCategory", new AttributeValue(input["measureCategory"].ToString()));
                item.Add("measureType", new AttributeValue(input["measureType"].ToString()));
                item.Add("organisationId", new AttributeValue(input["organisationId"].ToString()));
                item.Add("postcode", new AttributeValue(input["postcode"].ToString()));
                item.Add("surveyComplete", new AttributeValue() { BOOL = Boolean.Parse(input["surveyComplete"].ToString()) });
                item.Add("surveyDate", new AttributeValue(input["surveyDate"].ToString()));
                item.Add("tenure", new AttributeValue(input["tenure"].ToString()));

                var request = new PutItemRequest
                {
                    TableName = TableNames.property,
                    Item = item
                };

                await dynamoDbClient.PutItemAsync(request).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to put property: " + e.ToString());
            }

            property.propertyId = propertyId;
            property.address = input["address"].ToString();
            property.formsGenerated = Boolean.Parse(input["formsGenerated"].ToString());
            property.formsSigned = Boolean.Parse(input["formsSigned"].ToString());
            property.installComplete = Boolean.Parse(input["installComplete"].ToString());
            property.installDate = input["installDate"].ToString();
            property.installers = ((JArray)input["installers"]).Select(s => s.Value<string>()).ToArray();
            property.installManager = input["installManager"].ToString();
            property.measureCategory = input["measureCategory"].ToString();
            property.measureType = input["measureType"].ToString();
            property.organisationId = input["organisationId"].ToString();
            property.postcode = input["postcode"].ToString();
            property.surveyComplete = Boolean.Parse(input["surveyComplete"].ToString());
            property.surveyDate = input["surveyDate"].ToString();
            property.tenure = input["tenure"].ToString();

            return property;
        }
    }
}
