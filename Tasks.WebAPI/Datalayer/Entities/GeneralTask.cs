using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.WebAPI.Datalayer.Entities
{
    /// <summary>
    /// GeneralTask Entity for database
    /// </summary>
    [Table("GeneralTasks")]
    public class GeneralTask
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength(30,ErrorMessage = "Max length of name can be 30 characters"),MinLength(3,ErrorMessage = "Name must be at-least 3 chars long")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Required]
        [MaxLength(300,ErrorMessage = " Maximum length of a description is 300 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        [Required]
        public GeneralTaskPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Required]
        public GeneralTaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

    }

    /// <summary>
    /// GeneralTaskPriority enumeration for usage with database
    /// </summary>
    public enum GeneralTaskPriority
    {
        [Display(Name = "High")] High = 0,
        [Display(Name = "Middle")] Middle = 1,
        [Display(Name = "Low")] Low = 2,
    }

    /// <summary>
    /// GeneralTaskStatus enumeration for usage with database
    /// </summary>
    public enum GeneralTaskStatus
    {
        [Display(Name = "New")] New = 0,
        [Display(Name = "In Progress")] InProcess = 1,
        [Display(Name = "Finished")] Finished = 2,
    }
}
