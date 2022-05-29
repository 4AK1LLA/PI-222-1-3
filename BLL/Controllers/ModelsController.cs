using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModelsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.ModelRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _unitOfWork.ModelRepository.GetAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,ModelName")] Model model)
        {
        check:
            if (model.ModelId == 0)
            {
                model.ModelId = 1;
            }

            if (await _unitOfWork.ModelRepository.GetAsync(model.ModelId) != null)
            {
                model.ModelId++;
                goto check;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.ModelRepository.AddAsync(model);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _unitOfWork.ModelRepository.GetAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,ModelName")] Model model)
        {
            if (id != model.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ModelRepository.Update(model);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ModelId))
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
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _unitOfWork.ModelRepository.GetAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _unitOfWork.ModelRepository.GetAsync(id);
            _unitOfWork.ModelRepository.Remove(model);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int id)
        {
            return _unitOfWork.ModelRepository.NotEmpty(id);
        }
    }
}
