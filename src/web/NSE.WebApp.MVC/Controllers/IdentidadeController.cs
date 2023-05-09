using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
        private readonly IAutenticacaoService _autenticationService;

        public IdentidadeController(IAutenticacaoService autenticationService)
        {
            _autenticationService = autenticationService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<ActionResult> Registro(UsuarioRegistroViewModel usuarioRegistro)
        {
            if (!ModelState.IsValid)
                return View(usuarioRegistro);

            //API - Registro
            var resposta = await _autenticationService.Registro(usuarioRegistro);

            if (false) 
                return View(usuarioRegistro);

            //Realizar Login
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLoginViewModel usuarioLogin)
        {
            if(!ModelState.IsValid)
                return View(usuarioLogin);

            //API - Registro
            var resposta = await _autenticationService.Login(usuarioLogin);

            if (false) 
                return View(usuarioLogin);

            //Realizar Login
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
