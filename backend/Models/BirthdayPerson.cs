using System;
using System.ComponentModel.DataAnnotations;

namespace Congratz.backend.Models
{
    public class BirthdayPerson
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Полное имя' обязательно для заполнения.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 100 символов.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Дата рождения обязательна.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(BirthdayPerson), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        public byte[]? Photo { get; set; }

        public string? PhotoMimeType { get; set; }

        public static ValidationResult? ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth > DateTime.Today)
            {
                return new ValidationResult("Дата рождения не может быть в будущем.");
            }
            return ValidationResult.Success;
        }
    }
}
