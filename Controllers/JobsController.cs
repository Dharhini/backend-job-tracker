using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using server.Data;

[Route("api/jobs")]
[ApiController]
public class JobsController : ControllerBase {
    private readonly DatabaseContext _context;

    public JobsController(DatabaseContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Job>>> GetJobs() {
        return await _context.Jobs.ToListAsync();
    }

   [HttpPost]
public async Task<IActionResult> AddJob([FromBody] Job job) {
    Console.WriteLine($"Received job request: Company={job.Company}, Position={job.Position}, Status={job.Status}, UserEmail={job.UserEmail}");

    if (job == null) {
        Console.WriteLine("❌ Error: No job data received.");
        return BadRequest(new { message = "Invalid job data." });
    }

    if (string.IsNullOrEmpty(job.Company) || string.IsNullOrEmpty(job.Position) || string.IsNullOrEmpty(job.UserEmail)) {
        Console.WriteLine("❌ Error: Missing required fields.");
        return BadRequest(new { message = "Company, Position, and UserEmail are required." });
    }

    try {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        Console.WriteLine("✅ Job added successfully!");
        return Ok(new { message = "Job added successfully!" });
    } catch (Exception ex) {
        Console.WriteLine($"❌ Database error: {ex.Message}");
        return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
    }
}







[HttpGet("user-jobs")]
public async Task<ActionResult<List<Job>>> GetUserJobs([FromQuery] string userEmail) {
    if (string.IsNullOrEmpty(userEmail)) 
        return BadRequest(new { message = "User email is required." });

    var userJobs = await _context.Jobs
        .Where(j => j.UserEmail == userEmail) // ✅ Filter jobs by user
        .ToListAsync();

    if (userJobs.Count == 0) 
        return NotFound(new { message = "No jobs found for this user." });

    return Ok(userJobs);
}





    [HttpPut("{id}")]
public async Task<IActionResult> UpdateJob(int id, [FromBody] Job updatedJob) {
    var existingJob = await _context.Jobs.FindAsync(id);
    if (existingJob == null) return NotFound("Job not found.");

    existingJob.Company = updatedJob.Company;
    existingJob.Position = updatedJob.Position;

    // Check if status is changed to "Interview Scheduled"
    if (existingJob.Status != "Interview Scheduled" && updatedJob.Status == "Interview Scheduled") {
        var notification = new Notification {
            UserEmail = existingJob.UserEmail,
            Message = $"Your interview has been scheduled with {existingJob.Company}.",
            IsRead = false
        };
        _context.Notifications.Add(notification);
    }

    existingJob.Status = updatedJob.Status;

    await _context.SaveChangesAsync();
    return Ok(new { message = "Job updated successfully!" });
}



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id) {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return NotFound("Job not found.");

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Job deleted successfully!" });
    }
}
