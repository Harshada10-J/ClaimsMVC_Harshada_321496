namespace ClaimsMVC.Dtos
{
    public class RegisterDto
    {
        public required string EmployeeNo { get; set; }
        public required string FullName { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
