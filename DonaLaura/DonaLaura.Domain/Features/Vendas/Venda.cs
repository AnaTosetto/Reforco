﻿using DonaLaura.Domain.Exceptions;
using DonaLaura.Domain.Features.Vendas.Exceptions;
using DonaLaura.Dominio.Features.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonaLaura.Domain.Features.Vendas
{
    public class Venda
    {
        public long Id { get; set; }
        public Produto Produto { get; set; }
        public string NomeCliente { get; set; }
        public int Quantidade { get; set; }
        public double Lucro
        {
            get
            {
                return PrecoVenda() - PrecoCusto();
            }
        }

        public void Validate()
        {
            if (Quantidade <= 0)
                throw new QuantidadeMenorOuIgualZeroException();
            if (string.IsNullOrEmpty(NomeCliente))
                throw new NomeNuloOuVazioException();
        }

        public double PrecoVenda()
        {
            return Quantidade * Produto.PrecoVenda;
        }

        public double PrecoCusto()
        {
            return Quantidade * Produto.PrecoCusto;
        }
    }
}
