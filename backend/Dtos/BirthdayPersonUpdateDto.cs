using Microsoft.AspNetCore.Http;

public class BirthdayPersonUpdateDto
{
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public IFormFile? Photo { get; set; }
}