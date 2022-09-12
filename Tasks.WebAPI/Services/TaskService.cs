using System.Net;
using Microsoft.EntityFrameworkCore;
using Tasks.Domain.Models;
using Tasks.Domain.Validations;
using Tasks.WebAPI.Datalayer;
using Tasks.WebAPI.Datalayer.Entities;
using Tasks.WebAPI.Interfaces;
using Tasks.WebAPI.Mappings;

namespace Tasks.WebAPI.Services
{
    /// <summary>
    /// Implementation of ITaskService
    /// </summary>
    /// <seealso cref="Tasks.WebAPI.Interfaces.ITaskService" />
    public class TaskService:ITaskService
    {
        private readonly TasksDbContext _tasksDbContext;
        private readonly IConfiguration _configuration;
        private readonly IValidationService _validationService;
   
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="tasksDbContext">The tasks database context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="validationService">The validation service.</param>
        public TaskService(TasksDbContext tasksDbContext, IConfiguration configuration, IValidationService validationService)
        {
            _tasksDbContext = tasksDbContext;
            _configuration = configuration;
            _validationService = validationService;
        }

        /// <summary>
        /// Get all tasks from data layer
        /// </summary>
        /// <returns></returns>
        public async Task<TaskResponse> GetTasks()
        {
            TaskResponse response = new TaskResponse();
            try
            {
                var allTasks = await _tasksDbContext.Tasks.ToListAsync();

                    foreach (var task in allTasks)
                    {
                        response.Payload.tasks.Add(Mapper.MapDtoToTaskModel(task));
                    }

                response.Flags.HasSuccess = true;
                response.Flags.StatusCode = HttpStatusCode.OK;
                response.Message="Get Task has succeeded";
            }
            catch (Exception exception)
            {
                response.Flags.HasSuccess = false;
                response.Flags.StatusCode= HttpStatusCode.BadRequest;
                response.Message=exception.ToString();
            }

            return response;
        }

        /// <summary>
        /// Add a new task
        /// </summary>
        /// <param name="newTask"></param>
        /// <returns></returns>
        public async Task<TaskResponse> AddTask(TaskModel newTask)
        {
            TaskResponse response = await ValidateInputs(newTask);
            if (!response.Flags.HasSuccess)  return response;
            
            try
            {
                GeneralTask generalTask = Mapper.MapInsertTaskModelToDto(newTask);
                _tasksDbContext.Tasks.Add(generalTask);
               var isSuccess= await _tasksDbContext.SaveChangesAsync() > 0;

               if (isSuccess)
               {
                   response.Flags.HasSuccess = true;
                   response.Flags.StatusCode = HttpStatusCode.OK;
                   response.Message="Add Task has succeeded";
               }
               else
               {
                   response.Flags.HasSuccess = false;
                   response.Flags.StatusCode = HttpStatusCode.InternalServerError;
                   response.Message="Add Task has failed";
               }

            }
            catch (DbUpdateException exception)
            {
                response.Flags.HasSuccess = false;
                response.Flags.StatusCode= HttpStatusCode.BadRequest;
                response.Message=exception.ToString();
            }  
            return response;

        }

        /// <summary>
        /// Update a existing task
        /// </summary>
        /// <param name="updateTask"></param>
        /// <returns></returns>
        public async Task<TaskResponse> UpdateTask(TaskModel updateTask)
        {
            var response = await ValidateInputs(updateTask);
            if (!response.Flags.HasSuccess)  return response;

            try
            {
                GeneralTask generalTask = Mapper.MapTaskModelToDto(updateTask);
                if (await _tasksDbContext.Tasks.AnyAsync(taskDbRecord => taskDbRecord.Id == updateTask.Id))
                {
                    _tasksDbContext.Tasks.Update(generalTask);
                    var isSuccess= await _tasksDbContext.SaveChangesAsync() > 0;

                    if (isSuccess)
                    {
                        response.Flags.HasSuccess = true;
                        response.Flags.StatusCode = HttpStatusCode.OK;
                        response.Message="Task update has succeeded !";
                    }
                    else
                    {
                        response.Flags.HasSuccess = false;
                        response.Flags.StatusCode = HttpStatusCode.InternalServerError;
                        response.Message="Task update has failed.";
                    }    
                }
                else
                {
                    response.Flags.HasSuccess = false;
                    response.Flags.StatusCode = HttpStatusCode.NotFound;
                    response.Message="Record not found, Cannot update ";
                }
            }
            catch (DbUpdateException exception)
            {
                response.Flags.HasSuccess = false;
                response.Flags.StatusCode= HttpStatusCode.BadRequest;
                response.Message=exception.ToString();
            }

            return response;
        }

        /// <summary>
        /// Validates the inputs.
        /// </summary>
        /// <param name="updateTask">The update task.</param>
        /// <returns></returns>
        private async Task<TaskResponse> ValidateInputs(TaskModel updateTask)
        {
            TaskResponse response = new TaskResponse();

            if (!_validationService.IsDateInFuture(updateTask.DueDate))
            {
                response.Flags.HasSuccess = false;
                response.Flags.StatusCode = HttpStatusCode.Forbidden;
                response.Message = "Due date can't be a past date";
                return response;
            }

            if (!await IsNotExceedingUnFinishedTasks(updateTask.Priority, updateTask.Status, updateTask.DueDate))
            {
                response.Flags.HasSuccess = false;
                response.Flags.StatusCode = HttpStatusCode.Forbidden;
                response.Message = "Reached max number of unfinished task with high priority, cannot insert or update new task";
                return response;
            }

            response.Flags.HasSuccess = true;
            return response;
        }

        /// <summary>
        /// Determines whether [is not exceeding un finished tasks] [the specified priority].
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="status">The status.</param>
        /// <param name="dueDate">The due date.</param>
        /// <returns>
        ///   <c>true</c> if [is not exceeding un finished tasks] [the specified priority]; otherwise, <c>false</c>.
        /// </returns>
        private async Task<bool> IsNotExceedingUnFinishedTasks(TaskPriorityModelEnum priority, TaskStatusModelEnum status, DateTime dueDate)
        {
            int.TryParse(_configuration.GetSection("TaskAppSettings").GetSection("MaxUnFinishedHighPriorityTasks").Value, out var maxAllowedUnfinishedTasks);

            if (_validationService.IsValidPriority(priority, status))
            {
                var recordCountByDueDate = await _tasksDbContext.Tasks.CountAsync(task =>
                                                                                                task.Status != GeneralTaskStatus.Finished
                                                                                                && task.Priority == GeneralTaskPriority.High
                                                                                                && task.DueDate.Date == dueDate.Date);
                if (recordCountByDueDate > maxAllowedUnfinishedTasks)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
