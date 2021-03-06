﻿using Prova2.Dominio.Features.Emprestimos;
using Prova2.Dominio.Features.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova2.Comum.Testes.Features.Emprestimos
{
    public static partial class ObjectMother
    {
        public static Emprestimo ObterEmprestimoValido()
        {
            return new Emprestimo
            {
                NomeCliente = "Cliente",
                DataDevolucao = DateTime.Now.AddDays(2)
            };
        }

        public static Emprestimo ObterEmprestimoInvalido_NomeClienteNuloOuVazio()
        {
            return new Emprestimo
            {
                NomeCliente = "",
                DataDevolucao = DateTime.Now.AddDays(2)
            };
        }
    }
}
