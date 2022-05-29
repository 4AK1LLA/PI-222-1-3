using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Controllers
{
    public class OrderTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.OrderTypeRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderType = await _unitOfWork.OrderTypeRepository.GetAsync(id);
            if (orderType == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Orders", new { id = orderType.OrderTypeId, name = orderType.OrderName });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderTypeId,OrderName")] OrderType orderType)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.OrderTypeRepository.AddAsync(orderType);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var orderType = await _unitOfWork.OrderTypeRepository.GetAsync(id);
            if (orderType == null)
            {
                return NotFound();
            }
            return View(orderType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderTypeId,OrderName")] OrderType orderType)
        {
            if (id != orderType.OrderTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.OrderTypeRepository.Update(orderType);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderTypeExists(orderType.OrderTypeId))
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
            return View(orderType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var orderType = await _unitOfWork.OrderTypeRepository.GetAsync(id);
            if (orderType == null)
            {
                return NotFound();
            }
            return View(orderType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderType = await _unitOfWork.OrderTypeRepository.GetAsync(id);
            _unitOfWork.OrderTypeRepository.Remove(orderType);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderTypeExists(int id)
        {
            return _unitOfWork.OrderTypeRepository.NotEmpty(id);
        }
    }
}
