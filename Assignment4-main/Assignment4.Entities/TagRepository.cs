using Assignment4.Core;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Assignment4.Entities
{
    public class TagRepository
    {
        
        private readonly KanbanContext _context;

        public TagRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TagId) Create(TagCreateDTO tag){
            var entity = new Tag{Name = tag.Name};
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
                .Select(c => new TagDTO(c.Id, c.Name))
                .ToList()  
                .AsReadOnly();
        
    
        public Tag Read(int tagId) => _context.Tags.Find(tagId);
        
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

            if (tag.Tasks == null || force || tag.Tasks.Any() == true) //This logic-order is important, as .Any() cannot be called on null
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
