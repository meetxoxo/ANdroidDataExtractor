using System;
using System.Collections.Generic;
using System.Data.SQLite;
using AndroidDataExtractor.Models;

namespace AndroidDataExtractor.Services
{
    public class DatabaseManager
    {
        private readonly string connectionString = "Data Source=androiddata.db;Version=3;";

        public DatabaseManager()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS ContactsTable (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    PhoneNumber TEXT,
                    Email TEXT
                );
                CREATE TABLE IF NOT EXISTS MessagesTable (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Sender TEXT,
                    MessageContent TEXT,
                    Timestamp TEXT
                );
                CREATE TABLE IF NOT EXISTS CallLogsTable (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    PhoneNumber TEXT,
                    CallType TEXT,
                    Duration TEXT
                );
                CREATE TABLE IF NOT EXISTS DeviceInfoTable (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CPUInfo TEXT,
                    MemoryInfo TEXT
                );
            ";
            command.ExecuteNonQuery();
        }

        public void InsertContacts(List<Contact> contacts)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            foreach (var contact in contacts)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO ContactsTable (Name, PhoneNumber, Email) VALUES (@Name, @Phone, @Email)";
                cmd.Parameters.AddWithValue("@Name", contact.Name);
                cmd.Parameters.AddWithValue("@Phone", contact.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", contact.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertMessages(List<Message> messages)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            foreach (var msg in messages)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO MessagesTable (Sender, MessageContent, Timestamp) VALUES (@Sender, @Content, @Time)";
                cmd.Parameters.AddWithValue("@Sender", msg.Sender);
                cmd.Parameters.AddWithValue("@Content", msg.MessageContent);
                cmd.Parameters.AddWithValue("@Time", msg.Timestamp);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertCallLogs(List<CallLog> logs)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            foreach (var log in logs)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO CallLogsTable (PhoneNumber, CallType, Duration) VALUES (@Phone, @Type, @Duration)";
                cmd.Parameters.AddWithValue("@Phone", log.PhoneNumber);
                cmd.Parameters.AddWithValue("@Type", log.CallType);
                cmd.Parameters.AddWithValue("@Duration", log.Duration);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertDeviceInfo(DeviceInfo info)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO DeviceInfoTable (CPUInfo, MemoryInfo) VALUES (@CPU, @Memory)";
            cmd.Parameters.AddWithValue("@CPU", info.CPUInfo);
            cmd.Parameters.AddWithValue("@Memory", info.MemoryInfo);
            cmd.ExecuteNonQuery();
        }

        internal void InsertMessages(List<System.Windows.Forms.Message> messages)
        {
            throw new NotImplementedException();
        }
    }
}
