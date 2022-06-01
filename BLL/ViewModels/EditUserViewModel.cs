using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BLL.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Range(1900, 2021, ErrorMessage = "Введено некоректний рік")]
        public int Year { get; set; }
    }
}
