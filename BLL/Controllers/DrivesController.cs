using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Controllers
{
    public class DrivesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DrivesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.DriveRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var drive = await _unitOfWork.DriveRepository.GetAsync(id);
            if (drive == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Cars", new { id = drive.DriveId, name = drive.DriveType });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriveId,DriveType")] Drive drife)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DriveRepository.AddAsync(drife);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drife);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var drive = await _unitOfWork.DriveRepository.GetAsync(id);
            if (drive == null)
            {
                return NotFound();
            }
            return View(drive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DriveId,DriveType")] Drive drive)
        {
            if (id != drive.DriveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DriveRepository.Update(drive);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriveExists(drive.DriveId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drive);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var drive = await _unitOfWork.DriveRepository.GetAsync(id);
            if (drive == null)
            {
                return NotFound();
            }

            return View(drive);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drive = await _unitOfWork.DriveRepository.GetAsync(id);
            _unitOfWork.DriveRepository.Remove(drive);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriveExists(int id)
        {
            return _unitOfWork.DriveRepository.NotEmpty(id);
        }
    }
}
