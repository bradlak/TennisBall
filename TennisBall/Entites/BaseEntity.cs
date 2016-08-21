using System;
using SQLite;

namespace TennisBall.Entites
{
    [Serializable]
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}