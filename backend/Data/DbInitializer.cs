using Bogus;
using Congratz.backend.Models;
using Congratz.backend.Context;

namespace Congratz.backend.Seeder
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(BirthdayContext context)
        {
            context.Database.EnsureCreated();

            if (context.Persons.Any()) return;

            var faker = new Faker<BirthdayPerson>("ru")
                .RuleFor(p => p.FullName, f => f.Name.FullName())
                .RuleFor(p => p.DateOfBirth, f => f.Date.Past(50, DateTime.Today.AddYears(-18)));

            var httpClient = new HttpClient();
            var people = new List<BirthdayPerson>();

            for (int i = 0; i < 10; i++)
            {
                var person = faker.Generate();
                person.DateOfBirth = person.DateOfBirth;

                try
                {
                    var photoBytes = await httpClient.GetByteArrayAsync($"https://i.pravatar.cc/150?img={i + 1}");
                    person.Photo = photoBytes;
                    person.PhotoMimeType = "image/jpeg";
                }
                catch
                {
                    person.Photo = null;
                    person.PhotoMimeType = null;
                }

                people.Add(person);
            }

            context.Persons.AddRange(people);
            await context.SaveChangesAsync();
        }
    }
}
