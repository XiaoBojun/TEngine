using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface IUI_HUD
    {
        void Update_Level(string _level);

        void Update_Money(int _money);

        void Update_CountDown(int _countDown);
    }
}