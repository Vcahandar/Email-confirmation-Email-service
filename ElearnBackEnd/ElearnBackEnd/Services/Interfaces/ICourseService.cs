using ElearnBackEnd.Model;

namespace ElearnBackEnd.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAll();

    }
}
