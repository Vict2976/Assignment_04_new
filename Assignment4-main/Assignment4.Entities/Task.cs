using System.Collections.Generic;
using Assignment4.Core;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Assignment4.Entities
{
    public class Task
    {
        public int Id { get; init; }
        [MaxLength(100)]
        [Required]
        public string Title { get; init; }
        public int? AssignedToId { get; init; }
        public string Description { get; init; }
        public State State { get; set; }
        public List<Tag> Tags { get; set; }

        public DateTime CreatedUTC { get; set; }

        public DateTime StateUpdatedUTC { get; set; }

    }
}
