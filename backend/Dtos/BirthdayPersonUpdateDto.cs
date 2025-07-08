using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Congratz.backend.Dtos
{
    public class BirthdayPersonUpdateDto
    {
        [Required(ErrorMessage = "Полное имя обязательно")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Полное имя должно содержать от 2 до 100 символов")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(BirthdayPersonUpdateDto), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        public IFormFile? Photo { get; set; }

        public static ValidationResult? ValidateDateOfBirth(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Today)
                return new ValidationResult("Дата рождения не может быть в будущем.");
            return ValidationResult.Success;
        }
    }
}