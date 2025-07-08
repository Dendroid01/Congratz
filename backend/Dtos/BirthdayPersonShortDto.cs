namespace Congratz.backend.Dtos
{
    public class BirthdayPersonShortDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}