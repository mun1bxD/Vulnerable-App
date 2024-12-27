using BaseEncodingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace BaseEncodingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Base64()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Base64(string inputText, IFormFile inputFile)
        {
            string encodedText = null;

            if (!string.IsNullOrEmpty(inputText))
            {
             
                encodedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(inputText));
            }
            else if (inputFile != null)
            {
             
                using (var memoryStream = new MemoryStream())
                {
                    inputFile.CopyTo(memoryStream);
                    encodedText = Convert.ToBase64String(memoryStream.ToArray());
                }
            }


            ViewBag.EncodedText = encodedText;

            return View();
        }
        public IActionResult Decode()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Decode(string inputText, IFormFile inputFile)
        {
            string decodedText = string.Empty;
            string decodedFileContent = string.Empty;

           
            if (!string.IsNullOrEmpty(inputText))
            {
                try
                {
                    byte[] decodedBytes = Convert.FromBase64String(inputText);
                    decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                }
                catch (FormatException)
                {
                    ViewBag.ErrorMessage = "Invalid Base64 string.";
                }
            }

           
            if (inputFile != null && inputFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    inputFile.CopyTo(stream);
                    byte[] fileBytes = stream.ToArray();

                    try
                    {
           
                        decodedFileContent = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(fileBytes)));
                    }
                    catch (FormatException)
                    {
                        ViewBag.ErrorMessage = "The file content is not valid Base64.";
                    }
                }
            }

           
            ViewBag.DecodedText = decodedText;
            ViewBag.DecodedFileContent = decodedFileContent;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
