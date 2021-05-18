using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Property
{    
    public class Property
    {
        public string address { get; set; }
        public bool formsGenerated { get; set; }
        public bool formsSigned { get; set; }
        public bool installComplete { get; set; }
        public string installDate { get; set; }
        public string[] installers { get; set; }
        public string installManager { get; set; }
        public string measureCategory { get; set; }
        public string measureType { get; set; }
        public string organisationId { get; set; }
        public string postcode { get; set; }
        public string propertyId { get; set; }
        public bool surveyComplete { get; set; }
        public string surveyDate { get; set; }
        public string tenure { get; set; }

        public Property(string address, bool formsGenerated, bool formsSigned, bool installComplete, string installDate, string[] installers, string installManager, string measureCategory, string measureType, string organisationId, string postcode, string propertyId, bool surveyComplete, string surveyDate, string tenure)
        {
            this.address = address;
            this.formsGenerated = formsGenerated;
            this.formsSigned = formsSigned;
            this.installComplete = installComplete;
            this.installDate = installDate;
            this.installers = installers;
            this.installManager = installManager;
            this.measureCategory = measureCategory;
            this.measureType = measureType;
            this.organisationId = organisationId;
            this.postcode = postcode;
            this.propertyId = propertyId;
            this.surveyComplete = surveyComplete;
            this.surveyDate = surveyDate;
            this.tenure = tenure;
        }

        public Property()
        {

        }

        public static Property deserialiseAsProperty(Dictionary<string, AttributeValue> item)
        {
            var property = new Property();

            property.address = item["address"].S;
            property.formsGenerated = item["formsGenerated"].BOOL;
            property.formsSigned = item["formsSigned"].BOOL;
            property.installComplete = item["installComplete"].BOOL;
            property.installDate = item["installDate"].S;
            property.installers = item["installers"].L.Select(s => s.S).ToArray();
            property.installManager = item["installManager"].S;
            property.measureCategory = item["measureCategory"].S;
            property.measureType = item["measureType"].S;
            property.organisationId = item["organisationId"].S;
            property.postcode = item["postcode"].S;
            property.propertyId = item["propertyId"].S;
            property.surveyComplete = item["surveyComplete"].BOOL;
            property.surveyDate = item["surveyDate"].S;
            property.tenure = item["tenure"].S;

            return property;
        }
    }
}
