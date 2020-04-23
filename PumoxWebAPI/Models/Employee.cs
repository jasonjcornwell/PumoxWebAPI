using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxWebAPI.Models
{

    public enum JobTitle
    {
        Administrator, Developer, Architect, Manager
    }

    public class Employee
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public JobTitle JobTitle { get; set; }

        //[ForeignKey("CompanyId")]
        //public Company Company { get; set; }
    }
}
