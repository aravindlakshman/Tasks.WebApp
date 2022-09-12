using System.ComponentModel.DataAnnotations;


namespace Tasks.Domain.Models
{
    /// <summary>
/// Model class for Task Add usage
/// </summary>
    public class InsertTaskModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TaskPriorityModelEnum Priority { get; set; }
        public TaskStatusModelEnum Status { get; set; }
    }

    public enum TaskPriorityModelEnum
    {
        [Display(Name = "High")] High = 0,
        [Display(Name = "Middle")] Middle = 1,
        [Display(Name = "Low")] Low = 2,
    }


    public enum TaskStatusModelEnum
    {
        [Display(Name = "New")] New = 0,
        [Display(Name = "In Progress")] InProcess = 1,
        [Display(Name = "Finished")] Finished = 2,
    }
}
