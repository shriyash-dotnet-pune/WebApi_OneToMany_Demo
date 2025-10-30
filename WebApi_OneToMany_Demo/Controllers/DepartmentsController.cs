using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_OneToMany_Demo.Data;
using WebApi_OneToMany_Demo.Models;

namespace WebApi_OneToMany_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DepartmentsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Departments.Include(d => d.Employees).ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var dept = await _db.Departments.Include(d => d.Employees)
                                           .FirstOrDefaultAsync(d => d.Id == id);
            if (dept == null) return NotFound();
            return Ok(dept);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department dept)
        {
            _db.Departments.Add(dept);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dept.Id }, dept);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Department dept)
        {
            if (id != dept.Id) return BadRequest();
            _db.Entry(dept).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _db.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            _db.Departments.Remove(dept);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
