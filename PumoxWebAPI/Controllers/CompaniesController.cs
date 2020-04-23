using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PumoxWebAPI.Models;

namespace PumoxWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly CompanyContext _context;

        public CompaniesController(ILogger<CompaniesController> logger, CompanyContext context)
        {
            _logger = logger;
            _context = context;
        }

        

[Route("InitDB")]
        [HttpPost]
        public async Task<IActionResult> InitDB()
        {
            var company = new Company
            {
                Name = "Something",
                EstablishmentYear = 1993,
                Employees = new List<Employee>
                {
                    new Employee { FirstName = "Jeff", LastName = "Dood", DateOfBirth = DateTime.Parse("2019-07-26T00:00:00"), JobTitle = JobTitle.Developer }
                }
            };

            _context.Companies.Add(company);
            _context.SaveChanges();


            var c = _context.Companies.First();
            if (!c.Employees.Any())
            {
                c.Employees.Add(new Employee { 
                    FirstName = "Bob", LastName = "Dood", DateOfBirth = DateTime.Parse("2019-07-26T00:00:00"), JobTitle = JobTitle.Developer 
                });
                _context.SaveChanges();
            }

            return Ok();
        }

// GET: api/Employees
[Route("GetEmployees")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return null;
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(long id)
        {
            var c = _context.Companies.First();
            if (!c.Employees.Any())
            {
                var e = new Employee
                {
                    FirstName = "Bob",
                    LastName = "Dood",
                    DateOfBirth = DateTime.Parse("2019-07-26T00:00:00"),
                    JobTitle = JobTitle.Developer,
                    //Company = c,
                };

                c.Employees.Add(e);
                _context.SaveChanges();
            }

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(long id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<long>> PostCompany(Company company)
        {
            _context.Companies.Add(company);
            //foreach(Employee employee in company.Employees)
            //{
            //    _context.Employees.Add(employee);
            //}
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        private bool CompanyExists(long id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
