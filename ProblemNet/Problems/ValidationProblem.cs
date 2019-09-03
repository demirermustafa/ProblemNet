using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace ProblemNet.Problems
{
    public class ValidationProblem
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "messages")]
        public List<string> Messages { get; set; } = new List<string>();

        public ValidationProblem()
        {
        }

        public ValidationProblem(string field, params string[] messages)
        {
            Field = field;
            Messages = messages.ToList();
        }

        public ValidationProblem(string field, List<string> messages)
        {
            Field = field;
            Messages = messages;
        }
    }
}
