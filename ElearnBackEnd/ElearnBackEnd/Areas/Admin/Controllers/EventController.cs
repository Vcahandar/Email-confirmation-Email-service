using ElearnBackEnd.Data;
using ElearnBackEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElearnBackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        public EventController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>  Index()
        {
            IEnumerable<Event> indexEvent = await _context.Events.Where(m => !m.SoftDelete).ToListAsync();
            return View(indexEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Event? indexEvent = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (indexEvent == null) return NotFound();

            return View(indexEvent);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Event? indexEvent = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (indexEvent == null) return NotFound();

            return View(indexEvent);
        }
    }
}
