namespace API.DTOs
{
    public record UserDto
    {
        public string Email { get; set; }
        public string DesplayName { get; set; }
        public string Token { get; set; }
    }
}
