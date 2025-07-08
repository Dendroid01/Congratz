public class BirthdayPersonDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }

    public string? PhotoBase64 { get; set; }
    public string? PhotoMimeType { get; set; }
}