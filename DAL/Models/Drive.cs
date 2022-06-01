using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class Drive
    {
        public Drive()
        {
            Cars = new HashSet<Car>();
        }

        public int DriveId { get; set; }
        [Display(Name = "Тип приводу")]
        public string DriveType { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
