using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PumoxWebAPI.Models
{
    public class Company
    {
        public Company()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 2050)]
        public int EstablishmentYear { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
