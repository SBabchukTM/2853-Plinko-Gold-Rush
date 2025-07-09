using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _balanceText;

        public event Action OnMenuPressed;
        public event Action OnPlayPressed;

        private void OnDestroy()
        {
            _menuButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
        }

        public void Initialize(int balance)
        {
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
            _playButton.onClick.AddListener(() => OnPlayPressed?.Invoke());
            _balanceText.text = balance.ToString();
        }
    }
}