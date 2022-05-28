using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class Manager
    {
        public int ManagerId { get; set; }
        [Display(Name = "ПІБ")]
        public string FullName { get; set; }
        [Display(Name = "Телефон")]
        public string Telephone { get; set; }
    }
}
