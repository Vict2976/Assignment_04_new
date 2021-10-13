using Assignment4.Core;
using System.Collections.Generic;
using System;
using System.Linq;
using Assignment4;

namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        
        private readonly KanbanContext _context;

        public TagRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TagId) Create(TagCreateDTO tag){
            var tasks = new List<Task>();
            var entity = new Tag{Name = tag.Name, Tasks = tasks};
            try{
                var addedTag = _context.Tags.Add(entity);
                _context.SaveChanges();
                 return (Response.Created, entity.Id);
            }catch (Exception ex){
                return (Response.Conflict, entity.Id);
            }
        }

        public IReadOnlyCollection<TagDTO> ReadAll() =>
            _context.Tags
                .Select(c => new TagDTO{Id = c.Id, Name = c.Name})
                .ToList()  
                .AsReadOnly();
        
    
        public TagDetailsDTO Read(int tagId) {
            Tag tag = _context.Tags.Find(tagId);
            var taskDTOs = new List<TaskDTO>();
            foreach(var t in tag.Tasks){
                taskDTOs.Add(new TaskDTO{Id = t.Id, Title = t.Title, AssignedToId = t.AssignedToId, Tags = t.Tags.Select(x => x.Id).ToList(), State = t.State});
            }
            return new TagDetailsDTO{id = tag.Id, name = tag.Name, Tasks = taskDTOs};
        }
        
        public Response Update(TagUpdateDTO tag){
            Tag entity = _context.Tags.Find(tag.Id);
            if (entity == null)
                return Response.NotFound;
            entity.Name = tag.Name;
            _context.SaveChanges();
            return Response.Updated;
            
        }
        public (Response response, int tagId , string tagName) Delete(int tagId, bool force = false)
        {
            var tag = _context.Tags.Find(tagId);
            if (tag == null)
                return (Response.NotFound, tagId, null);

            if (force || tag.Tasks.Any() == false) 
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
                return (Response.Deleted, tagId, tag.Name);
            }
            else 
                return (Response.Conflict, tagId, tag.Name);
        }

    }
}
