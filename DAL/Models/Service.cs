using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class Service
    {
        public int ServiceId { get; set; }
        [Display(Name = "Тип сервісу")]
        public string TypeOfService { get; set; }
    }
}
