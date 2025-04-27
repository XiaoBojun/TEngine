using TEngine;

using UnityEngine.Rendering.Universal;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface IUI_HUD
    {
        void Update_Level(string _level);

        void Update_Money(int _money);

        void Update_CountDown(string _countDown);
    }
}
