using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Services;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(ApplicationDbContext context, IDepartmentRepository departmentRepository)
        {
            _context = context;
            _departmentRepository = departmentRepository;
        }

        // GET: api/Departments
        [HttpGet("All-Departments")]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("Single-Department/{id}")]
        public async Task<ActionResult<Department>> GetSingleDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update-Department")]
        public async Task<IActionResult> UpdateSingleDepartment(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add-Department")]
        public async Task<ActionResult<Department>> AddNewDepartment(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("Delete-Department/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(department); // Ensure it returns a JSON response, like the deleted department
        }


        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        [HttpGet("ExportDepartmentsToCsv")]
        public async Task<IActionResult> ExportDepartmentsToCsv()
        {
            var csvBytes = await _departmentRepository.ExportToCsvAsync();
            return File(csvBytes, "text/csv", "Departments.csv");
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Department>>> SearchDepartmentsAsync(
            [FromQuery] string name, [FromQuery] string code)
        {
            var query = _context.Departments.AsQueryable();

            // Apply filters

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => (s.Name).Trim().Contains(name));

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(s => (s.Code).Trim().Contains(code));

            var departments = await query.ToListAsync();
            return Ok(departments);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginationModel<Department>>> GetPagedDepartmentsAsync(
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string? id, [FromQuery] string? name,
            [FromQuery] string? code, [FromQuery] string? createBy,
            [FromQuery] DateTime createOn,
            [FromQuery] string? sortField,
            [FromQuery] bool sortAscending = true)
        {
            var searchParameters = new SearchParameters
            {
                Name = name,
                Code = code,
                CreateBy = createBy,
                CreateOn = createOn,
                SortField = sortField,
                SortAscending = sortAscending
            };

            var result = await _departmentRepository.GetPagedDepartmentsAsync(pageNumber, pageSize, searchParameters);
            return Ok(result);
        }
    }
}
