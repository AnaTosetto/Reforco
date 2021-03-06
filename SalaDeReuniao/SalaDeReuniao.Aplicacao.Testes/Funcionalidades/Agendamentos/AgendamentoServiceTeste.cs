﻿using FluentAssertions;
using Moq;
using NUnit.Framework;
using SalaDeReuniao.Comum.Testes.Funcionalidades.Agendamentos;
using SalaDeReuniao.Dominio.Excecoes;
using SalaDeReuniao.Dominio.Funcionalidades.Agendamentos;
using SalaDeReuniao.Dominio.Funcionalidades.Agendamentos.Excecoes;
using SalaDeReuniao.Dominio.Funcionalidades.Funcionarios;
using SalaDeReuniao.Dominio.Funcionalidades.Salas;
using SalaDeReuniao.Funcionalidades.Agendamentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaDeReuniao.Aplicacao.Testes.Funcionalidades.Agendamentos
{
    public class AgendamentoServiceTeste
    {
        AgendamentoService _agendamentoService;

        private Mock<IAgendamentoRepositorio> _mockAgendamentoRepositorio;

        [SetUp]
        public void Inicializar()
        {
            _mockAgendamentoRepositorio = new Mock<IAgendamentoRepositorio>();
            _agendamentoService = new AgendamentoService(_mockAgendamentoRepositorio.Object);
        }

        [Test]
        public void AgendamentoService_Adicionar_DeveSerValido()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            Agendamento novoAgendamento = new Agendamento();
            novoAgendamento.HoraInicial = DateTime.Now.AddHours(4);
            novoAgendamento.HoraFinal = DateTime.Now.AddHours(5);
            novoAgendamento.Sala = sala;
            novoAgendamento.Funcionario = funcionario;
            
            _mockAgendamentoRepositorio.Setup(rp => rp.Adicionar(agendamento)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });
            _mockAgendamentoRepositorio.Setup(rp => rp.ObterTudo()).Returns(new List<Agendamento> {novoAgendamento});
            
            //Ação
            Agendamento retorno = _agendamentoService.Adicionar(agendamento);

            //Verificar
            _mockAgendamentoRepositorio.Verify(rp => rp.Adicionar(agendamento));
            retorno.Should().NotBeNull();
            retorno.Id.Should().BeGreaterThan(0);
            retorno.Id.Should().NotBe(agendamento.Id);
        }

        [Test]
        public void AgendamentoService_Adicionar_SalaIndisponivel_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            Agendamento novoAgendamento = new Agendamento();
            novoAgendamento.Id = 2;
            novoAgendamento.HoraInicial = DateTime.Now.AddHours(1);
            novoAgendamento.HoraFinal = DateTime.Now.AddHours(3);
            novoAgendamento.Sala = sala;
            novoAgendamento.Funcionario = funcionario;

            _mockAgendamentoRepositorio.Setup(rp => rp.ObterTudo()).Returns(new List<Agendamento> { novoAgendamento });

            //Ação
            Action acaoResultado = () => _agendamentoService.Adicionar(agendamento);

            //Verificar
            acaoResultado.Should().Throw<SalaIndisponivelException>();
        }

        [Test]
        public void AgendamentoService_Adicionar_HoraInicialInvalida_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoInvalido_HoraInicialInvalida();
            agendamento.Id = 0;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Adicionar(agendamento)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(-1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });

            //Ação
            Action acaoRetorno = () => _agendamentoService.Adicionar(agendamento);

            //Verificar
            acaoRetorno.Should().Throw<HoraInicialInvalidaException>();
            _mockAgendamentoRepositorio.VerifyNoOtherCalls();
        }

        [Test]
        public void AgendamentoService_Adicionar_HoraFinalInvalida_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoInvalido_HoraFinalInvalida();
            agendamento.Id = 0;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Adicionar(agendamento)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });

            //Ação
            Action acaoRetorno = () => _agendamentoService.Adicionar(agendamento);

            //Verificar
            acaoRetorno.Should().Throw<HoraFinalInvalidaException>();
            _mockAgendamentoRepositorio.VerifyNoOtherCalls();
        }

        [Test]
        public void AgendamentoService_Adicionar_HoraFinalMenorQueHoraInicial_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoInvalido_HoraFinalMenorQueHoraInicial();
            agendamento.Id = 0;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Adicionar(agendamento)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });

            //Ação
            Action acaoRetorno = () => _agendamentoService.Adicionar(agendamento);

            //Verificar
            acaoRetorno.Should().Throw<HoraFinalMenorQueHoraInicialException>();
            _mockAgendamentoRepositorio.VerifyNoOtherCalls();
        }

        [Test]
        public void AgendamentoService_Adicionar_FuncionarioNulo_DeveRetornarExcecao()
        {
            //Cenário
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoInvalido_FuncionarioNulo();
            agendamento.Id = 0;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Adicionar(agendamento)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = null, Sala = sala });

            //Ação
            Action acaoRetorno = () => _agendamentoService.Adicionar(agendamento);

            //Verificar
            acaoRetorno.Should().Throw<FuncionarioNuloException>();
            _mockAgendamentoRepositorio.VerifyNoOtherCalls();
        }

        [Test]
        public void AgendamentoService_Adicionar_SalaVazia_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoInvalido_SalaVazia();
            agendamento.Id = 0;
            agendamento.Funcionario = funcionario;

            _mockAgendamentoRepositorio.Setup(rp => rp.Adicionar(agendamento)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = null });

            //Ação
            Action acaoRetorno = () => _agendamentoService.Adicionar(agendamento);

            //Verificar
            acaoRetorno.Should().Throw<SalaNulaOuVaziaException>();
            _mockAgendamentoRepositorio.VerifyNoOtherCalls();
        }

        [Test]
        public void AgendamentoService_Atualizar_DeveSerValido()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 1;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            Agendamento novoAgendamento = new Agendamento();
            novoAgendamento.Id = 2;
            novoAgendamento.HoraInicial = DateTime.Now.AddHours(3);
            novoAgendamento.HoraFinal = DateTime.Now.AddHours(4);
            novoAgendamento.Sala = sala;
            novoAgendamento.Funcionario = funcionario;

            _mockAgendamentoRepositorio.Setup(rp => rp.Atualizar(agendamento)).Returns(new Agendamento { Id = agendamento.Id, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });
            _mockAgendamentoRepositorio.Setup(rp => rp.ObterTudo()).Returns(new List<Agendamento> { novoAgendamento });

            //Ação
            Agendamento retorno = _agendamentoService.Atualizar(agendamento);

            //Verificar
            _mockAgendamentoRepositorio.Verify(rp => rp.Atualizar(agendamento));
            retorno.Should().NotBeNull();
            retorno.Id.Should().Be(agendamento.Id);
        }

        [Test]
        public void AgendamentoService_Atualizar_SalaIndisponivel_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 1;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            Agendamento novoAgendamento = new Agendamento();
            novoAgendamento.Id = 2;
            novoAgendamento.HoraInicial = DateTime.Now.AddHours(1);
            novoAgendamento.HoraFinal = DateTime.Now.AddHours(3);
            novoAgendamento.Sala = sala;
            novoAgendamento.Funcionario = funcionario;

            _mockAgendamentoRepositorio.Setup(rp => rp.ObterTudo()).Returns(new List<Agendamento> { novoAgendamento });

            //Ação
            Action acaoResultado = () => _agendamentoService.Atualizar(agendamento);

            //Verificar
            acaoResultado.Should().Throw<SalaIndisponivelException>();
        }

        [Test]
        public void AgendamentoService_Atualizar_IdentificadorIndefinido_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 0;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Atualizar(agendamento)).Returns(agendamento);

            //Ação
            Action acaoResultado = () => _agendamentoService.Atualizar(agendamento);

            //Verificar
            acaoResultado.Should().Throw<IdentificadorIndefinidoException>();
            
        }

        [Test]
        public void AgendamentoService_Excluir_DeveSerValido()
        {
            //Cenário
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 1;

            _mockAgendamentoRepositorio.Setup(rp => rp.Excluir(agendamento));

            //Ação
            _agendamentoService.Excluir(agendamento);

            //Verificar
            _mockAgendamentoRepositorio.Verify(rp => rp.Excluir(agendamento));
        }

        [Test]
        public void AgendamentoService_Excluir_IdentificadorIndefinido_DeveRetornarExcecao()
        {
            //Cenário
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 0;

            _mockAgendamentoRepositorio.Setup(rp => rp.Excluir(agendamento));

            //Ação
            Action acaoResultado = () => _agendamentoService.Excluir(agendamento);

            //Verificar
            acaoResultado.Should().Throw<IdentificadorIndefinidoException>();
        }

        [Test]
        public void AgendamentoService_Obter_DeveSerValido()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 1;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Obter(agendamento.Id)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });

            //Ação
            Agendamento retorno = _agendamentoService.Obter(agendamento.Id);

            //Verificar
            _mockAgendamentoRepositorio.Verify(rp => rp.Obter(agendamento.Id));

            retorno.Should().NotBeNull();
            retorno.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public void AgendamentoService_Obter_IdentificadorIndefinido_DeveRetornarExcecao()
        {
            //Cenário
            Funcionario funcionario = new Funcionario();
            funcionario.Id = 1;
            Sala sala = new Sala();
            sala.Id = 1;
            Agendamento agendamento = ObjectMother.ObterAgendamentoValido();
            agendamento.Id = 0;
            agendamento.Funcionario = funcionario;
            agendamento.Sala = sala;

            _mockAgendamentoRepositorio.Setup(rp => rp.Obter(agendamento.Id)).Returns(new Agendamento { Id = 1, HoraInicial = DateTime.Now.AddHours(1), HoraFinal = DateTime.Now.AddHours(2), Funcionario = funcionario, Sala = sala });

            //Ação
            Action acaoResultado = () => _agendamentoService.Obter(agendamento.Id);

            //Verificar
            acaoResultado.Should().Throw<IdentificadorIndefinidoException>();
        }

        [Test]
        public void AgendamentoService_ObterTudo_DeveSerValido()
        {
            //Cenário
            _mockAgendamentoRepositorio.Setup(rp => rp.ObterTudo()).Returns(Enumerable.Empty<Agendamento>);

            //Ação
            IEnumerable<Agendamento> retorno = _agendamentoService.ObterTudo();

            //Verificar
            foreach (Agendamento agendamento in retorno)
            {
                agendamento.Id.Should().BeGreaterThan(0);
                agendamento.Should().NotBeNull();
            }

            _mockAgendamentoRepositorio.Verify(rp => rp.ObterTudo());
        }
    }
}
