using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ResumeBuilderAIAgent.Controllers
{
    [Route("[controller]")]
    public class ResumeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ResumeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        [HttpPost("GenerateResume")]
        public async Task<IActionResult> GenerateResume(
            string name,
            string email,
            string phone,
            string skills,
            string experience,
            string education,
            string jobRole)
        {
            string geminiApiKey = _configuration["Gemini:ApiKey"];

            var prompt = $@"
You are a professional resume writer.
Name: {name}
Email: {email}
Phone: {phone}
Skills: {skills}
Experience: {experience}
Education: {education}
Job Role: {jobRole}
Generate a professional ATS-friendly resume.
";
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={geminiApiKey}",
                new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseString);
            var resume = json.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            ViewBag.GeneratedResume = resume;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}