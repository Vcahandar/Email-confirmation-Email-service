using ElearnBackEnd.Data;
using ElearnBackEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElearnBackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        public NewsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<News> news = await _context.News.Include(m => m.Author).Where(s => !s.SoftDelete).ToListAsync();

            return View(news);

        }

        public async Task<IActionResult> Detail(int? id)
        {
            News? news = await _context.News.Include(m => m.Author).Where(s => !s.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
            return View(news);
        }

        public IActionResult Create()
        {
            return View();
        }

    }
}