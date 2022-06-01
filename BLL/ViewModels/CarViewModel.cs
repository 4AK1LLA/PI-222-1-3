using Microsoft.AspNetCore.Http;
using DAL.Models;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BLL.ViewModels
{
    public class CarViewModel
    {
        public int CarId { get; set; }
        [Display(Name = "Модель")]
        public int? ModelId { get; set; }
        [Display(Name = "Ціна")]
        [Range(0, 500000, ErrorMessage = "Введено некоректну ціну")]
        public decimal Price { get; set; }
        [Display(Name = "Рік випуску")]
        public int GraduationYear { get; set; }
        [Display(Name = "Тип кузова")]
        public int? BodyTypeId { get; set; }
        [Display(Name = "Назва кольору")]
        public int? ColorId { get; set; }
        [Display(Name = "Тип приводу")]
        public int? DriveId { get; set; }
        public IFormFile Image{ get; set; }
        public string Description { get; set; }
        [Display(Name = "Тип кузова")]
        public virtual BodyType BodyType { get; set; }
        [Display(Name = "Назва кольору")]
        public virtual Color Color { get; set; }
        [Display(Name = "Тип приводу")]
        public virtual Drive Drive { get; set; }
        [Display(Name = "Модель")]
        public virtual Model Model { get; set; }
    }
}
