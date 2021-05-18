using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PutUser
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        //public async Task<string> FunctionHandler(string input, ILambdaContext context)
        public async Task<string> FunctionHandler(object input, ILambdaContext context)
        {
            LambdaLogger.Log("Input: " + input.ToString());
            var result = await PutUser.CreateNewUser(JsonConvert.DeserializeObject<Dictionary<string, string>>(input.ToString())).ConfigureAwait(true);
            LambdaLogger.Log("Output: " + JsonConvert.SerializeObject(result));
            return JsonConvert.SerializeObject(result);
        }
    }
}
