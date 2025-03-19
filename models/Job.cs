public class Job {
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty; // ✅ Fix null warning
    public string Position { get; set; } = string.Empty; // ✅ Fix null warning
    public string Status { get; set; } = "Pending"; // ✅ Default value
    public string UserEmail { get; set; } = string.Empty; // ✅ Fix null warning
}