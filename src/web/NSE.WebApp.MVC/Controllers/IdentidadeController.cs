﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
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

            if (ResponsePossuiErros(resposta.ResponseResult))
                return View(usuarioRegistro);

            //Realizar Login
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLoginViewModel usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(usuarioLogin);

            //API - Registro
            var resposta = await _autenticationService.Login(usuarioLogin);

            if (ResponsePossuiErros(resposta.ResponseResult))
                return View(usuarioLogin);

            //Realizar Login
            await RealizarLogin(resposta);

            if(string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioRespostaLoginViewModel usuarioResposta)
        {
            var token = ObterTokenFormatado(usuarioResposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", usuarioResposta.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = System.DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        }
    }
}
