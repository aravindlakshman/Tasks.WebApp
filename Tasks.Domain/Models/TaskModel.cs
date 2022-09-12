namespace Tasks.Domain.Models
{
 /// <summary>
 /// Model class for use cases other than Insert
 /// </summary>
    public class TaskModel : InsertTaskModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

    }
}
