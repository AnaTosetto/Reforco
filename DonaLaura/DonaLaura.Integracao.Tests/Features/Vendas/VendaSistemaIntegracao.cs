﻿using DonaLaura.Aplicacao.Features.Vendas;
using DonaLaura.Common.Tests.Features;
using DonaLaura.Domain.Exceptions;
using DonaLaura.Domain.Features.Vendas;
using DonaLaura.Domain.Features.Vendas.Exceptions;
using DonaLaura.Dominio.Features.Produtos;
using DonaLaura.Infra.Data.Features.Vendas;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonaLaura.Integracao.Tests.Features.Vendas
{
    public class VendaSistemaIntegracao
    {
        VendaRepository _vendaRepository = new VendaRepository();

        VendaService _vendaService;

        [SetUp]
        public void TestSetup()
        {
            _vendaService = new VendaService(_vendaRepository);
        }

        [Test]
        public void VendaSistemaIntegracao_Adicionar_DeveRetornarOk()
        {
            //Arrange
            Produto produto = new Produto();
            long idProduto = 1;
            produto.Id = idProduto;
            Venda venda = ObjectMother.getValidoVenda();
            venda.Id = 0;
            venda.Produto = produto;

            IEnumerable<Venda> timelineAntes = _vendaService.ObtemTudo();

            //Action
            Venda vendaResultado = _vendaService.Adiciona(venda);

            //Verify
            vendaResultado.Should().NotBeNull();
            vendaResultado.Id.Should().BeGreaterThan(0);
            vendaResultado.NomeCliente.Should().Be(venda.NomeCliente);
            vendaResultado.Quantidade.Should().Be(venda.Quantidade);
            vendaResultado.Lucro.Should().Be(venda.Lucro);
            vendaResultado.Produto.Should().Be(venda.Produto);

            Venda vendaGet = _vendaService.Obtem(vendaResultado.Id);

            vendaResultado.Id.Should().Be(vendaGet.Id);

            IEnumerable<Venda> timelineDepois = _vendaService.ObtemTudo();

            timelineDepois.Should().NotBeEmpty();
            timelineDepois.Count().Should().BeGreaterThan(timelineAntes.Count());

            _vendaService.Exclui(vendaResultado);
        }

        [Test]
        public void VendaSistemaIntegracao_Adicionar_NomeNuloOuVazio_DeveRetornarExcecao()
        {
            //Arrange
            Produto produto = new Produto();
            long idProduto = 1;
            produto.Id = idProduto;
            Venda venda = ObjectMother.getNomeNuloOuVazioVenda();
            venda.Id = 0;
            venda.Produto = produto;

            //Action
            Action acaoResultado = () => _vendaService.Adiciona(venda);

            //Verify
            acaoResultado.Should().Throw<NomeNuloOuVazioException>();
        }

        [Test]
        public void VendaSistemaIntegracao_Adicionar_QuantidadeMenorOuIgualZero_DeveRetornarExcecao()
        {
            //Arrange
            Produto produto = new Produto();
            long idProduto = 1;
            produto.Id = idProduto;
            Venda venda = ObjectMother.getQuantidadeMenorOuIgualZeroVenda();
            venda.Id = 0;
            venda.Produto = produto;

            //Action
            Action acaoResultado = () => _vendaService.Adiciona(venda);

            //Verify
            acaoResultado.Should().Throw<QuantidadeMenorOuIgualZeroException>();
        }

    }
}