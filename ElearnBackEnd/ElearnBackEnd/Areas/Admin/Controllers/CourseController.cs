using ElearnBackEnd.Areas.Admin.ViewModels;
using ElearnBackEnd.Data;
using ElearnBackEnd.Helpers;
using ElearnBackEnd.Model;
using ElearnBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace ElearnBackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ICourseService _courseService;
        private readonly IAuthorService _authorService;

        public CourseController(AppDbContext context,
                                ICourseService courseService,
                                IAuthorService authorService,
                                IWebHostEnvironment env)
        {
            _context = context;
            _courseService = courseService;
            _authorService = authorService;
            _env = env;
        }


        private async Task<SelectList> GetAuthorsAsync()
        {
            IEnumerable<Author> authors = await _authorService.GetAll();
            return new SelectList(authors, "Id", "FullName");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            return View(await _courseService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.authors = await GetAuthorsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM createVM)
        {
            try
            {
                ViewBag.authors = await GetAuthorsAsync();

                if (!ModelState.IsValid)
                {
                    return View();
                }


                foreach (var photo in createVM.Photos)
                {

                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File Type must be image");
                        return View();
                    }

                    if (photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image Size must be max 200kb");
                        return View();
                    }
                }

                List<CourseImage> courseImages = new();

                foreach (var photo in createVM.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    CourseImage courseImage = new()
                    {
                        Image = fileName,
                    };

                    courseImages.Add(courseImage);
                }

                courseImages.FirstOrDefault().IsMain = true;

                decimal convertedPrice = decimal.Parse(createVM.Price);


                Course newCourse = new()
                {
                    Title = createVM.Title,
                    Description = createVM.Description,
                    Price = (int)convertedPrice,
                    AuthorId = createVM.AuthorId,
                    Images = courseImages,
                    SaleCount = createVM.Count
                };

                await _context.CourseImages.AddRangeAsync(courseImages);
                await _context.Courses.AddAsync(newCourse);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Course course = await _context.Courses.Include(m=>m.Images).FirstOrDefaultAsync(m => m.Id == id);

                if (course == null) return NotFound();

                foreach (var item in course.Images)
                {
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "images", item.Image);
                    FileHelper.DeleteFile(path);
                }
                

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task <IActionResult> Detail(int? id)
        {
            ViewBag.authors = await GetAuthorsAsync();

            if (id is null) return BadRequest();

            Course course = await _context.Courses.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);

            if (course is null) return NotFound();

            return View(course);

        }


        [HttpGet]
        public async Task <IActionResult> Edit(int? id)
        {
            ViewBag.authors = await GetAuthorsAsync();

            if (id == null) return BadRequest();
            Course dbCourse = await _context.Courses.Include(m=>m.Images).FirstOrDefaultAsync(m => m.Id == id);
            if (dbCourse is null) return NotFound();

            CourseEditVM editVM = new()
            {
                Id = dbCourse.Id,
                Title = dbCourse.Title,
                Price = dbCourse.Price.ToString(),
                AuthorId = dbCourse.AuthorId,
                Description = dbCourse.Description,
                Count = dbCourse.SaleCount,
                CourseImages = dbCourse.Images.ToList()
            };

            return View(editVM);
          
        }

        public async Task <IActionResult> Edit (int? id,CourseEditVM courseEdit)
        {
            if(id==null) return BadRequest();

            ViewBag.authors = await GetAuthorsAsync();

            Course dbCourse = await _context.Courses.AsNoTracking().Include(m => m.Images).Include(m => m.Author).FirstOrDefaultAsync(m => m.Id == id);

            if (dbCourse is null) return NotFound();

            if (!ModelState.IsValid)
            {
                courseEdit.CourseImages= dbCourse.Images.ToList();
                return View(courseEdit);
            }

            List<CourseImage> courseImages = new();

            if (courseEdit.Photos is not null) 
            {
                foreach (var photo in courseEdit.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        courseEdit.CourseImages = dbCourse.Images.ToList();
                        return View(courseEdit);
                    }

                    if (photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        courseEdit.CourseImages = dbCourse.Images.ToList();
                        return View(courseEdit);
                    }
                }

                foreach (var photo in courseEdit.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    CourseImage courseImage = new()
                    {
                        Image = fileName
                    };

                    courseImages.Add(courseImage);
                }

                await _context.CourseImages.AddRangeAsync(courseImages);
            }

            decimal convertedPrice = decimal.Parse(courseEdit.Price);

            Course newCourse = new()
            {
                Id = dbCourse.Id,
                Title = courseEdit.Title,
                Price = (int)convertedPrice,
                Description = courseEdit.Description,
                AuthorId = courseEdit.AuthorId,
                SaleCount = courseEdit.Count,
                Images = courseImages.Count == 0 ? dbCourse.Images : courseImages
            };


            _context.Courses.Update(newCourse);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            if (id == null) return BadRequest();

            bool result = false;

            CourseImage courseImage = await _context.CourseImages.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (courseImage == null) return NotFound();

            var data = await _context.Courses.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == courseImage.CourseId);

            if (data.Images.Count > 1)
            {
                string path = FileHelper.GetFilePath(_env.WebRootPath, "images", courseImage.Image);

                FileHelper.DeleteFile(path);

                _context.CourseImages.Remove(courseImage);

                await _context.SaveChangesAsync();

                result = true;
            }

            data.Images.FirstOrDefault().IsMain = true;

            await _context.SaveChangesAsync();

            return Ok(result);

        }
    }
}
