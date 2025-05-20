using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace codegen.Helpers
{
    public class APIExceptionHelper
    {
        public static ErrorDetails CreateExceptionFromResponseErrors(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject = new ErrorDetails();

            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            return deserializedErrorObject;
        }
    }

    public class ErrorDetails
    {
        public ErrorDetails()
        {
        }

        public int StatusCode { get; set; }
        public string Message
        {
            get
            {
                var result = string.Empty;
                if (Error.Count > 0)
                {
                    foreach (var item in Error)
                    {
                        result = $"{result} {item}";
                    }
                }

                return result;
            }
        }
        public Exception Exception { get; set; }
        public List<string> Error { get; set; }

    }
}
