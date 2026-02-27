using TEngine;

namespace GameLogic
{
    public class FightSys:Singleton<FightSys>
    {
        public void EnterFight() {
            GameModule.Audio.Play(AudioType.UISound, Constants.BGRoom);
            GameModule.UI.ShowUIAsync<InfoWindow>();
            
        }
    }
}