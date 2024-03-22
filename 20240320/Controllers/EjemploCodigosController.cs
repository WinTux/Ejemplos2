using Microsoft.AspNetCore.Mvc;

namespace _20240320.Controllers
{
    public class EjemploCodigosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Generar(string valor) { 
            ViewBag.valor = valor;
            return View("Index");
        }
    }
}
