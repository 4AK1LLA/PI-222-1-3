using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.ServiceRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await _unitOfWork.ServiceRepository.GetAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,TypeOfService")] Service service)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ServiceRepository.AddAsync(service);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _unitOfWork.ServiceRepository.GetAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,TypeOfService")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ServiceRepository.Update(service);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
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
            return View(service);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var service = await _unitOfWork.ServiceRepository.GetAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _unitOfWork.ServiceRepository.GetAsync(id);
            _unitOfWork.ServiceRepository.Remove(service);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _unitOfWork.ServiceRepository.NotEmpty(id);
        }
    }
}
