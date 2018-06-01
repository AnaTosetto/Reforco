﻿using SalaDeReuniao.Dominio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaDeReuniao.Dominio.Funcionalidades.Funcionarios.Excecoes
{
    public class CargoNuloOuVazioException : ExcecaoDeNegocio
    {
        public CargoNuloOuVazioException() : base("O cargo do funcionário não pode ser nulo.")
        {
        }
    }
}
