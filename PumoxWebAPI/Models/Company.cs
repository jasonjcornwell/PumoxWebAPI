using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
