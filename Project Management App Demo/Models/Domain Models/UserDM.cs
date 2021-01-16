using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_App_Demo.Models.Domain_Models
{
    [Table("User")]
    public class UserDM
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name="User")]
        public int UserID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Display(Name="Hours Per Week")]
        public int HoursPerWeek { get; set; }
        [Display(Name="Rate Per Hour")]
        public float RatePerHour { get; set; }
        public ICollection<ProjectUserDM> ProjectUsers { get; set; }
        //trackable properites
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
    }
}
