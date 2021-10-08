using System;
using Assignment4.Entities;
using Assignment4.Core;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTest : IDisposable
    {
        private readonly KanbanContext _context;
        private readonly TagRepository _repo;

        public TagRepositoryTest(){

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            context.SaveChanges();

            _context = context;
            _repo = new TagRepository(_context);
        }

        [Fact]
        public void test_create_creates_tag_given_new_tag(){
            var tag = new TagCreateDTO{Name = "backend"};
        
            var created = _repo.Create(tag);

            Assert.Equal((Response.Created, 1), created);
            
        }

        public void Dispose(){
            _context.Dispose();
        }


    }
}
