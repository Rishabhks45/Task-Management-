using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTaskTrackr.Models.SimpleTaskTrackModel
{
    [Table("TasksUpdatedBy")]
    public class TasksUpdatedBy
    {
        [Key]
        public Guid TaskUpdateId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string TaskId { get; set; }
        [Required]
        public string? UpdatedBy { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }

    }
        
   
}
