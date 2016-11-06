using System;
using System.Collections.Generic;
using System.Linq;
using TennisBall.Entites;
using TennisBall.Logic;

namespace TennisBall.Data
{
    public class MatchesRepository : IMatchesRepository
    {
        private IDatabaseManager dbManager;

        public MatchesRepository(IDatabaseManager manager)
        {
            this.dbManager = manager;
        }

        public void AddMatch(IEnumerable<Set> playedSets, string winnerName)
        {
            using (var connection = dbManager.GetConnection())
            {
                try
                {
                    connection.BeginTransaction();

                    SavedMatch match = new SavedMatch();
                    match.Player1Name = playedSets.FirstOrDefault().Player1Name;
                    match.Player2Name = playedSets.FirstOrDefault().Player2Name;

                    match.Player1Sets = string.Join(",", playedSets.Select(z => z.Player1Games));
                    match.Player2Sets = string.Join(",", playedSets.Select(z => z.Player2Games));

                    match.WinnerName = winnerName;

                    connection.Insert(match, typeof(SavedMatch));

                    connection.Commit();
                }
                catch (Exception ex)
                {
                    connection.Rollback();
                }
            }
        }

        public IEnumerable<SavedMatch> GetAllMatches()
        {
            using (var connection = dbManager.GetConnection())
            {
                return connection.Table<SavedMatch>().ToList();
            }
        }

        public void RemoveMatch(int id)
        {
            using (var connection = dbManager.GetConnection())
            {
                connection.Delete<SavedMatch>(id);
            }
        }
    }
}