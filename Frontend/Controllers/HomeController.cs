using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Frontend.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Books()
        {
            List<BookModel> books = new List<BookModel>();
            var client = new HttpClient();
            await Task.Run(() =>
            {
                var response = client.GetAsync("https://localhost:7199/api/Books").Result;

                if(response.IsSuccessStatusCode)
                { 
                    var data = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<BookModel>>(data);

                    foreach(var item in result)
                    {
                        if(item != null)
                        {
                            books.Add(item);
                        }
                    }

                }
            });

            return View(books);
        }
        public IActionResult CreateBook()
        {
            return View();
        }

        public async Task<IActionResult> AddBook(BookModel book)
        {
            book.BookId = Guid.NewGuid().ToString();

            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(book);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7199/api/Books", httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("dodano ksiazke");
            }
            else
            {
                Console.WriteLine("nie udalo sie dodac ksiazki");
            }
            Console.WriteLine(json);

            return RedirectToAction("Books");
        }

        public IActionResult EditBook(BookModel book)
        {
            return View(book);
        }

        public async Task<IActionResult> EditBookById(BookModel book)
        {
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(book);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"https://localhost:7199/api/Books/{book.BookId}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("zmieniono ksiazke");
            }
            else
            {
                Console.WriteLine("nie udalo sie zmienic ksiazki");
            }
            Console.WriteLine(json);

            return RedirectToAction("Books");
        }

        public IActionResult DeleteBook(BookModel book)
        {
            return View(book);
        }

        public async Task<IActionResult> DeleteBookById(string id)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync($"https://localhost:7199/api/Books/{id}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"usunieto ksiazke o id {id}");
            }
            else
            {
                Console.WriteLine($"nie udalo sie zmienic ksiazki o id {id}");
            }

            return RedirectToAction("Books");
        }

        public IActionResult BookDetails(BookModel book)
        {
            return View(book);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
