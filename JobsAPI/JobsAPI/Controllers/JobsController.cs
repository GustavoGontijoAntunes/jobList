using JobsAPI.Entities;
using JobsAPI.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace JobsAPI.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly JobsDbContext _context;
        public JobsController(JobsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobs = _context.Jobs.ToList();

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var job = _context.Jobs.SingleOrDefault(j => j.Id == id);

            if(job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpPost]
        public IActionResult Post(Job job)
        {
            _context.Jobs.Add(job);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
        }

        [HttpPut("{id}")] // Good practice
        public IActionResult Put(int id, Job input)
        {
            var job = _context.Jobs
                // .AsNoTracking() // Function used to not track the object
                .SingleOrDefault(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            job.Update(input.Title, input.Description, input.Company, input.Location, input.Salary);
            
            _context.Jobs.Update(job);
            // _context.Entry(job).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")] // Good practice
        public IActionResult Delete(int id)
        {
            var job = _context.Jobs
                .SingleOrDefault(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }
            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return NoContent();
        }
    }
}