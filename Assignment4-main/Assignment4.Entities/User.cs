using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Assignment4.Entities
{
    public class User
    {
        public int id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
