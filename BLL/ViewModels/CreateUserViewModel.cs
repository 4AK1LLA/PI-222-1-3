using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BLL.ViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Range(1900, 2021, ErrorMessage = "Введено некоректний рік")]
        public int Year { get; set; }
    }
}
