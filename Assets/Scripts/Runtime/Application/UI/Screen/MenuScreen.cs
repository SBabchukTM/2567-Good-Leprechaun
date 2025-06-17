using System;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _easyLevelButton;
        [SerializeField] private Button _middleLevelButton;
        [SerializeField] private Button _hardLevelButton;

        public event Action OptionsButtonPressEvent;
        public event Action<int> PlayButtonPressEvent;

        private void OnDestroy()
        {
            _optionsButton.onClick.RemoveAllListeners();
            _easyLevelButton.onClick.RemoveAllListeners();
            _middleLevelButton.onClick.RemoveAllListeners();
            _hardLevelButton.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            _optionsButton.onClick.AddListener(OnOptionsButtonPress);
            _easyLevelButton.onClick.AddListener(() => OnPlayButtonPress(0));
            _middleLevelButton.onClick.AddListener(() => OnPlayButtonPress(1));
            _hardLevelButton.onClick.AddListener(() => OnPlayButtonPress(2));
        }

        private void OnPlayButtonPress(int id)
        {
            PlayButtonPressEvent?.Invoke(id);
        }

        private void OnOptionsButtonPress()
        {
            OptionsButtonPressEvent?.Invoke();
        }
    }
}