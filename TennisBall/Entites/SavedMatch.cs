using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace TennisBall.Entites
{
    [Table("SavedMatches")]
    public class SavedMatch : BaseEntity
    {
        public string Player1Sets { get; set; }

        public string Player2Sets { get; set; }

        public string Player1Total { get; set; }

        public string Player2Total { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

        public string WinnerName { get; set; }

    }
}