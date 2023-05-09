﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models
{
    public class UsuarioRegistroViewModel
    {
        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage ="O campo {0} está em um formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage ="O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength =3)]
        public string Senha { get; set; }

        [DisplayName("Confirme sua senha")]
        [Compare("Senha", ErrorMessage ="As senhas não conferem")]
        public string SenhaConfirmacao { get; set; }
    }
}
