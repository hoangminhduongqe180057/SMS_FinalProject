using Microsoft.AspNetCore.Mvc;

namespace StudentsManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            try
            {
                // Ensure the "Images" directory exists in wwwroot
                var uploadsPath = Path.Combine(_env.WebRootPath, "Images");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Extract file extension (e.g., .jpg, .png)
                var fileExtension = Path.GetExtension(file.FileName);

                // Generate a unique file name with the original extension
                var fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{fileExtension}";

                // Full file path
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save the file to the path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the file name (with extension) so it can be stored in the database
                return Ok(new { filePath = $"{fileName}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "File upload failed", error = ex.Message });
            }
        }
    }

}
