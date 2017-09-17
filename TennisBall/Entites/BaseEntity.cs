using System;
using SQLite;
using SQLite.Net.Attributes;

namespace TennisBall.Entites
{
    [Serializable]
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}