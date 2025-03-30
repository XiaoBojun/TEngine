using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface IUI_HUD
    {
        void Update_Level(string level);

        void Update_Money(int money);
    }
}