using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DiffingAPITask.BusinessLogic.DTO
{
    public class DiffResultDTO
    {
        public ResultType DiffResultType { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>(StringComparer.Ordinal);
        
        public enum ResultType
        {
            Equals,
            SizeDoNotMatch,
            ContentDoNotMatch
        }
    }
}