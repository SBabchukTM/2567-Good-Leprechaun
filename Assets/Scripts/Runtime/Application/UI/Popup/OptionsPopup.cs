using Application.Services.Audio;
using Application.UI;
using Core.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : BasePopup
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _goToHomeButton;
    [SerializeField] private Button _restartButton;

    public event Action SettingsButtonPressEvent;
    public event Action InfoButtonPressEvent;
    public event Action ShopButtonPressEvent;
    public event Action GoToHomeButtonPressEvent;
    public event Action RestartLevelButtonPressEvent;

    public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
    {
        var optionsPopupData = data as OptionsPopupData;

        _goToHomeButton.gameObject.SetActive(optionsPopupData.IsGameOptions);
        _restartButton.gameObject.SetActive(optionsPopupData.IsGameOptions);

        if (optionsPopupData.IsGameOptions)
        {
            BindButton(_goToHomeButton, () => GoToHomeButtonPressEvent?.Invoke());
            BindButton(_restartButton, () => RestartLevelButtonPressEvent?.Invoke());
        }

        BindButton(_backButton, DestroyPopup);
        BindButton(_settingsButton, () => SettingsButtonPressEvent?.Invoke());
        BindButton(_infoButton, () => InfoButtonPressEvent?.Invoke());
        BindButton(_shopButton, () => ShopButtonPressEvent?.Invoke());

        AudioService.PlaySound(ConstAudio.OpenPopupSound);

        return base.Show(data, cancellationToken);
    }

    public override void DestroyPopup()
    {
        AudioService.PlaySound(ConstAudio.ClosePopupSound);
        Destroy(gameObject);
    }

    private void BindButton(Button button, Action action)
    {
        button.onClick.AddListener(() => HandleButtonPress(action));
    }

    private void HandleButtonPress(Action callback)
    {
        AudioService.PlaySound(ConstAudio.PressButtonSound);
        callback?.Invoke();
        Destroy(gameObject);
    }
}