using MoviePlatform1PL.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;
using System.Security.Claims;

namespace MoviePlatform1.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CategoryController(ICategoryService categoryService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }
        [HttpPost]


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lang = Request.Headers["Accept-Language"].ToString();
            var category = await _categoryService.CreateCategory(request, lang);
            if (category == null)
            {
                return BadRequest(
                    new
                    {
                        data = (object?)null,
                        message = "Category already exists"
                    }
                    
                    );
            }
            return Ok(new
            {
                data = category
            })
            ;
            //return Ok("وصل");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var lang = Request.Headers["Accept-Language"].ToString();
            var categories = await _categoryService.GetAllCategories(lang);
            return Ok(
                new
                {
                    data = categories
                    ,
                    message = _localizer["Success"].Value
                });
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int Id)
        {
            return Ok(await _categoryService.GetCategory(c => c.Id == Id));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _categoryService.DeleteCategory(id);
            if (!delete)
            {
                return NotFound(new
                {
                    message = _localizer["NotFound"].Value
                });
            }
            return NotFound(new
            {
                message = _localizer["Success"].Value
            });
        }
        [HttpPatch("{Id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateRequest request)
        {
            var response = await _categoryService.UpdateCategory(id, request);
            if (!response) return BadRequest();
            return Ok();
        }
    }

        
    }
