using DAL.Contexts;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BLL.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AutoShowContext _context;

        public OrdersController(IUnitOfWork unitOfWork, AutoShowContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.OrderTypeId = id;
            ViewBag.OrderName = name;
            var orderByorderType = _context.Orders.Where(b => b.OrderTypeId == id).Include(b => b.OrderType).Include(b=> b.Manager).Include(b=> b.Model).Include(b=> b.Customer);
            return View(await orderByorderType.ToListAsync());
        }

        public async Task<IActionResult> IndexOrder()
        {
            return View(await _context.Orders.Include(b => b.Manager).Include(b => b.Model).Include(b => b.Customer).Include(b => b.OrderType).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(c=>c.Manager)
                .Include(c=>c.Model)
                .Include(c=>c.Customer)
                .Include(c=>c.OrderType)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Create(int orderTypeId)
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewBag.OrderTypeId = orderTypeId;
            ViewBag.OrderName = _context.OrderTypes.Where(c => c.OrderTypeId == orderTypeId).FirstOrDefault().OrderName;
            return View();
        }

        public IActionResult CreateOrder()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int orderTypeId, [Bind("OrderId,ManagerId,ModelId,CustomerId,OrderTypeId,OrderDate")] Order order)
        {
        check:
            if (order.OrderId == 0)
            {
                order.OrderId = 1;
            }

            if (await _unitOfWork.OrderRepository.GetAsync(order.OrderId) != null)
            {
                order.OrderId++;
                goto check;
            }

            order.OrderTypeId = orderTypeId;
            if (ModelState.IsValid)
            {
                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction("Index", "Orders", new { id = orderTypeId, name = _context.OrderTypes.Where(c => c.OrderTypeId == orderTypeId).FirstOrDefault().OrderName });
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);

            return RedirectToAction("Index", "Orders", new { id = orderTypeId, name = _context.OrderTypes.Where(c => c.OrderTypeId == orderTypeId).FirstOrDefault().OrderName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder( [Bind("OrderId,ManagerId,ModelId,CustomerId,OrderTypeId,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(IndexOrder));
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);

            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ManagerId,ModelId,CustomerId,OrderTypeId,OrderDate")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.OrderRepository.Update(order);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexOrder));
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);
            return View(order);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(c => c.Manager)
                .Include(c => c.Model)
                .Include(c => c.Customer)
                .Include(c => c.OrderType)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(id);
            _unitOfWork.OrderRepository.Remove(order);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(IndexOrder));
        }

        private bool OrderExists(int id)
        {
            return _unitOfWork.OrderRepository.NotEmpty(id);
        }
    }
}
