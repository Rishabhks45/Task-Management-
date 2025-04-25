using System.ComponentModel.DataAnnotations;

namespace SimpleTaskTrackr.Models.SimpleTaskTrackModel
{
    public class Userproperty
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]        
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        

    }
}
