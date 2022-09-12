using Tasks.Domain.Models;
using Tasks.WebAPI.Datalayer.Entities;

namespace Tasks.WebAPI.Mappings
{
    /// <summary>
    /// DTO mapper class
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Maps the dto to task model.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static TaskModel MapDtoToTaskModel(GeneralTask task)
        {
            TaskModel taskModel = new TaskModel
            {
                Name = task.Name,
                Description = task.Description,
                Id = task.Id,
                Status = (TaskStatusModelEnum) task.Status,
                DueDate = task.DueDate,
                EndDate = task.EndDate,
                StartDate = task.StartDate,
                Priority = (TaskPriorityModelEnum)task.Priority

            };
            return taskModel;
        }

        /// <summary>
        /// Maps the task model to dto.
        /// </summary>
        /// <param name="taskModel">The task model.</param>
        /// <returns></returns>
        public static GeneralTask  MapTaskModelToDto(TaskModel taskModel)
        {
            GeneralTask generalTask = new GeneralTask
            {
                Name = taskModel.Name,
                Description = taskModel.Description,
                //Id = taskModel.Id,
                Status = (GeneralTaskStatus)taskModel.Status,
                DueDate = taskModel.DueDate,
                EndDate = taskModel.EndDate,
                StartDate = taskModel.StartDate,
                Priority = (GeneralTaskPriority)taskModel.Priority,
                DateCreated = DateTime.Now
                
            };
            return generalTask;
        }
        /// <summary>
        /// Maps the insert task model to dto.
        /// </summary>
        /// <param name="taskModel">The task model.</param>
        /// <returns></returns>
        public static GeneralTask  MapInsertTaskModelToDto(TaskModel taskModel)
        {
            GeneralTask generalTask = new GeneralTask
            {
                Name = taskModel.Name,
                Description = taskModel.Description,
                Status = (GeneralTaskStatus)taskModel.Status,
                DueDate = taskModel.DueDate,
                EndDate = taskModel.EndDate,
                StartDate = taskModel.StartDate,
                Priority = (GeneralTaskPriority)taskModel.Priority,
                DateCreated = DateTime.Now
            };
            return generalTask;
        }

        /// <summary>
        /// Maps the insert task model to task model.
        /// </summary>
        /// <param name="insertTaskModel">The insert task model.</param>
        /// <returns></returns>
        public static TaskModel  MapInsertTaskModelToTaskModel(InsertTaskModel insertTaskModel)
        {
            TaskModel taskModel = new TaskModel
            {
                Name = insertTaskModel.Name,
                Description = insertTaskModel.Description,
                Status = insertTaskModel.Status,
                DueDate = insertTaskModel.DueDate,
                EndDate = insertTaskModel.EndDate,
                StartDate = insertTaskModel.StartDate,
                Priority = insertTaskModel.Priority
                
            };
            return taskModel;
        }
    }
}
