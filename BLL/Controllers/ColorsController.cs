using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Controllers
{
    public class ColorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ColorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.ColorRepository.GetAllAsync());
        }

        [HttpGet("api/[controller]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.ColorRepository.GetAllAsync());
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> Get2(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            return Ok(color);
        }

        public async Task<ActionResult<Color>> Details(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColorId,ColorName")] Color color)
        {
        check:
            if (color.ColorId == 0)
            {
                color.ColorId = 1;
            }

            if (await _unitOfWork.ColorRepository.GetAsync(color.ColorId) != null)
            {
                color.ColorId++;
                goto check;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.ColorRepository.AddAsync(color);
                await _unitOfWork.ConfirmAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(color);
        }

        [HttpPost("api/[controller]")]
        public async Task<IActionResult> Post([FromBody][Bind("ColorId,ColorName")] Color color)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ColorRepository.AddAsync(color);
                await _unitOfWork.ConfirmAsync();
            }
            return Ok(color);
        }
       
        public async Task<IActionResult> Edit(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            return View(color);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColorId,ColorName")] Color color)
        {
            if (id != color.ColorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ColorRepository.Update(color);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.ColorId))
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
            return View(color);
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody][Bind("ColorId,ColorName")] Color color)
        {
            if (id != color.ColorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ColorRepository.Update(color);
                    await _unitOfWork.ConfirmAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.ColorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(color);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            return View(color);
        }

        [HttpDelete("api/[controller]/{id}")]
        public async Task<IActionResult> Ddelete(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetAsync(id);
            _unitOfWork.ColorRepository.Remove(color);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetAsync(id);
            _unitOfWork.ColorRepository.Remove(color);
            await _unitOfWork.ConfirmAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorExists(int id)
        {
            return _unitOfWork.ColorRepository.NotEmpty(id);
        }
    }
}
