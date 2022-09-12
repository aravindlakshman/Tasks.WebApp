using Tasks.Domain.Models;

namespace Tasks.WebAPI.Interfaces
{
    /// <summary>
    /// Interface to define functionalities of Task API
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Get all tasks from data layer
        /// </summary>
        /// <returns></returns>
        Task<TaskResponse> GetTasks();
        
        /// <summary>
        /// Add a new task 
        /// </summary>
        /// <param name="newTask"></param>
        /// <returns></returns>
        Task<TaskResponse> AddTask(TaskModel newTask);
        
        /// <summary>
        /// Update a existing task
        /// </summary>
        /// <param name="updateTask"></param>
        /// <returns></returns>
        Task<TaskResponse> UpdateTask(TaskModel updateTask);
    }
}
