using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetProperties
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<object> FunctionHandler(object input, ILambdaContext context)
        {
            LambdaLogger.Log("Input: " + input.ToString());

            var inp = JsonConvert.DeserializeObject<Dictionary<string, object>>(input.ToString());
            var vars = inp["path"].ToString().Split("/").Last();

            var properties = await GetProperties.RetrieveProperties(vars.Split("_")).ConfigureAwait(true);

            var resp = JsonConvert.SerializeObject(properties);
            LambdaLogger.Log("Output: " + resp);

            var response = new Dictionary<string, object>();
            response.Add("statusCode", 200);
            response.Add("headers", new Dictionary<string, object>(){
                { "Content-Type", "application/json" } ,
                { "Access-Control-Allow-Origin", "*" }
            });
            response.Add("body", resp);
            response.Add("isBase64Encoded", false);

            LambdaLogger.Log("Response: " + JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
