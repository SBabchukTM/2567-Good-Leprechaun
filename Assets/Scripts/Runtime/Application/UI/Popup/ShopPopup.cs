using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Services.Audio;
using Application.Services.UserData;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.UI
{
    public class ShopPopup : BasePopup
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private List<ShopItemView> _shopItemViews;
        private UserDataService _userDataService;

        [Inject]
        public void Construct(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _backButton.onClick.AddListener(DestroyPopup);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            Initialization();

            return base.Show(data, cancellationToken);
        }

        private void Initialization()
        {
            UpdateMoney();

            var boughtButtons = _userDataService.GetUserData().BoughtButtonsId;
            _shopItemViews.First().Buy();

            foreach (var shopItem in _shopItemViews)
            {
                shopItem.BuyButton.onClick.AddListener(() => OnBuyButtonPress(shopItem));
                shopItem.UseButton.onClick.AddListener(() => OnUseButtonPress(shopItem));

                if (boughtButtons.Contains(shopItem.Id))
                {
                    shopItem.Buy();
                }
                else
                {
                    shopItem.UnUse();
                }
            }
            _shopItemViews[_userDataService.GetUserData().UsedButtonId].Use();
        }

        private void UpdateMoney()
        {
            var money = _userDataService.GetUserData().Money;
            _moneyText.text = money.ToString();
        }

        private void OnBuyButtonPress(ShopItemView shopItemView)
        {

            int itemId = shopItemView.Id;
            int itemCost = shopItemView.Cost;

            var userMoney = _userDataService.GetUserData().Money;
            if (userMoney >= itemCost)
            {
                _userDataService.GetUserData().Money -= itemCost;
                _userDataService.SaveUserData();

                _userDataService.GetUserData().BoughtButtonsId.Add(itemId);
                shopItemView.Buy();
            }
            UpdateMoney();
        }

        private void OnUseButtonPress(ShopItemView shopItemView)
        {
            _userDataService.GetUserData().UsedButtonId = shopItemView.Id;
            _userDataService.GetUserData().UsedButtonColor = shopItemView.ButtonColor;
            _userDataService.SaveUserData();
            foreach (var item in _shopItemViews)
            {
                item.UnUse();
            }

            shopItemView.Use();
        }
    }
}