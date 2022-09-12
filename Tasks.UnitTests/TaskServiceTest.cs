using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Tasks.Domain.Models;
using Tasks.Domain.Validations;
using Tasks.WebAPI.Datalayer;
using Tasks.WebAPI.Datalayer.Entities;
using Tasks.WebAPI.Interfaces;
using Tasks.WebAPI.Services;

namespace Tasks.UnitTests
{
    [TestClass]
    public class TaskServiceTest
    {
        private readonly ITaskService _taskService;
        private readonly TasksDbContext _tasksDbContext;
        private  TaskModel _taskModel;
        private readonly InsertTaskModel _insertTaskModel;
        private readonly GeneralTask _generalTask;
        private readonly IConfiguration _configuration;
        private readonly IValidationService _validationService;
        
        /// <summary>
        /// Constructor and initialization for test data
        /// </summary>
        public TaskServiceTest()
        {
            //Initialize services
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true).Build();

            _validationService = new ValidationService();

            //Initialize in-memory database and db context
            var dbContextOptions = new DbContextOptionsBuilder<TasksDbContext>()
                .UseInMemoryDatabase(databaseName: "Taskdb", new InMemoryDatabaseRoot())
                .Options;

            _tasksDbContext = new TasksDbContext(dbContextOptions);


            _taskService = new TaskService(_tasksDbContext, _configuration, _validationService);

            _generalTask = new GeneralTask
            {
                Name = "Build a bluetooth IoT device",
                Description = "Build a blue tooth low power consuming device",
                DueDate = DateTime.Now.AddDays(15).Date,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30).Date,
                Priority = GeneralTaskPriority.High,
                Status = GeneralTaskStatus.New
            };

