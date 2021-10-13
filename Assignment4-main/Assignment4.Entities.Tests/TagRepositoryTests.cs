using System;
using Assignment4.Entities;
using Assignment4.Core;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


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
        public void create_creates_tag_given_new_tag(){
            var tag = new TagCreateDTO{Name = "backend"};
        
            var createdTag = _repo.Create(tag);

            Assert.Equal((Response.Created, 1), createdTag);
            
        }

        [Fact]
        public void create_returns_conflict_when_creating_existing_tag(){
            var tagOne = new TagCreateDTO{Name = "backend"};
            var createdTagOnce = _repo.Create(tagOne);
            _context.SaveChanges();


            var tagTwo = new TagCreateDTO{Name = "backend"};
            var createdTagTwice = _repo.Create(tagTwo);

            Assert.Equal((Response.Conflict, 0), createdTagTwice);
        }

        [Fact]
        public void update_nonexisting_tag_returns_NotFound(){
            var update = new TagUpdateDTO{Id = 1000, Name = "Fun"};

            var response = _repo.Update(update);

            Assert.Equal(Response.NotFound, response);
        }

        [Fact]
        public void update_tag_updates_name(){
            var tag = _repo.Create(new TagCreateDTO{Name = "Fun"});
            
            var update = new TagUpdateDTO{Id = tag.TagId, Name = "Not Fun"};

            var response = _repo.Update(update);
            
            Assert.Equal(Response.Updated, response);
            Assert.Equal(update.Id, tag.TagId);
            Assert.Equal(update.Name, _repo.Read(update.Id).name);
        }
        
        [Fact]
        public void return_list_of_all_tags_in_database(){

            _context.Tags.AddRange(
                new Tag { Id = 1, Name = "BackEnd"},
                new Tag { Id = 2, Name = "FrontEnd"},
                new Tag { Id = 3, Name = "FullStack"}
            );

            _context.SaveChanges();


            var eTag = new List<TagDTO>(){
                new TagDTO{Id = 1, Name = "BackEnd"},
                new TagDTO{Id = 2, Name = "FrontEnd"},
                new TagDTO{Id = 3, Name = "FullStack"}
            };

            Assert.Equal(eTag,  _repo.ReadAll());

        }



        [Fact]
        public void delete_tag_get_response_deleted(){

            // Arrange
            var tag = new TagCreateDTO{Name ="Backend"};
            var createdTag = _repo.Create(tag);
            var expected = (Response.Deleted, 1, "Backend");

            // Act
            var actual = _repo.Delete(1);
            
            // Assert
            Assert.Equal(expected, actual);
        }   
        
        [Fact]
        public void delete_tag_get_response_notfound(){

            // Arrange
            var tag = new TagCreateDTO{Name ="Backend"};
            var createdTag = _repo.Create(tag);
            string str = null;
            var expected = (Response.NotFound, 2, str);

            // Act
            var actual = _repo.Delete(2);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /*[Fact] //Tasks skal først implementeres før der kan testes for conflict
        public void delete_tag_get_response_conflict(){

            // Arrange
            var tag = new TagCreateDTO{Name ="Backend"};
            var createdTag = _repo.Create(tag);
            string str = null;
            var expected = (Response.Conflict, 1, "Backend");
            // Kode som laver _repo.Create(Task) med "Backend" som tag

            // Act
            var actual = _repo.Delete(1);
            
            // Assert
            Assert.Equal(expected, actual);
        }*/

        public void Dispose(){
            _context.Dispose();
        }
    }
}
