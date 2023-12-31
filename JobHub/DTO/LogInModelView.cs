﻿using System.ComponentModel.DataAnnotations;

namespace JobHub.DTO
{
    public class LogInModelView
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
