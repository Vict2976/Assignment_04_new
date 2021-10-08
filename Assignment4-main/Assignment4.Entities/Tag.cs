using System.Collections.Generic;
using Assignment4.Core;

using System.ComponentModel.DataAnnotations;

namespace Assignment4.Entities

{
    public class Tag
    {
        public int Id { get; init; }

        [MaxLength(50)]
        [Required]
        public string Name { get; init; }
        public List<Task> Tasks { get; init; }
    }
}
