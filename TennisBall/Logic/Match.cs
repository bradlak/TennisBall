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
using TennisBall.Infrastructure;

namespace TennisBall.Logic
{
    [Serializable]
    public class Match : StatesReminder<Match>
    {
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }

        public List<Set> PlayedSets = new List<Set>();

        public bool finished = false;

        public Match(string p1name, string p2name, PlayerNumber firstServer) : base()
        {
            this.PlayerOne = new Player(p1name,PlayerNumber.One);
            this.PlayerTwo = new Player(p2name,PlayerNumber.Two);
            GetPlayerByNumber(firstServer).IsServer = true;

            States = new List<Match>();
        }


        public void AddPoint(PlayerNumber number)
        {
            if (States.Any(z => z.StateId > this.StateId))
            {
                States.Clear();
            }

            if (!States.Any(z => z.StateId == this.StateId))
            {
                SaveCurrentState(this);
            }
       
            var player = GetPlayerByNumber(number);
            var opponent = GetPlayerOpponent(number);

            if (player.IsTieBreak && opponent.IsTieBreak)
            {
                player.GamePoints++;

                if ((player.GamePoints + opponent.GamePoints) % 2 == 0) ChangeServer();

                if (player.GamePoints >= 7 && Math.Abs(player.GamePoints - opponent.GamePoints) >= 2) AddGame(number);
            }
            else
            {
                if ((player.GamePoints == 3 && opponent.GamePoints < 3) || player.HasAdventage) this.AddGame(number);
                else if (opponent.HasAdventage) opponent.HasAdventage = false;
                else if (player.GamePoints == 3 && opponent.GamePoints == 3) player.HasAdventage = true;
                else player.GamePoints++;
            }

            player.TotalPoints++;


            StateId = States.Max(z => z.StateId) + 1;
            SaveCurrentState(this);
        }

        private void AddGame(PlayerNumber number)
        {
            var player = GetPlayerByNumber(number);
            var opponent = GetPlayerOpponent(number);

            ChangeServer();

            if ((player.Games >= 5 && opponent.Games <= 4) || player.Games >= 6)
            {
                this.AddSet(number);
            }
            else if(player.Games >= 5 && opponent.Games == 6)
            {
                player.IsTieBreak = true;
                opponent.IsTieBreak = true;

                if (player.StartNumber == PlayerNumber.One) PlayerOne.Games++;
                else PlayerTwo.Games++;
            }
            else  player.Games++;

            player.GamePoints = 0;
            opponent.GamePoints = 0;
            player.HasAdventage = false;
            opponent.HasAdventage = false;
        }

        public void AddSet(PlayerNumber number)
        {
            var player = GetPlayerByNumber(number);
            var opponent = GetPlayerOpponent(number);

            if (player.StartNumber == PlayerNumber.One) PlayerOne.Games++;
            else PlayerTwo.Games++;

            int nr = PlayedSets.Max(z => (int?)z.Number) ?? 0;

            string p1games = player.IsTieBreak ? String.Format("{0}({1})",PlayerOne.Games.ToString(), PlayerOne.GamePoints.ToString())  : PlayerOne.Games.ToString();
            string p2games = opponent.IsTieBreak ? String.Format("{0}({1})", PlayerTwo.Games.ToString(), PlayerTwo.GamePoints.ToString()) : PlayerTwo.Games.ToString();

            PlayedSets.Add(new Set() {Winner = player.StartNumber, Loser = opponent.StartNumber, Player1Games = p1games, Player2Games = p2games, Number = nr+1, Player1Name = PlayerOne.Name,Player2Name = PlayerTwo.Name});

            player.Games = 0;
            opponent.Games = 0;
            player.IsTieBreak = false;
            opponent.IsTieBreak = false;
        }


        public string GetWinner()
        {
            string name = null;
            if (PlayedSets.Count(z => z.Winner== PlayerNumber.One) >= 2) name =  PlayerOne.Name;
            else if (PlayedSets.Count(z => z.Winner == PlayerNumber.Two) >= 2) name =  PlayerTwo.Name;

            if (!String.IsNullOrEmpty(name)) finished = true;
            return name;
        }

        private void ChangeServer()
        {
            if (PlayerOne.IsServer)
            {
                PlayerOne.IsServer = false;
                PlayerTwo.IsServer = true;
            }

            else if (PlayerTwo.IsServer)
            {
                PlayerTwo.IsServer = false;
                PlayerOne.IsServer = true;
            }
        }

        public PlayerNumber CurrentServer
        {
            get
            {
                if (PlayerOne.IsServer) return PlayerNumber.One;
                else return PlayerNumber.Two;
            }
        }

        private Player GetPlayerByNumber(PlayerNumber number)
        {
            if (number == PlayerNumber.One) return PlayerOne;
            else return PlayerTwo;
        }

        private Player GetPlayerOpponent(PlayerNumber number)
        {
            if (number == PlayerNumber.One) return PlayerTwo;
            else return PlayerOne;
        }

        public override bool CanGoBack
        {
            get
            {
                return States.SingleOrDefault(z => z.StateId == StateId - 1) != null;
            }
        }
        protected override void SetPreviousState()
        {
            Match previousState = States.SingleOrDefault(z => z.StateId == StateId - 1);
            var tempState = ObjectCloner.Clone<Match>(previousState);
            this.StateId = tempState.StateId;
            this.PlayerOne = tempState.PlayerOne;
            this.PlayerTwo = tempState.PlayerTwo;
            this.PlayedSets = tempState.PlayedSets;
        }

        public override bool CanGoForward
        {
            get
            {
                return States.SingleOrDefault(z => z.StateId == StateId + 1) != null;
            }
        }

        protected override void SetNextState()
        {
            Match nextState = States.SingleOrDefault(z => z.StateId == StateId + 1);
            var tempState = ObjectCloner.Clone<Match>(nextState);
            this.StateId = tempState.StateId;
            this.PlayerOne = tempState.PlayerOne;
            this.PlayerTwo = tempState.PlayerTwo;
            this.PlayedSets = tempState.PlayedSets;
        }
    }
}