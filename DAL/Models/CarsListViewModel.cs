using Microsoft.EntityFrameworkCore;
using DAL.Contexts;

namespace DAL.Models
{
    public class CarsListViewModel
    {
        public CarsListViewModel(AutoShowContext context)
        {
            Cars = context.Cars;
            Colors = context.Colors;
            Drives = context.Drives;
            SelectedColor = null;
            SelectedDrive = null;
           
        }
        public IEnumerable<Car> Cars;
        public DbSet<Color> Colors;
        public int? SelectedColor { get; set; }
        public DbSet<Drive> Drives;
        public int? SelectedDrive{ get; set; }
    }
}