            _insertTaskModel = new InsertTaskModel
            {
                Name = "Build a AI bot",
                Description = "Build a bot",
                DueDate = DateTime.Now.AddDays(15).Date,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30).Date,
                Priority = TaskPriorityModelEnum.High,
                Status = TaskStatusModelEnum.New
            };

            _taskModel = new TaskModel
            {
                Name = "Build a rocking chair",
                Description = "Build a chair which can rock for 30 minutes",
                DueDate = DateTime.Now.AddDays(15).Date,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30).Date,
                Priority = TaskPriorityModelEnum.High,
                Status = TaskStatusModelEnum.New
            };

            
        }
       

        [TestMethod]
        public void AddTask_ValidInput_Return_Success()
        {            
            //Assign
            var objectResponse = _taskService.AddTask(_taskModel);

            //Assert
            Assert.IsNotNull(objectResponse?.Result);
            Assert.AreEqual(true, objectResponse.Result.Flags.HasSuccess);
            Assert.AreEqual(true, objectResponse.Result.Flags.StatusCode == HttpStatusCode.OK);
            
        }

        [TestMethod]
        public void AddTask_PastDueDate_Return_Failure()
        {
            var pastDueDateTaskModel = new TaskModel
            {
                Name = "Build a rocking chair",
                Description = "Build a chair which can rock for 30 minutes",
                DueDate = DateTime.Now.AddDays(-15).Date,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30).Date,
                Priority = TaskPriorityModelEnum.High,
                Status = TaskStatusModelEnum.New
            };
            
            
            
            var objectResponse = _taskService.AddTask(pastDueDateTaskModel);

            
            Assert.AreEqual(false, objectResponse.Result.Flags.HasSuccess);
            Assert.AreEqual(true, objectResponse.Result.Flags.StatusCode==HttpStatusCode.Forbidden);            
        }

        [TestMethod]
        public void AddTask_ExceedsMaxCount_DateAndPriority_Return_Failure()
        {
            int.TryParse(_configuration.GetSection("TaskAppSettings").GetSection("MaxUnFinishedHighPriorityTasks").Value, out var maxAllowedUnfinishedTasks);

            //Arrange
            for (var i = 0; i <= maxAllowedUnfinishedTasks; i++)
            {
                _tasksDbContext.Tasks.Add(new GeneralTask
                {
                    Name = "Build a Electronic Guitar IoT device",
                    Description = "Build a Electronic Guitar device with oak wood",
                    DueDate = DateTime.Now.AddDays(15).Date,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30).Date,
                    Priority = GeneralTaskPriority.High,
                    Status = GeneralTaskStatus.New
                });
            }

            _tasksDbContext.SaveChangesAsync();

            //Assign
            var failAddTaskObjectResponse = _taskService.AddTask(_taskModel);

            //Assert
            Assert.IsNotNull(failAddTaskObjectResponse?.Result);
            Assert.AreEqual(false, failAddTaskObjectResponse.Result.Flags.HasSuccess);
            Assert.AreEqual(true, failAddTaskObjectResponse.Result.Flags.StatusCode==HttpStatusCode.Forbidden);   
        }


       


        [TestMethod]
        public void UpdateTask_PastDueDate_Return_Failure()
        {
            _taskService.AddTask(_taskModel);

            int recordId = _tasksDbContext.Tasks.FirstOrDefault(t => t.Name == _taskModel.Name && t.DueDate == _taskModel.DueDate)!.Id;
            _taskModel.Id = recordId;
            _taskModel.DueDate = DateTime.Now.AddDays(-15);

            var objectResponse = _taskService.UpdateTask(_taskModel);

            Assert.IsNotNull(objectResponse?.Result);
            Assert.AreEqual(false, objectResponse.Result.Flags.HasSuccess);
            Assert.AreEqual(true, objectResponse.Result.Flags.StatusCode == HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public void UpdateTask_ExceedsMaxCount_DateAndPriority_Return_Failure()
        {
            //Arrange
            int.TryParse(_configuration.GetSection("TaskAppSettings").GetSection("MaxUnFinishedHighPriorityTasks").Value, out var maxAllowedUnfinishedTasks);
            for (var i = 0; i <= maxAllowedUnfinishedTasks+1; i++)
            {
                _tasksDbContext.Tasks.Add(new GeneralTask
                {
                    Name = "Build a Electronic Guitar IoT device",
                    Description = "Build a Electronic Guitar device with oak wood",
                    DueDate = DateTime.Now.AddDays(15).Date,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30).Date,
                    Priority = GeneralTaskPriority.High,
                    Status = GeneralTaskStatus.New
                });    
            }
            _tasksDbContext.SaveChangesAsync();

            int recordId = _tasksDbContext.Tasks.FirstOrDefault(t =>
                string.Equals(t.Name, "Build a Electronic Guitar IoT device", StringComparison.InvariantCultureIgnoreCase) && t.DueDate == DateTime.Now.AddDays(15).Date)!.Id;
            _taskModel.Id = recordId;
            _taskModel.Name = "Build a smartphone application - Updated";
            
            //Assign
            var objectResponse = _taskService.UpdateTask(_taskModel);

            //Assert
            Assert.IsNotNull(objectResponse?.Result);
            Assert.AreEqual(false, objectResponse.Result.Flags.HasSuccess);
            Assert.AreEqual(true, objectResponse.Result.Flags.StatusCode == HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public void UpdateTask_ValidInput_Return_Success()
        {
            
            _tasksDbContext.Tasks.Add(_generalTask);
            _tasksDbContext.SaveChanges();

            int recordId = _tasksDbContext.Tasks.FirstOrDefault(t => t.Name == _generalTask.Name && t.DueDate == _generalTask.DueDate)!.Id;
            _taskModel.DueDate = DateTime.Now.AddDays(5);
            _taskModel.Id=recordId;
            
            var objectResponse = _taskService.UpdateTask(_taskModel);

            Assert.IsNotNull(objectResponse?.Result);
            Assert.AreEqual(true, objectResponse.Result.Flags.HasSuccess);
            Assert.AreEqual(true, objectResponse.Result.Flags.StatusCode == HttpStatusCode.OK);


        }
    }
}