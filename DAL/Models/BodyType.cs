using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class BodyType
    {
        public BodyType()
        {
            Cars = new HashSet<Car>();
        }

        public int BodyTypeId { get; set; }
        [Display(Name = "Тип кузова")]
        public string BodyTypeNames { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
