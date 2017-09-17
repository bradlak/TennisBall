using TennisBall.Entites;
using SQLite;
using Mono.Data.Sqlite;
using SQLite.Net;

namespace TennisBall.Data
{
    public interface IDatabaseManager
    {
        bool CreateDatabaseIfNotExist();

        bool CreateTable<T>() where T : BaseEntity;

        SQLiteConnection GetConnection();
    }
}