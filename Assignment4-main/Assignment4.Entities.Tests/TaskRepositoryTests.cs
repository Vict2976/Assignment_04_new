using System;
using Assignment4.Entities;
using Assignment4.Core;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

/*
1. Only tasks with the state `New` can be deleted from the database.
1. Deleting a task which is `Active` should set its state to `Removed`.
1. Deleting a task which is `Resolved`, `Closed`, or `Removed` should return `Conflict`.
1. Creating a task will set its state to `New` and `Created`/`StateUpdated` to current time in UTC.
1. Create/update task must allow for editing tags.
1. Updating the `State` of a task will change the `StateUpdated` to current time in UTC.
1. Assigning a user which does not exist should return `BadRequest`.
1. TaskRepository may *not* depend on *TagRepository* or *UserRepository*.
*/
namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly KanbanContext _context;
        private readonly TaskRepository _task_repo;

        private readonly UserRepository _user_repo;

        public TaskRepositoryTests(){

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            context.SaveChanges();

            _context = context;
            _task_repo = new TaskRepository(_context);
            _user_repo = new UserRepository(_context);
        }

        //User not implemented yet. [Fact]
        public void Assigning_nonexisting_user_gives_BadRequest(){
            var TCDTO = new TaskCreateDTO{Title = "Task for a Ghost", AssignedToId = 10000, Description = "A task assigned to a nonexisting user", Tags = new List<int>()};
            var resp = _task_repo.Create(TCDTO);
            Assert.Equal(Response.BadRequest, resp.Response);
        }

        [Fact]
        public void Created_task_has_State_New_and_UTC_now_and_has_stateUpdated_UTC_now(){
/*             var user = _user_repo.Create(new UserCreateDTO{Name = "Søren", Email = "Søren@postdanmark.dk"});
 */            var task = new TaskCreateDTO{Title = "work", /* AssignedToId = user.UserId, */ Description = "hard", Tags = new List<int>()};
            
            var createdTask = _task_repo.Create(task);
            Task findTask = _context.Tasks.Find(createdTask.TaskId);

            Assert.Equal((Response.Created, 1), createdTask);
            Assert.Equal(DateTime.UtcNow, findTask.CreatedUTC, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(DateTime.UtcNow, findTask.StateUpdatedUTC, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(State.New, findTask.State);
            
        }

        [Fact]
        public void Creating_allows_setting_tags(){
            
        }
        [Fact]
        public void Updating_allows_setting_tags(){
            
        }
        [Fact]
        public void New_task_can_be_deleted(){
/*             var user = _user_repo.Create(new UserCreateDTO{Name = "Søren", Email = "Søren@postdanmark.dk"});
 */            var TCDTO = new TaskCreateDTO{Title = "Task for a Søren"/*, AssignedToId = user.UserId*/, Description = "A task assigned to a user Søren", Tags = new List<int>()};
            var resp = _task_repo.Create(TCDTO);
            int id = resp.TaskId;
            var delete = _task_repo.Delete(id);
            Assert.Null(_context.Tasks.Find(id));
        }

        [Fact]
        public void Delete_Active_Task_makes_it_Removed(){
/*             var user = _user_repo.Create(new UserCreateDTO{Name = "Søren", Email = "Søren@postdanmark.dk"});
 */            var TCDTO = new TaskCreateDTO{Title = "Task for a Søren", Description = "A task assigned to a user Søren", Tags = new List<int>()};
            var resp = _task_repo.Create(TCDTO);
            int id = resp.TaskId;
            var update = _task_repo.Update(new TaskUpdateDTO{Id = id, State = State.Active, Title = "Task for a Søren", /* AssignedToId = user.UserId, */ Description = "A task assigned to a user Søren", Tags = new List<int>()});
            var delete = _task_repo.Delete(id);
            Assert.Equal(State.Removed, _context.Tasks.Find(id).State);
        }

        [Fact]
        public void Deleting_Resolved_gives_Confict(){
/*             var user = _user_repo.Create(new UserCreateDTO{Name = "Søren", Email = "Søren@postdanmark.dk"});
 */            var TCDTO = new TaskCreateDTO{Title = "Task for a Søren"/*, AssignedToId = user.UserId*/, Description = "A task assigned to a user Søren", Tags = new List<int>()};
            var resp = _task_repo.Create(TCDTO);
            int id = resp.TaskId;
            var update = _task_repo.Update(new TaskUpdateDTO{Id = id, State = State.Resolved, /* AssignedToId = user.UserId, */ Description = "A task assigned to a user Søren", Tags = new List<int>()});
            var delete = _task_repo.Delete(id);
            Assert.Equal(Response.Conflict, delete.response);
        }
        
        [Fact]
        public void Deleting_Closed_gives_Confict(){
            //var user = _user_repo.Create(new UserCreateDTO{Name = "Søren", Email = "Søren@postdanmark.dk"});
            var TCDTO = new TaskCreateDTO{Title = "Task for a Søren"/*, AssignedToId = user.UserId*/, Description = "A task assigned to a user Søren", Tags = new List<int>()};
            var resp = _task_repo.Create(TCDTO);
            int id = resp.TaskId;
            var update = _task_repo.Update(new TaskUpdateDTO{Id = id, State = State.Closed, Title = "Task for a Søren", /* AssignedToId = user.UserId, */ Description = "A task assigned to a user Søren", Tags = new List<int>()});
            var delete = _task_repo.Delete(id);
            Assert.Equal(Response.Conflict, delete.response);
        }

        [Fact]
        public void Deleting_Removed_gives_Confict(){
/*             var user = _user_repo.Create(new UserCreateDTO{Name = "Søren", Email = "Søren@postdanmark.dk"});
 */            var TCDTO = new TaskCreateDTO{Title = "Task for a Søren"/*, AssignedToId = user.UserId*/, Description = "A task assigned to a user Søren", Tags = new List<int>()};
            var resp = _task_repo.Create(TCDTO);
            int id = resp.TaskId;
            var update = _task_repo.Update(new TaskUpdateDTO{Id = id, State = State.Removed, Title = "Task for a Søren", /* AssignedToId = user.UserId, */ Description = "A task assigned to a user Søren", Tags = new List<int>()});
            var delete = _task_repo.Delete(id);
            Assert.Equal(Response.Conflict, delete.response);
        }

        public void Dispose(){
            _context.Dispose();
        }
    }

}
