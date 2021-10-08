using Assignment4.Core;
using System.Collections.Generic;
using System;
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
            var entity = new Tag{Name = "backend"};
            var addedTag = _context.Tags.Add(entity);
            _context.SaveChanges();
            return (Response.Created, entity.Id);
        }
        public IReadOnlyCollection<TagDTO> ReadAll(){
            throw new NotImplementedException();
        }
        public TagDTO Read(int tagId){
            throw new NotImplementedException();
        }
        public Response Update(TagUpdateDTO tag){
            throw new NotImplementedException();
            
        }
        public Response Delete(int tagId, bool force = false)
        {
            throw new NotImplementedException();
        }
    }
}
