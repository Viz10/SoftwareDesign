using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Warehouse.Shared.Common
{
    public class Message
    {
        [JsonPropertyName("message")]
        public string message { get; set; } = string.Empty;

        public Message(string message)
        {
            this.message = message;
        }

        public Message() { }

        static public Message CreateMessage(string message) => new Message(message);

    }
}
