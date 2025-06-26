
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface IUI_Battle
    {
        public void SetCursorImageColor(Color color);
        public void ShowCursorImage(bool show);
    }
}