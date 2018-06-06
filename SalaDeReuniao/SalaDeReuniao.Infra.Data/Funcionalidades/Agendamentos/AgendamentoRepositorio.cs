﻿using SalaDeReuniao.Dominio.Excecoes;
using SalaDeReuniao.Dominio.Funcionalidades.Agendamentos;
using SalaDeReuniao.Dominio.Funcionalidades.Funcionarios;
using SalaDeReuniao.Dominio.Funcionalidades.Salas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaDeReuniao.Infra.Data.Funcionalidades.Agendamentos
{
    public class AgendamentoRepositorio : IAgendamentoRepositorio
    {
        string _sqlInserir = @"INSERT INTO Agendamento
                                (HoraInicial, HoraFinal, FuncionarioId, SalaId)
                              VALUES
                                (@HoraInicial, @HoraFinal, @FuncionarioId, @SalaId);";

        string _sqlEditar = @"UPDATE Agendamento
                                SET HoraInicial = @HoraInicial, HoraFinal = @HoraFinal, FuncionarioId = @FuncionarioId, SalaId = @SalaId
                             WHERE Id = @Id";

        string _sqlExcluir = @"DELETE FROM Agendamento
                                WHERE Id = @Id";

        string _sqlObter = @"SELECT * FROM Agendamento
                                WHERE Id = @Id";

        string _sqlObterTudo = @"SELECT * FROM Agendamento";

        public Agendamento Adicionar(Agendamento agendamento)
        {
            agendamento.Validar();
            agendamento.Id = Db.Insert(_sqlInserir, Take(agendamento));
            return agendamento;
        }

        public Agendamento Atualizar(Agendamento agendamento)
        {
            agendamento.Validar();
            Db.Update(_sqlEditar, Take(agendamento));
            return agendamento;
        }

        public void Excluir(Agendamento agendamento)
        {
            Db.Delete(_sqlExcluir, Take(agendamento));
        }

        public Agendamento Obter(int id)
        {
            return Db.Get<Agendamento>(_sqlObter, Make, new object[] { "@Id", id });
        }

        public IEnumerable<Agendamento> ObterTudo()
        {
            return Db.GetAll<Agendamento>(_sqlObterTudo, Make);
        }

        private object[] Take(Agendamento agendamento)
        {
            return new object[]
            {
                "@Id", agendamento.Id,
                "@HoraInicial", agendamento.HoraInicial,
                "@HoraFinal", agendamento.HoraFinal,
                "@SalaId", agendamento.Sala.Id,
                "@FuncionarioId", agendamento.Funcionario.Id
            };
        }

        private static Func<IDataReader, Agendamento> Make = reader =>
        new Agendamento
        {
            Id = Convert.ToInt32(reader["Id"]),
            HoraInicial = Convert.ToDateTime(reader["HoraInicial"]),
            HoraFinal = Convert.ToDateTime(reader["HoraFinal"]),
            Sala = new Sala
            {
                Id = Convert.ToInt32(reader["SalaId"]),
            },
            Funcionario = new Funcionario
            {
                Id = Convert.ToInt32(reader["FuncionarioId"]),
            }
        };

    }
}
