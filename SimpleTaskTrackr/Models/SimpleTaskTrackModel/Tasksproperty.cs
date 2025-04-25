using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SimpleTaskTrackr.Models.SimpleTaskTrackModel
{
    public class TasksProperties
    {
        [Key]
        public Guid TaskId { get; set; }

        [Required]
        [StringLength(200)]
        public string TaskTitle { get; set; }
        
        [StringLength(1000)]
        public string TaskDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime TaskDueDate { get; set; }

        [Required]
        public string TaskStatus { get; set; }  // e.g., Pending, Completed , In Progress

        public string TaskRemarks { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }
        // 🔗 Foreign Key
        [Required]
        public Guid UserId { get; set; }

        // 🌐 Navigation Property
        [ForeignKey("UserId")]
        public Userproperty User { get; set; }

    }
}
