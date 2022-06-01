using BLL.ViewModels;
using DAL.Contexts;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BLL.Controllers
{

    public class CarsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AutoShowContext _context;

        public CarsController(IUnitOfWork unitOfWork, AutoShowContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        private IEnumerable<Car> ApplyFilters(int? drive, int? color, int? year)
        {
            IEnumerable<Car> cars = from s in _context.Cars
                                                      .Include(x => x.Drive)
                                                      .Include(x => x.Color) select s;
            if (drive != null)
            {
                cars = cars.Where(c => c.DriveId == drive);
            }
            if (color != null)
            {
                cars = cars.Where(c => c.ColorId == color);
            }
            if (year != null)
            {
                cars = cars.Where(c => c.GraduationYear == year);
            }
            return cars;
        }

        public async Task<IActionResult> IndexAuto(int? drive, int? color, int? year)
        {
            CarsListViewModel listModelView = new CarsListViewModel(_context);
            listModelView.Cars = ApplyFilters(drive, color, year);
            return View(listModelView);
        }
       
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.DriveId = id;
            ViewBag.DriveType = name;
            var autoByDrives = _context.Cars.Where(b => b.DriveId == id).Include(b => b.Drive).Include(j => j.Model).Include(j => j.BodyType).Include(j => j.Color);
            return View(await autoByDrives.ToListAsync());
        }

        public async Task<IActionResult> IndexM(int? id, string? name)
        {
            ViewBag.BodyTypeId = id;
            ViewBag.BodyTypeNames = name;
            var autoByBodyTypes = _context.Cars.Where(b => b.BodyTypeId == id).Include(b => b.BodyType).Include(j => j.Drive).Include(j => j.Model).Include(j => j.Color);
            return View(await autoByBodyTypes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.BodyType)
                .Include(c => c.Color)
                .Include(c => c.Drive)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public IActionResult CreateAuto()
        {
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuto(CarViewModel cvm)
        {
        check:
            if (cvm.CarId == 0)
            {
                cvm.CarId = 1;
            }

            if (await _unitOfWork.CarRepository.GetAsync(cvm.CarId) != null)
            {
                cvm.CarId++;
                goto check;
            }

            Car car = new Car
            {
                BodyTypeId = cvm.BodyTypeId,
                ColorId = cvm.ColorId,
                DriveId = cvm.DriveId,
                ModelId = cvm.ModelId,
                Price = cvm.Price,
                GraduationYear = cvm.GraduationYear,
                Description = cvm.Description,
                CarId = cvm.CarId
            };

            if (ModelState.IsValid)
            {
                if (cvm.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(cvm.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)cvm.Image.Length);
                    }
                    car.Image = imageData;
                }
                await _unitOfWork.CarRepository.AddAsync(car);
                await _unitOfWork.ConfirmAsync();

                return RedirectToAction(nameof(IndexAuto));

            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            return View(car);
        }
        
        public IActionResult Create(int driveId)
        {
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewBag.DriveId = driveId;
            ViewBag.DriveType = _context.Drives.Where(c => c.DriveId == driveId).FirstOrDefault().DriveType;

            return View();
        }

        public IActionResult CreateM(int bodytypeId)
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewData["PhotoId"] = new SelectList(_context.Models, "PhotoId", "Image");
            ViewBag.BodyTypeId = bodytypeId;
            ViewBag.BodyTypeNames = _context.BodyTypes.Where(c => c.BodyTypeId == bodytypeId).FirstOrDefault().BodyTypeNames;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int driveId, [Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId,Image,Description")] Car car)
        {
            car.DriveId = driveId;

            if (ModelState.IsValid)
            {
                await _unitOfWork.CarRepository.AddAsync(car);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction("Index", "Cars", new { id = driveId, name = _context.Drives.Where(c => c.DriveId == driveId).FirstOrDefault().DriveType });
            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            return RedirectToAction("Index", "Cars", new { id = driveId, name = _context.Drives.Where(c => c.DriveId == driveId).FirstOrDefault().DriveType });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateM(int bodytypeId, [Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId,Image,Description")] Car car)
        {
            car.BodyTypeId = bodytypeId;
            if (ModelState.IsValid)
            {
                await _unitOfWork.CarRepository.AddAsync(car);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction("IndexM", "Cars", new { id = bodytypeId, name = _context.BodyTypes.Where(c => c.BodyTypeId == bodytypeId).FirstOrDefault().BodyTypeNames });
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);

            return RedirectToAction("IndexM", "Cars", new { id = bodytypeId, name = _context.BodyTypes.Where(c => c.BodyTypeId == bodytypeId).FirstOrDefault().BodyTypeNames });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await _unitOfWork.CarRepository.GetAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId,Image,Description")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CarRepository.Update(car);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAuto));
            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.BodyType)
                .Include(c => c.Color)
                .Include(c => c.Drive)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _unitOfWork.CarRepository.GetAsync(id);
            _unitOfWork.CarRepository.Remove(car);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(IndexAuto));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
