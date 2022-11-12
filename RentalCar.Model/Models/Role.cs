using System.ComponentModel.DataAnnotations;

namespace RentalCar.Model.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(256)]
        public string Name { get; set; }

        public List<RoleUser> RoleUsers {get; set; }

        public Role(){}

        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}