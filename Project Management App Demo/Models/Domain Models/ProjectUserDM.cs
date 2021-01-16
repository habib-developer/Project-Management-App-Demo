using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_App_Demo.Models.Domain_Models
{
    [Table("ProjectUserMapping")]
    public class ProjectUserDM
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectUserID { get; set; }
        [ForeignKey("Project")]
        public int ProjectID { get; set; }
        public ProjectDM Project { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public UserDM User { get; set; }
        //Trackable properties
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
