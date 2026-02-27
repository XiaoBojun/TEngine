using Fantasy.Entitas;

namespace Fantasy;

public class PokerRoom:Entity
{
    public int RoomID { private set; get; }
    List<Poker> pokerLst = new List<Poker>();
    List<Poker> keepLst = new List<Poker>();
}