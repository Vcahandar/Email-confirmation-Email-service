using ElearnBackEnd.Model;

namespace ElearnBackEnd.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAll();
    }
}
