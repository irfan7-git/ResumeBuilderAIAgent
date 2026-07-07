//using Microsoft.AspNetCore.Mvc;
//using Microsoft.SemanticKernel;
//using ResumeBuilderAIAgent.Models;
//using System.Diagnostics;

//namespace ResumeBuilderAIAgent.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private readonly Kernel _kernel;


//        public HomeController(ILogger<HomeController> logger, Kernel kernel)
//        {
//            _logger = logger;
//            _kernel = kernel;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> GenerateResume(string name, string email, string phone, string skills, string experience, string education, string jobRole)
//        {
//            try
//            {
//                // Load the prompt template from file
//                var prompt = @"
//You are a professional resume writer. Based on the following user input, generate a tailored resume:
//Name: {{""name""}}
//Email: {{""email""}}
//Phone: {{""phone""}}
//Skills: {{""skills""}}
//Experience: {{""experience""}}
//Education: {{""education""}}
//Job Role: {{""jobRole""}}

//Format the resume in a clean, professional layout.
//";



//                // Create the semantic function
//                // Create the semantic function
//                var resumeFunction = _kernel.CreateFunctionFromPrompt(prompt);

//                // Prepare input arguments
//                var arguments = new KernelArguments
//                {
//                    ["name"] = name,
//                    ["email"] = email,
//                    ["phone"] = phone,
//                    ["skills"] = skills,
//                    ["experience"] = experience,
//                    ["education"] = education,
//                    ["jobRole"] = jobRole
//                };

//                // ✅ Correct way to invoke the function
//                var result = await resumeFunction.InvokeAsync(_kernel, arguments);

//                // Pass the generated resume to the view
//                ViewBag.GeneratedResume = result.ToString();
//            }
//            catch (Exception ex)
//            {

//            }
//            return View("Index");
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// Add MVC services
builder.Services.AddControllersWithViews();

// Add HttpClient for Gemini API
builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
