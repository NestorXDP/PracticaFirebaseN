using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using PracticaFirebase.Models;
using System.Diagnostics;

namespace PracticaFirebase.Controllers
{
    public class HomeController : Controller
{
        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            // Leemos el archivo subido
            Stream archivoASubir = archivo.OpenReadStream();

            string email = "nestor.diaz@catolica.edu.sv";
            string clave = "2021DP650";
            string ruta = "gs://practica-23e01.appspot.com";
            string api_key = "AIzaSyAFn6yq1nFl376JBooicW1Uu1yWx84zXpY";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            
            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                        new FirebaseStorageOptions
                                                        {
                                                            AuthTokenAsyncFactory = () => Task.FromResult(tokenUser), ThrowOnCancel = true
                                                        }
                                                        ).Child("Archivos")
                                                        .Child(archivo.FileName)
                                                        .PutAsync(archivoASubir, cancellation.Token);

          
            var urlArchivoCargado = await tareaCargarArchivo;

           
            return RedirectToAction("VerImagen");
        }

    

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
