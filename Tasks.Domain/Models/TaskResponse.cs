using System.Net;
using System.Text.Json.Serialization;


namespace Tasks.Domain.Models
{
    /// <summary>
    /// API response domain model
    /// </summary>
    public class TaskResponse
    {
        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        /// <value>
        /// The payload.
        /// </value>
        [JsonPropertyName("_payload")]
        public Payload Payload { get; set; }

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>
        /// The flags.
        /// </value>
        [JsonPropertyName("_flags")]
        public Flags Flags { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonPropertyName("_message")]
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResponse"/> class.
        /// </summary>
        public TaskResponse()
        {
            Payload = new Payload();
            Flags = new Flags();
        }
    }
    /// <summary>
    /// Payload consists of entire business object in JSON format
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public List<TaskModel> tasks { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Payload"/> class.
        /// </summary>
        public Payload()
        {
            tasks = new List<TaskModel>();
        }
    }

    /// <summary>
    /// All flags which can be used by front end system can be appended here
    /// </summary>
    public class Flags
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        [JsonPropertyName("StatusCode")]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has success; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("HasSuccess")]
        public bool HasSuccess { get; set; }

    }

}
