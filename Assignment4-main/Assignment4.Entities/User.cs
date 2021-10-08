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


//City, CityRepo, iCityRepo


/*public cityDTO read (int cityID)
    var citeis= from c in _context.cities
                where c.id==cityid
                select new cityDTO(c.id, c.name)
    
    return cities.firstOrDefault();
            

    */