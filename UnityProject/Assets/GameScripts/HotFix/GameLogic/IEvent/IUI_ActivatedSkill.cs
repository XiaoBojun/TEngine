using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface IUI_ActivatedSkill
    {
        public void Action_WaitForCooldown();
    }
}