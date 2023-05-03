using ElearnBackEnd.Data;
using ElearnBackEnd.Model;
using ElearnBackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElearnBackEnd.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.ToListAsync();
        }
    }
}
