using Assignment4.Core;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }

        //Creating a task will set its state to New and Created/StateUpdated to current time in UTC.
        //Create/update task must allow for editing tags.
        public (Response Response, int TaskId) Create(TaskCreateDTO task){
/*             if (_context.Users.Find(task.AssignedToId) == null)
                return (Response.BadRequest, 0); */

            var entity = new Task{   CreatedUTC = DateTime.UtcNow,
                                     StateUpdatedUTC = DateTime.UtcNow,
                                     Title = task.Title,
                                     AssignedToId = task.AssignedToId,
                                     Description = task.Description,
                                     State = State.New,
                                     Tags = idsToTags(task.Tags).ToList()};
            
            var addedTask = _context.Tasks.Add(entity);
             _context.SaveChanges();
            return(Response.Created, entity.Id);
        }
        public IReadOnlyCollection<TaskDTO> ReadAll(){
            throw new NotImplementedException();
        }   
        public IReadOnlyCollection<TaskDTO> ReadAllRemoved(){
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag){
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId){
            throw new NotImplementedException();
        }
        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state){
            throw new NotImplementedException();
        }
        public TaskDetailsDTO Read(int taskId){
            throw new NotImplementedException();
        }
        public Response Update(TaskUpdateDTO task){
            var entity = _context.Tasks.Find(task.Id);
            if (entity == null)
                return Response.NotFound;
            entity.State = task.State;
            entity.StateUpdatedUTC = DateTime.Now;
            entity.Tags = idsToTags(task.Tags).ToList();
            _context.SaveChanges();
            return Response.Updated;
        }
        
        public (Response response, int taskId, string taskTitle) Delete(int taskId){
            var task = _context.Tasks.Find(taskId);
            if (task == null)
                return (Response.NotFound, taskId, null);

            if (task.State == State.New){
                foreach(var tag in task.Tags){
                    _context.Tags.Find(tag).Tasks.Remove(task); 
                }
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                return (Response.Deleted, taskId, task.Title); //Kan måske give null pointer reference exception hvis task bliver slettet - idt. prøv at gemme titel som string før task bliver fjernet.
            }
            
            if (task.State == State.Active){
                var taskUpdate = new TaskUpdateDTO{Id = taskId, State = State.Removed, Tags = task.Tags.Select(x => x.Id).ToList()};
                Update(taskUpdate);
                return (Response.Deleted, taskId, task.Title);
            }
            else 
                return (Response.Conflict, taskId, task.Title);
        }
        public IEnumerable<Tag> idsToTags(List<int> ids){
            foreach(var id in ids){
                yield return _context.Tags.Find(id);
            }

        }

        
    }
}
