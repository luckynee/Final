using UnityEngine.Events;

namespace PokerSeed.UI
{
    public interface IUIPopup
    {
        public void Init(UnityAction _openCallback, UnityAction _closeCallback);
        public void OnOpen();

        public void OnClose();
    }
}
