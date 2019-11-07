using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using Npgsql;

namespace DNVDO.Model.Data
{
    public class DataAccessLayer
    {
        private static string serverName = "127.0.0.1";
        private static string port = "5432";
        private static string userName = "postgres";
        private static string password = "123456";
        private static string databaseName = "Registro";
        private static string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", serverName, port, userName, password, databaseName);
        private static NpgsqlConnection conn;
        private static bool connectionOpened = false;

        // Open/Close Connection
        private static void DBOpenConnection()
        {
            try
            {
                NpgsqlConnectionStringBuilder connectionStringBuilder = new NpgsqlConnectionStringBuilder();
                connectionStringBuilder.Clear();
                Console.WriteLine("Openning a DataBase Connection.");
                connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
                conn = new NpgsqlConnection(connectionStringBuilder.ToString());
                conn.Open();
                connectionOpened = true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Open a DataBase Connection.");
                Console.WriteLine("Error: " + e.Message);
                connectionOpened = false;
                throw;
            }
        }
        private static void DBCloseConnection()
        {
            try
            {
                conn.Close();
                connectionOpened = false;
                Console.WriteLine("DataBase Connection Closed.");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Close a DataBase Connection.");
                Console.WriteLine("Error: " + e.Message);
                connectionOpened = false;
            }
        }

        /*
         * CRUD
         */
        // Create
        public static bool InputRegistry(Registro newRegistry)
        {
            if (!connectionOpened)
            {
                try
                {
                    DBOpenConnection();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Returning without any Input.");
                    return false;
                }
            }
            try
            {
                int newID = NextRegistryID();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.\"REGISTRY\" (RegistryNumber, BookName, BookNumber, PageNumber, RegistryDate, Name, Sex, BirthDate, BirthHour, FatherName, FatherBirthDate, FatherBirthCity, MotherName, MotherBirthDate, MotherBirthCity, DocmentNumber, IsOnDeadline)"
                                + "VALUES (" + newID + ", '" + newRegistry.Book_Name + "', " + newRegistry.Book_Number + ", " + newRegistry.Page_Number + ", '" + newRegistry.Registry_Date + "', '" + newRegistry.Name + "', '" + newRegistry.Sex.value.ToString() + "', '" + newRegistry.BirthDate + "', '" + newRegistry.BirthHour + "', '" + newRegistry.FatherName + "', '" + newRegistry.FatherBirthDate + "', '" + newRegistry.FatherBirthCity + "', '" + newRegistry.MotherName + "', '" + newRegistry.MotherBirthDate + "', '" + newRegistry.MotherBirthCity + "', '" + newRegistry.DocumentNumber + "', " + (newRegistry.IsOnDeadline ? 1 : 0) + ");", conn);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Input in the Table 'REGISTRY' in the DataBase.");
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
            DBCloseConnection();
            return true;
        }
        
