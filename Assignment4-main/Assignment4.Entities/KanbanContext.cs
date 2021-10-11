using System.Collections.Generic;
using System;
using Assignment4.Entities;
using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Assignment4.Entities

{
    public class KanbanContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }

    
/*         protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql($"Host=localhost;Database=lecture4;Username=postgres;Password=cxr48ges"); */

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder
                .Entity<Task>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (State)Enum.Parse(typeof(State), v));
                
            modelBuilder
                .Entity<Tag>()
                .HasIndex(c => c.Name)
                .IsUnique();
            


        }
    }
}


