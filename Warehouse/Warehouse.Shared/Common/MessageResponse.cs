using System.Text.Json.Serialization;

namespace Warehouse.Shared.Common
{
    public class MessageResponse
    {
        [JsonPropertyName("message")]
        [JsonInclude]
        public string Message { get; private set; } = string.Empty;

        /// The parameterless ctor is just used to create an empty instance — then properties are filled in one by one by the deserializer
        /// Builder Method
        static public MessageResponse CreateMessage(string msg)
        {
            return new MessageResponse { Message = msg }; 
        }
    }
}