        private static int NextRegistryID()
        {
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT MAX(RegistryNumber) AS LASTID FROM REGISTRY", conn);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    return (int) reader["LASTID"] + 1;
                else
                    return 1;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Get the Next ID for the Table 'REGISTRY' in the DataBase.");
                Console.WriteLine("Error: " + e.Message);
                throw;
            }
        }
        // Read
        public static List<Registro> ReadRegistry()
        {
            try
            {
                DBOpenConnection();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Returning a null Connection.");
                return null;
            }
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.\"REGISTRY\"", conn);
                NpgsqlDataReader reader = command.ExecuteReader();
                List<Registro> registros = new List<Registro>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        registros.Add(new Registro((int)reader["RegistryNumber"], reader["BookName"].ToString(),
                                    (int)reader["BookNumber"], (int)reader["PageNumber"],
                                    reader["RegistryDate"].ToString(), reader["Name"].ToString(),
                                    reader["Sex"].ToString(), reader["BirthDate"].ToString(),
                                    reader["BirthHour"].ToString(), reader["FatherName"].ToString(),
                                    reader["FatherBirthDate"].ToString(), reader["FatherBirthCity"].ToString(),
                                    reader["MotherName"].ToString(), reader["MotherBirthDate"].ToString(),
                                    reader["MotherBirthCity"].ToString(), reader["DocmentNumber"].ToString(),
                                    (int)reader["IsOnDeadline"]));
                    }
                }
                DBCloseConnection();
                return registros;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Get Tables from the DataBase.");
                Console.WriteLine("Error: " + e.Message);
                throw;
            }
        }
        public static List<Registro> ReadRegistry(int searchRegistry)
        {
            try
            {
                DBOpenConnection();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Returning a null Connection.");
                return null;
            }
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.\"REGISTRY\" WHERE RegistryNumber = " + searchRegistry + " ", conn);
                NpgsqlDataReader reader = command.ExecuteReader();
                List<Registro> registros = new List<Registro>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        registros.Add(new Registro((int)reader["RegistryNumber"], reader["BookName"].ToString(),
                                    (int)reader["BookNumber"], (int)reader["PageNumber"],
                                    reader["RegistryDate"].ToString(), reader["Name"].ToString(),
                                    reader["Sex"].ToString(), reader["BirthDate"].ToString(),
                                    reader["BirthHour"].ToString(), reader["FatherName"].ToString(),
                                    reader["FatherBirthDate"].ToString(), reader["FatherBirthCity"].ToString(),
                                    reader["MotherName"].ToString(), reader["MotherBirthDate"].ToString(),
                                    reader["MotherBirthCity"].ToString(), reader["DocmentNumber"].ToString(),
                                    (int)reader["IsOnDeadline"]));
                    }
                }
                DBCloseConnection();
                return registros;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Get Tables from the DataBase.");
                Console.WriteLine("Error: " + e.Message);
                throw;
            }
        }
        // Update
        public static bool UpdateRegistry(Registro newRegistry)
        {
            if (!connectionOpened)
            {
                try
                {
                    DBOpenConnection();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Returning without any Input.");
                    return false;
                }
            }
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("UPDATE public.\"REGISTRY\" "
                                + "SET RegistryNumber = " + newRegistry.Registry_Number + ", BookName = '" + newRegistry.Book_Name + "', BookNumber = " + newRegistry.Book_Number + ", PageNumber = " + newRegistry.Page_Number + ", RegistryDate = '" + newRegistry.Registry_Date.ToString() + "', Name = '" + newRegistry.Name + "', Sex = '" + newRegistry.Sex.value.ToString() + "', BirthDate = '" + newRegistry.BirthDate.ToString() + "', BirthHour = '" + newRegistry.BirthHour.ToString() + "', FatherName = '" + newRegistry.FatherName.ToString() + "', FatherBirthDate = '" + newRegistry.FatherBirthDate.ToString() + "', FatherBirthCity = '" + newRegistry.FatherBirthCity.ToString() + "', MotherName = '" + newRegistry.MotherName + "', MotherBirthDate = '" + newRegistry.MotherBirthDate.ToString() + "', MotherBirthCity = '" + newRegistry.MotherBirthCity.ToString() + "', DocumentNumber = '" + newRegistry.DocumentNumber.ToString() + "', IsOnDeadline = " + (newRegistry.IsOnDeadline ? 1 : 0) + " "
                                + "WHERE Registry_Number = " + newRegistry.Registry_Number + ";", conn);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when trying to Update data on the Table 'REGISTRY' in the DataBase.");
                Console.WriteLine("Error: " + e.Message);
                throw;
            }
            DBCloseConnection();
            return true;
        }
        
        /*
         * Por questão de segurança os comandos de DELETE não foram implementados, a boa prática sugere que os 
         * dados sejam desativados, por meio de um campo booleano de Active e quando for ser 'removido' apenas 
         * marcado como inativo. O projeto não previa um campo para esse controle e também não previa a remoção 
         * de registros, apenas a inclusão e busca dos mesmos.
         */
        // Delete
        private void DeleteSex() {}
        private void DeleteEstate() {}
        private void DeleteCity() {}
        private void DeleteRegistry() {}
    }
}