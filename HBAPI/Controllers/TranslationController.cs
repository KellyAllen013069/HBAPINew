using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HBAPI.Data;
using HBAPI.DTOs;
using HBAPI.Models;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly HbDbContext _dbContext;

        public TranslationController(HbDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: /api/translation/add
        [HttpPost("add")]
        public async Task<IActionResult> AddTranslation([FromBody] TranslationDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Key_Name) || 
                string.IsNullOrWhiteSpace(request.English_Text) || string.IsNullOrWhiteSpace(request.Spanish_Text))
            {
                return BadRequest("Invalid translation data.");
            }

            // Check if the key already exists
            var existingTranslation = _dbContext.Translations
                .FirstOrDefault(t => t.Key_Name == request.Key_Name);

            if (existingTranslation != null)
            {
                return Conflict("A translation with this key already exists.");
            }

            // Create a new Translation object
            var translation = new Translations
            {
                Key_Name = request.Key_Name,
                English_Text = request.English_Text,
                Spanish_Text = request.Spanish_Text
            };

            // Add the new translation to the database
            _dbContext.Translations.Add(translation);
            await _dbContext.SaveChangesAsync();

            // Regenerate JSON files after adding the new translation
            await GenerateJsonFiles();

            return Ok("Translation added successfully.");
        }

        // POST: /api/translation/generate-json
        [HttpPost("generate-json")]
        public async Task<IActionResult> GenerateJsonFiles()
        {
            var translations = _dbContext.Translations.ToList();

            // Create English JSON
            var enTranslations = translations.ToDictionary(t => t.Key_Name, t => t.English_Text);
            System.IO.File.WriteAllText("locales/en.json", JsonConvert.SerializeObject(enTranslations, Formatting.Indented));

            // Create Spanish JSON
            var esTranslations = translations.ToDictionary(t => t.Key_Name, t => t.Spanish_Text);
            System.IO.File.WriteAllText("locales/es.json", JsonConvert.SerializeObject(esTranslations, Formatting.Indented));

            return Ok("Translation files generated successfully.");
        }

        // GET /api/translation/en
        [HttpGet("en")]
        public IActionResult GetEnglishTranslations()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "locales", "en.json");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("English translation file not found.");
            }

            var jsonContent = System.IO.File.ReadAllText(filePath);
            var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
            return Ok(translations);
        }

        // GET /api/translation/es
        [HttpGet("es")]
        public IActionResult GetSpanishTranslations()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "locales", "es.json");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Spanish translation file not found.");
            }

            var jsonContent = System.IO.File.ReadAllText(filePath);
            var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
            return Ok(translations);
        }
    }
}
