using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
            try
            {
                var connection = new SQLite.SQLiteConnection(path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateTable<T>() where T : BaseEntity
        {
            try
            {
                var connection = new SQLite.SQLiteConnection(path);
                connection.CreateTable<T>();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public SQLite.SQLiteConnection GetConnection()
        {
            return new SQLite.SQLiteConnection(path);
        }
    }
}
