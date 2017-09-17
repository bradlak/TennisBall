using SQLite;
using System;
using System.IO;
using TennisBall.Entites;
using Mono.Data.Sqlite;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using SQLite.Net.Interop;

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
            using (var connection = new SqliteConnection(path))
            {
                return true;
            }
        }

        public bool CreateTable<T>() where T : BaseEntity
        {
            using (var connection = new SQLiteConnection(GetPlatform(),path))
            {
                connection.CreateTable<T>();
            }
            return true;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(GetPlatform(), path);
        }


        private ISQLitePlatform GetPlatform()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
			{
                return new SQLitePlatformAndroidN();
			}

            return new SQLitePlatformAndroid();
        }
    }
}
