﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace mypass.Model
{
    public class ActionsDB : DataBase
    {
        private int _idaction;
        public int IDAction
        {
            get => _idaction;
            set => _idaction = value;
        }
        private int _idaccount;
        public int IDAccount
        {
            get => _idaccount;
            set => _idaccount = value;
        }
        private int _idevent;
        public int IDEvent
        {
            get => _idevent;
            set => _idevent = value;
        }
        private DateTime _timeevent;
        public DateTime TimeEvent
        {
            get => _timeevent;
            set => _timeevent = value;
        }

        // Инициализация конструктора
        public ActionsDB(string databasePath, string password) : base() { }

        // Метод для добавления новой записи
        public void AddAction(int idAccount, int idEvent, DateTime timeEvent)
        {
            OpenConnection();
            string query = "INSERT INTO Actions (IdAccount, IdEvent, TimeEvent) VALUES (@IdAccount, @IdEvent, @TimeEvent);";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@IdEvent", idEvent);
                command.Parameters.AddWithValue("@TimeEvent", timeEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для обновления записи
        public void UpdateAction(int idAction, int idAccount, int idEvent, DateTime timeEvent)
        {
            OpenConnection();
            string query = @"UPDATE Actions 
                             SET IdAccount = @IdAccount, IdEvent = @IdEvent, TimeEvent = @TimeEvent 
                             WHERE IdAction = @IdAction;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAction", idAction);
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@IdEvent", idEvent);
                command.Parameters.AddWithValue("@TimeEvent", timeEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для удаления записи
        public void DeleteAction(int idAction)
        {
            OpenConnection();
            string query = "DELETE FROM Actions WHERE IdAction = @IdAction;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAction", idAction);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для получения записи в виде Dictionary
        public Dictionary<string, string> GetActionById(int idAction)
        {
            OpenConnection();
            string query = "SELECT * FROM Actions WHERE IdAction = @IdAction;";
            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAction", idAction);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result["IdAction"] = reader["IdAction"].ToString();
                        result["IdAccount"] = reader["IdAccount"].ToString();
                        result["IdEvent"] = reader["IdEvent"].ToString();
                        result["TimeEvent"] = reader["TimeEvent"].ToString();
                    }
                }
            }
            CloseConnection();
            return result;
        }
    }
}
