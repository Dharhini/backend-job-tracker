namespace server.Models {
    public class User {
        public int Id { get; set; }
        public required string Email { get; set; }  // Ensures Email is required
        public required string Password { get; set; }  // Ensures Password is required
    }
}
