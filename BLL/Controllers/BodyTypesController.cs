using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;

#nullable disable

namespace BLL.Controllers
{
    
    public class BodyTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BodyTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.BodyTypeRepository.GetAllAsync());
        }

        public async Task<ActionResult<BodyType>> Details(int id)
        {
            var bodyType = await _unitOfWork.BodyTypeRepository.GetAsync(id);
            if (bodyType == null)
            {
                return NotFound();
            }

            return View(bodyType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BodyTypeId,BodyTypeNames")] BodyType bodyType)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.BodyTypeRepository.AddAsync(bodyType);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bodyType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BodyTypeId,BodyTypeNames")] BodyType bodyType)
        {
            if (id != bodyType.BodyTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.BodyTypeRepository.Update(bodyType);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyTypeExists(bodyType.BodyTypeId))
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
            return View(bodyType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bodyType = await _unitOfWork.BodyTypeRepository.GetAsync(id);
            if (bodyType == null)
            {
                return NotFound();
            }

            return View(bodyType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bodyType = await _unitOfWork.BodyTypeRepository.GetAsync(id);
            _unitOfWork.BodyTypeRepository.Remove(bodyType);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyTypeExists(int id)
        {
            return _unitOfWork.BodyTypeRepository.NotEmpty(id);
        }
    }
}