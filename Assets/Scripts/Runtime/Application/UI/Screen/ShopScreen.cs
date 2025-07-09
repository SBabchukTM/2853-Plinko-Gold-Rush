using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class ShopScreen : UiScreen
    {
        [SerializeField] private Button _goBackButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private RectTransform _shopItemsParent;

        public event Action OnBackPressed;

        private void OnDestroy()
        {
            _goBackButton.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            SubscribeToEvents();
        }

        public void UpdateBalance(int balance) => _balanceText.text = balance.ToString();

        public void SetShopItems(List<ShopItemDisplay> items)
        {
            foreach (ShopItemDisplay item in items)
                item.transform.SetParent(_shopItemsParent, false);
        }

        private void SubscribeToEvents()
        {
            _goBackButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }
    }
}