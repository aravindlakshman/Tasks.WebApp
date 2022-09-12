using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Models;

namespace Tasks.Domain.Validations
{
    public interface IValidationService
    {
        /// <summary>
        /// Check date provided as input is not a past date
        /// </summary>
        /// <param name="inputDateTime"></param>
        /// <returns></returns>
        bool IsDateInFuture(DateTime inputDateTime);
       /// <summary>
       /// Check its not a high priority or the status is in Finished state, this combination of tasks are allowed for CRUD
       /// </summary>
       /// <param name="priority"></param>
       /// <param name="status"></param>
       /// <returns></returns>
        bool IsValidPriority(TaskPriorityModelEnum priority, TaskStatusModelEnum status);

    }
}
