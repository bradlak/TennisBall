using System.Collections.Generic;
using TennisBall.Logic;
using TennisBall.Entites;

namespace TennisBall.Data
{
    public interface IMatchesRepository
    {
        void AddMatch(IEnumerable<Set> playedSets, string winnerName);
        IEnumerable<SavedMatch> GetAllMatches();
        void RemoveMatch(int id);
    }
}