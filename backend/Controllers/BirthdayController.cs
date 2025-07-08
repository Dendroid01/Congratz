using Microsoft.AspNetCore.Mvc;
using Congratz.backend.Context;
using Congratz.backend.Models;
using Congratz.backend.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Congratz.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BirthdayPeopleController : ControllerBase
    {
        private readonly BirthdayContext _context;
        private readonly IMapper _mapper;

        public BirthdayPeopleController(BirthdayContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BirthdayPersonShortDto>>> GetAll()
        {
            var people = await _context.Persons.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BirthdayPersonShortDto>>(people));
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<BirthdayPersonShortDto>>> GetToday()
        {
            var today = DateTime.Today;
            var people = await _context.Persons
                .Where(p => p.DateOfBirth.Day == today.Day && p.DateOfBirth.Month == today.Month)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<BirthdayPersonShortDto>>(people));
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<BirthdayPersonShortDto>>> GetUpcoming()
        {
            var today = DateTime.Today;
            var todayMonth = today.Month;
            var todayDay = today.Day;

            var inSevenDays = today.AddDays(7);
            var inSevenDaysMonth = inSevenDays.Month;
            var inSevenDaysDay = inSevenDays.Day;

            IQueryable<BirthdayPerson> peopleQuery = _context.Persons;

            if (todayMonth < inSevenDaysMonth || (todayMonth == inSevenDaysMonth && todayDay <= inSevenDaysDay))
            {
                peopleQuery = peopleQuery.Where(p =>
                    (p.DateOfBirth.Month > todayMonth || (p.DateOfBirth.Month == todayMonth && p.DateOfBirth.Day >= todayDay)) &&
                    (p.DateOfBirth.Month < inSevenDaysMonth || (p.DateOfBirth.Month == inSevenDaysMonth && p.DateOfBirth.Day <= inSevenDaysDay))
                );
            }
            else
            {
                peopleQuery = peopleQuery.Where(p =>
                    (p.DateOfBirth.Month > todayMonth || (p.DateOfBirth.Month == todayMonth && p.DateOfBirth.Day >= todayDay)) ||
                    (p.DateOfBirth.Month < inSevenDaysMonth || (p.DateOfBirth.Month == inSevenDaysMonth && p.DateOfBirth.Day <= inSevenDaysDay))
                );
            }

            var people = await peopleQuery.ToListAsync();

            var upcoming = people
                .Select(p =>
                {
                    int nextYear = today.Year;
                    var nextBirthday = new DateTime(nextYear, p.DateOfBirth.Month, p.DateOfBirth.Day);
                    if (nextBirthday < today)
                        nextYear++;
                    nextBirthday = new DateTime(nextYear, p.DateOfBirth.Month, p.DateOfBirth.Day);

                    var daysUntil = (nextBirthday - today).TotalDays;
                    return new { Person = p, DaysUntil = daysUntil };
                })
                .Where(x => x.DaysUntil > 0 && x.DaysUntil <= 7)
                .OrderBy(x => x.DaysUntil)
                .Select(x => x.Person)
                .ToList();

            return Ok(_mapper.Map<IEnumerable<BirthdayPersonShortDto>>(upcoming));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BirthdayPersonDto>> GetById(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();

            return Ok(_mapper.Map<BirthdayPersonDto>(person));
        }

        [HttpPost]
        public async Task<ActionResult<BirthdayPersonDto>> Create([FromForm] BirthdayPersonCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var person = _mapper.Map<BirthdayPerson>(dto);

            if (dto.Photo != null)
            {
                if (!IsValidPhoto(dto.Photo))
                    return BadRequest("Invalid photo file. Allowed types: JPEG, PNG. Max size: 2 MB.");

                using var ms = new MemoryStream();
                await dto.Photo.CopyToAsync(ms);
                person.Photo = ms.ToArray();
                person.PhotoMimeType = dto.Photo.ContentType;
            }

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BirthdayPersonDto>(person);
            return CreatedAtAction(nameof(GetById), new { id = person.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] BirthdayPersonUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();

            _mapper.Map(dto, person);

            if (dto.Photo != null)
            {
                if (!IsValidPhoto(dto.Photo))
                    return BadRequest("Invalid photo file. Allowed types: JPEG, PNG. Max size: 2 MB.");

                using var ms = new MemoryStream();
                await dto.Photo.CopyToAsync(ms);
                person.Photo = ms.ToArray();
                person.PhotoMimeType = dto.Photo.ContentType;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IsValidPhoto(IFormFile photo)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            const long maxFileSize = 2 * 1024 * 1024;

            return photo != null &&
                   allowedTypes.Contains(photo.ContentType) &&
                   photo.Length > 0 &&
                   photo.Length <= maxFileSize;
        }
    }
}