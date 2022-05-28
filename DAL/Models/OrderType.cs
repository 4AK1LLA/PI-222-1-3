using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class OrderType
    {
        public int OrderTypeId { get; set; }
        [Display(Name = "Тип замовлення")]
        public string OrderName { get; set; }
    }
}
