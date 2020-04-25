using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PumoxWebAPI.Models;

namespace PumoxWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CompanyController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(long id)
        {
            var company = await _context.Companies.Include(c => c.Employees).FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.Include(c=>c.Employees).ToListAsync();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<long>> PostCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Created(nameof(GetCompany), new { Id = company.Id });
        }

        [AllowAnonymous]
        [Route("Search")]
        [HttpPost]
        public async Task<ActionResult<object>> SearchCompanies([FromBody] CompanySearchModel search)
        {
            IQueryable<Company> companies = null;
            if(search.keyword != null)
            {
                companies = _context.Companies.Include(c => c.Employees).Where(c => c.Name.Contains(search.keyword)
                || c.Employees.Any(e => e.FirstName.Contains(search.keyword))
                || c.Employees.Any(e => e.LastName.Contains(search.keyword))
                );
            }
            if(companies.Count() > 0 && search.EmployeeDateOfBirthFrom != null)
            {
                companies = companies?.Where(c => c.Employees.Any(e => e.DateOfBirth > search.EmployeeDateOfBirthFrom));
            }
            if (companies.Count() > 0 && search.EmployeeDateOfBirthTo != null)
            {
                companies = companies?.Where(c => c.Employees.Any(e => e.DateOfBirth < search.EmployeeDateOfBirthTo));
            }
            if (companies.Count() > 0 && search.EmployeeJobTitles != null && search.EmployeeJobTitles.Count > 0)
            {
                companies = companies?.Where(c => c.Employees.Any(e => search.EmployeeJobTitles.Contains(e.JobTitle.Value)));
            }

            if (companies.Count() == 0)
            {
                return new { Results = new int[0] };
            }

            return new { Results = companies.ToList() };
        }

        [Route("Update/{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(long id, Company company)
        {
            var dbCompany = await _context.Companies.Include(c => c.Employees).FirstOrDefaultAsync(c => c.Id == id);
            if (dbCompany == null)
            {
                return NotFound();
            }

            dbCompany.Name = company.Name;
            dbCompany.EstablishmentYear = company.EstablishmentYear;
            dbCompany.Employees = company.Employees;

            _context.Entry(dbCompany).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Route("Delete/{id}")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(long id)
        {
            var company = await _context.Companies.Include(c => c.Employees).FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            foreach (Employee employee in company.Employees)
            {
                _context.Employees.Remove(employee);
            }
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public class CompanySearchModel
        {
            public string keyword { set; get; }
            public DateTime? EmployeeDateOfBirthFrom { set; get; }
            public DateTime? EmployeeDateOfBirthTo { set; get; }
            public List<JobTitle> EmployeeJobTitles { set; get; }
        }
    }
}
