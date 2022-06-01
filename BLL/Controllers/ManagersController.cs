using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Controllers
{
   
    public class ManagersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.ManagerRepository.GetAllAsync());
        }

        public async Task<ActionResult<Manager>> Details(int id)
        {
            var manager = await _unitOfWork.ManagerRepository.GetAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerId,FullName,Telephone")] Manager manager)
        {
        check:
            if (manager.ManagerId == 0)
            {
                manager.ManagerId = 1;
            }

            if (await _unitOfWork.ManagerRepository.GetAsync(manager.ManagerId) != null)
            {
                manager.ManagerId++;
                goto check;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.ManagerRepository.AddAsync(manager);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var manager = await _unitOfWork.ManagerRepository.GetAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerId,FullName,Telephone")] Manager manager)
        {
            if (id != manager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ManagerRepository.Update(manager);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.ManagerId))
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
            return View(manager);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var manager = await _unitOfWork.ManagerRepository.GetAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _unitOfWork.ManagerRepository.GetAsync(id);
            _unitOfWork.ManagerRepository.Remove(manager);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return _unitOfWork.ManagerRepository.NotEmpty(id);
        }
    }
}
