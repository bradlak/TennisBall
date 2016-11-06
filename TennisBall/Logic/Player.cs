using System;

namespace TennisBall.Logic
{
    [Serializable]
    public class Player
    {
        public Player(string name, PlayerNumber number)
        {
            this.Name = name;
            this.StartNumber = number;
        }

        public PlayerNumber StartNumber { get; set; }

        public string Name { get; set; }

        public int TotalPoints { get; set; }

        public int GamePoints { get; set; }

        public int Games { get; set; }

        public int Sets { get;  set; }

        public bool IsServer { get; set; }

        public bool HasAdventage { get; set; }

        public bool IsTieBreak { get; set; }

        public string DisplayPoints
        {
            get
            {
                if (IsTieBreak) return GamePoints.ToString();
                else if (!HasAdventage)
                {
                    if (GamePoints == 0) return "0";
                    if (GamePoints == 1) return "15";
                    if (GamePoints == 2) return "30";
                    if (GamePoints >= 3) return "40";
                }
                return "AD";
            }
        }     
    }
}