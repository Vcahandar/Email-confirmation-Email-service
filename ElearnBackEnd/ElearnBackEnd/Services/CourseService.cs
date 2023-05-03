using ElearnBackEnd.Data;
using ElearnBackEnd.Model;
using ElearnBackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElearnBackEnd.Services
{
    public class CourseService:ICourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Courses.Include(m=>m.Images).Include(m=>m.Author).Where(m=>!m.SoftDelete).ToListAsync();
        }
    }
}
