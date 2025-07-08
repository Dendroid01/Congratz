using Microsoft.AspNetCore.Http;

public class BirthdayPersonCreateDto
{
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public IFormFile? Photo { get; set; }
}