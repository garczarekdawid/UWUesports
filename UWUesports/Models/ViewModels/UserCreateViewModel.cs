﻿using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.Models.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
