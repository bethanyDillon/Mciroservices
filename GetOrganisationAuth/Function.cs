using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetOrganisationAuth
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(object input, ILambdaContext context)
        {
            LambdaLogger.Log("Input: " + input.ToString());
            var auth = await GetOrganisationAuth.GetOrganisationAuthCodes().ConfigureAwait(true);
            var result = JsonConvert.SerializeObject(auth);
            LambdaLogger.Log("Output: " + result);
            return result;
        }
    }
}
