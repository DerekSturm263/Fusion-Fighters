﻿using System.Collections.Generic;
using System.Linq;

namespace Quantum
{
    [System.Serializable]
    public abstract unsafe partial class WinCondition : InfoAsset
    {
        public virtual bool IsMatchOver(Frame f, IEnumerable<Team> teams)
        {
            // Get if the match timer hits 0.
            bool isMatchOver = f.Global->IsTimerOver;
            if (f.Global->CurrentMatch.Ruleset.Players.StockCount == -1)
                return isMatchOver;

            // Get if 1 or fewer teams have any players left alive.
            bool isOneTeamLeft = teams.Count(team => team.Get(f).Any(item => f.Unsafe.GetPointer<Stats>(FighterIndex.GetPlayerFromIndex(f, item.Index))->CurrentStats.Stocks > 0)) < 2;

            // Return true (match is over) if the match timer hits 0 OR if 1 or fewer teams have any players left alive.
            return isMatchOver || isOneTeamLeft;
        }
        
        public abstract System.Func<Team, int> SortTeams(Frame f, IEnumerable<Team> teams);
    }
}
