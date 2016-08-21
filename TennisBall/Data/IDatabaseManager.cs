using TennisBall.Entites;
using SQLite;

namespace TennisBall.Data
{
    public interface IDatabaseManager
    {
        bool CreateDatabaseIfNotExist();
        bool CreateTable<T>() where T : BaseEntity;
        SQLiteConnection GetConnection();
    }
}