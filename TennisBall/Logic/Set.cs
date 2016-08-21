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

namespace TennisBall.Logic
{
    [Serializable]
    public class Set
    {
        public int Number { get; set; }

        public PlayerNumber Winner { get; set; }

        public string Player1Games { get; set; }

        public PlayerNumber Loser { get; set; }

        public string Player2Games { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

    }
}