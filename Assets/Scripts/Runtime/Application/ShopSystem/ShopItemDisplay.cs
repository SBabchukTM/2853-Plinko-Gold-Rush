using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private Button _purchaseButton;

    private bool _inAnim = false;

    private ShopItem _shopItem;
    public ShopItem ShopItem => _shopItem;

    public event Action<ShopItemDisplay> OnPurchasePressed;

    private void OnDestroy()
    {
        _purchaseButton.onClick.RemoveAllListeners();
    }

    public void Initialize(ShopItem shopItem)
    {
        _shopItem = shopItem;

        _itemImage.sprite = _shopItem.ItemSprite;
        _priceText.text = _shopItem.ItemPrice.ToString();

        _purchaseButton.onClick.AddListener(() => OnPurchasePressed?.Invoke(this));
    }

    public void SetStatus(string status) => _statusText.text = status;

    public async UniTaskVoid Shake(CancellationToken token, PurchaseFailedShakeParameters purchaseFailedShakeParameters)
    {
        if (_inAnim)
            return;

        _inAnim = true;
        _purchaseButton.transform.DOShakePosition(purchaseFailedShakeParameters.ShakeDuration, 
                                          purchaseFailedShakeParameters.Strength, 
                                          purchaseFailedShakeParameters.Vibrato, 
                                          purchaseFailedShakeParameters.Randomness,
                                          purchaseFailedShakeParameters.Snapping,
                                          purchaseFailedShakeParameters.FadeOut, 
                                          purchaseFailedShakeParameters.ShakeRandomnessMode).SetLink(gameObject);

        await UniTask.WaitForSeconds(purchaseFailedShakeParameters.ShakeDuration);
        token.ThrowIfCancellationRequested();

        _inAnim = false;
    }
}
