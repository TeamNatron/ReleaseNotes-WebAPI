using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes_WebAPI.Domain.Services;

namespace ReleaseNotes_WebAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ImagesController(
            IImageService imageService,
            IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _imageService.SaveToFilesystem(file);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{imageUrl}")]
        public async Task<IActionResult> GetImage([FromRoute] string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return BadRequest("Finner ingen imageUrl i addressen..");

            var fileStream = await _imageService.GetAsync(imageUrl);
            return File(fileStream, "image/png");
        }
    }
}