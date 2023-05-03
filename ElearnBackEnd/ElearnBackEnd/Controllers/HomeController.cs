
using ElearnBackEnd.Data;
using ElearnBackEnd.Model;
using ElearnBackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ElearnBackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context= context;
        }


        public async Task<IActionResult>  Index()
        {
            IEnumerable<Slider>sliders = await _context.Sliders.ToListAsync();
            IEnumerable<Course>courses = await _context.Courses.Include(m=>m.Images).Include(m=>m.Author).Where(m=>!m.SoftDelete).OrderByDescending(m=>m.Id).ToArrayAsync();
            IEnumerable<Event> events = await _context.Events.ToArrayAsync();
            IEnumerable<News> news = await _context.News.Where(m => !m.SoftDelete).ToArrayAsync();



            HomeVM model = new HomeVM
            {
                Sliders = sliders,
                Courses= courses,
                Events = events,
                News= news
                
            };

            return View(model);
        }




    }
}