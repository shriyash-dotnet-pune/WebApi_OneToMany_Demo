using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_OneToMany_Demo.Data;
using WebApi_OneToMany_Demo.Models;

namespace WebApi_OneToMany_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EmployeesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Employees.Include(e => e.Department).ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _db.Employees.Include(e => e.Department)
                                         .FirstOrDefaultAsync(e => e.Id == id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            // optional: verify department exists
            var deptExists = await _db.Departments.AnyAsync(d => d.Id == emp.DepartmentId);
            if (!deptExists) return BadRequest($"DepartmentId {emp.DepartmentId} does not exist.");

            _db.Employees.Add(emp);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = emp.Id }, emp);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Employee emp)
        {
            if (id != emp.Id) return BadRequest();
            _db.Entry(emp).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();
            _db.Employees.Remove(emp);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
