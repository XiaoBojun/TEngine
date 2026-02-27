using Fantasy.Entitas;

namespace Fantasy;

public sealed class GateComponent:Entity
{
    public readonly Dictionary<string, Account> Accounts = new Dictionary<string, Account>();
    
    public List<ScoreRank> scoreRankLst = new List<ScoreRank>();
    public List<ArenaRank> arenaRankLst = new List<ArenaRank>();
}