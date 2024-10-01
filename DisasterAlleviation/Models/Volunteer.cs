using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Models
{
    public class Volunteer
    {
        public int Id { get; set; } // Primary Key
        public string UserId { get; set; } // Foreign Key to associate with IdentityUser
       
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Contact Info is required")]
        public string ContactInfo { get; set; }

        public string Skills { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        public DateTime Availability { get; set; }
      //  public DateTime Date { get; set; }
    }



}
