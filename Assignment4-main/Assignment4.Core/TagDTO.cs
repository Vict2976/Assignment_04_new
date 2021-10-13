using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Assignment4.Core
{

    public record TagDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
    };

    public record TagCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; init; }
    }

    public record TagUpdateDTO : TagCreateDTO
    {
        public int Id { get; init; }
    }

    public record TagDetailsDTO{

        
        public int id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public List<TaskDTO> Tasks { get; init; }

    }
}