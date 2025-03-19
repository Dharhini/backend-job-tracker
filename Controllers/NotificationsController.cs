using Microsoft.AspNetCore.Mvc; // ✅ Fix ControllerBase & API attributes
using Microsoft.EntityFrameworkCore; // ✅ Fix DatabaseContext issues
using Microsoft.AspNetCore.DataProtection;// ✅ Ensure DatabaseContext is recognized
using server.Models; // ✅ Ensure Notification model is recognized
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Data;

[Route("api/notifications")]
[ApiController]
public class NotificationsController : ControllerBase {
    private readonly DatabaseContext _context;

    public NotificationsController(DatabaseContext context) {
        _context = context;
    }

    [HttpGet("{userEmail}")]
    public async Task<ActionResult<List<Notification>>> GetUserNotifications(string userEmail) {
        return await _context.Notifications
            .Where(n => n.UserEmail == userEmail && !n.IsRead)
            .ToListAsync();
    }

    [HttpPut("mark-as-read/{id}")]
public async Task<IActionResult> MarkAsRead(int id) {
    var notification = await _context.Notifications.FindAsync(id);
    if (notification == null) return NotFound(new { message = "Notification not found" });

    notification.IsRead = true;
    await _context.SaveChangesAsync();

    return Ok(new { message = "Notification marked as read" });
}

}
