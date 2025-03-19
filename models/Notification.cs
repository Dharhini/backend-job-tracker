public class Notification {
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty; // ✅ Fix null warning
    public string Message { get; set; } = string.Empty; // ✅ Fix null warning
    public bool IsRead { get; set; } = false; // ✅ Default value
}