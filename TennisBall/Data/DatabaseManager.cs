using SQLite;
using System;
using System.IO;
using TennisBall.Entites;

namespace TennisBall.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        private string path;

        public DatabaseManager()
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "matches.db3");
        }

        public bool CreateDatabaseIfNotExist()
        {
            using (var connection = new SQLiteConnection(path))
            {
                return true;
            }
        }

        public bool CreateTable<T>() where T : BaseEntity
        {
            using (var connection = new SQLiteConnection(path))
            {
                connection.CreateTable<T>();
            }

            return true;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(path);
        }
    }
}
