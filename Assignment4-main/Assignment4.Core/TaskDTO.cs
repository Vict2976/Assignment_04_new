using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;



namespace Assignment4.Core
{
    public record TaskDTO
    {
        public int Id {get; init;}
        public string Title {get; init;}
        public string AssignedToName {get; set;}
        public ICollection<string> Tags {get; set;}
        public State State {get; set;}
    } 

    public record TaskDetailsDTO
    {
        public int Id {get; init;}
        public string Title {get; init;}
        public string Description {get; init;}
        public DateTime Created { get; set; }
        public string AssignedToName { get; set; }
        public ICollection<string> Tags { get; set; }
        public State state { get; set; }

        public DateTime StateUpdated { get; set; }

        
    }
    public record TaskCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; init; }

        public int? AssignedToId { get; init; }

        public string Description { get; init; }

        public ICollection<string> Tags { get; init; }
    }

    public record TaskUpdateDTO : TaskCreateDTO
    {
        public int Id { get; init; }

        public State State { get; init; }
    }
}