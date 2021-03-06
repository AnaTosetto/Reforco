﻿using SalaDeReuniao.Dominio.Excecoes;
using SalaDeReuniao.Dominio.Funcionalidades.Agendamentos;
using SalaDeReuniao.Dominio.Funcionalidades.Agendamentos.Excecoes;
using SalaDeReuniao.Funcionalidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaDeReuniao.Funcionalidades.Agendamentos
{
    public class AgendamentoService : IService<Agendamento>
    {
        IAgendamentoRepositorio _agendamentoRepositorio;

        public AgendamentoService(IAgendamentoRepositorio agendamentoRepositorio)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
        }

        public Agendamento Adicionar(Agendamento agendamento)
        {
            agendamento.Validar();
            VerificarSalaDisponivel(agendamento);

            return _agendamentoRepositorio.Adicionar(agendamento);
        }

        public Agendamento Atualizar(Agendamento agendamento)
        {
            agendamento.Validar();
            VerificarSalaDisponivel(agendamento);

            if (agendamento.Id < 1)
                throw new IdentificadorIndefinidoException();

            return _agendamentoRepositorio.Atualizar(agendamento);
        }

        public void Excluir(Agendamento agendamento)
        {
            if (agendamento.Id < 1)
                throw new IdentificadorIndefinidoException();

            _agendamentoRepositorio.Excluir(agendamento);
        }

        public Agendamento Obter(int id)
        {
            if (id < 1)
                throw new IdentificadorIndefinidoException();

            return _agendamentoRepositorio.Obter(id);
        }

        public IEnumerable<Agendamento> ObterTudo()
        {
            return _agendamentoRepositorio.ObterTudo();
        }

        public void VerificarSalaDisponivel(Agendamento agendamento)
        {
            IEnumerable<Agendamento> lista = _agendamentoRepositorio.ObterTudo();

            foreach (Agendamento a in lista)
            {
                if (agendamento.Sala.Id == a.Sala.Id)
                {
                    if (agendamento.HoraInicial.Day == a.HoraInicial.Day)
                    {
                        if (agendamento.HoraInicial.Hour == a.HoraInicial.Hour
                            || (agendamento.HoraInicial.Hour > a.HoraInicial.Hour && agendamento.HoraInicial.Hour < a.HoraFinal.Hour)
                            || (agendamento.HoraInicial.Hour < a.HoraInicial.Hour && agendamento.HoraFinal.Hour > a.HoraInicial.Hour)
                            || (agendamento.HoraFinal.Hour > a.HoraFinal.Hour && agendamento.HoraInicial.Hour < a.HoraFinal.Hour)
                            || (agendamento.HoraFinal.Hour < a.HoraFinal.Hour && agendamento.HoraFinal.Hour > a.HoraInicial.Hour)
                            || (agendamento.HoraInicial.Hour < a.HoraInicial.Hour && agendamento.HoraFinal.Hour > a.HoraFinal.Hour)
                            )
                            throw new SalaIndisponivelException();
                    }
                }
            }
        }
    }
}
