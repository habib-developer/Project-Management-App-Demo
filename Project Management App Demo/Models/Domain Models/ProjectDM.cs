using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Management_App_Demo.Models.Domain_Models
{
    [Table("Project")]
    public class ProjectDM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name="Project")]
        public int ProjectID { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProjectUserDM> ProjectUsers { get; set; }
        //Trackable properties
        [Display(Name="Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
    }
}
