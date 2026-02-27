using Fantasy.Entitas;

namespace Fantasy;

public sealed class Account : Entity
{
    public string acct;
    public string pwd;
    public string name;
    public int lv;
    public int exp;
    public int coin;
    public int diamond;
    public int win;
    public int lose;
    public int winlast;
}