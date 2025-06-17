using Application.Game;
using Application.Services.UserData;
using Application.UI;
using Core;
using Core.Services.Audio;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BaseSlotGameStateController : StateController
{
    private readonly IUiService _uiService;
    private readonly IAudioService _audioService;
    private readonly UserDataService _userDataService;
    protected int _columnCount = 3;
    protected int _spinSpeed = 3500;

    protected SlotGameScreen _screen;

    public BaseSlotGameStateController(ILogger logger,
         IUiService uiService,
         IAudioService audioService,
         UserDataService userDataService
        ) : base(logger)
    {
        _uiService = uiService;
        _audioService = audioService;
        _userDataService = userDataService;
    }

    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        await Initialization();
    }

    public override async UniTask Exit()
    {
        await base.Exit();
        UnsubscribeScreen();
        await _uiService.HideScreen(ConstScreens.SlotGameScreen);
    }

    protected virtual async UniTask Initialization()
    {
        await CreateScreen();
        _screen.ShowAsync().Forget();
        await UniTask.CompletedTask;
    }

    protected virtual async UniTask CreateScreen()
    {
        _screen = _uiService.GetScreen<SlotGameScreen>(ConstScreens.SlotGameScreen);
        SubscribeScreen();
        _screen.Initialize(_columnCount, _spinSpeed);
        await UniTask.CompletedTask;
    }

    protected virtual void SubscribeScreen()
    {
        _screen.GameOverEvent += GameOver;
        _screen.OptionsButtonPressEvent += ShowOptionsPopup;
    }

    protected virtual void UnsubscribeScreen()
    {
        _screen.GameOverEvent -= GameOver;
    }

    protected virtual void RestartLevel()
    {
        GoTo<BaseSlotGameStateController>();
    }

    protected virtual void GoToHome()
    {
        GoTo<MenuStateController>();
    }

    protected virtual void GameOver(bool isWin, int score)
    {
        var gameOverPopup = _uiService.GetPopup<GameOverPopup>(ConstPopups.GameOverPopup);
        gameOverPopup.Show(new GameOverPopupData() { IsWin = isWin, WinCost = score });
        gameOverPopup.RestartLevelPressEvent += RestartLevel;
        gameOverPopup.GoToHomeButtonPressEvent += GoToHome;
    }

    protected virtual void ShowOptionsPopup()
    {
        OptionsPopup optionsPopup = _uiService.GetPopup<OptionsPopup>(ConstPopups.OptionsPopup);

        optionsPopup.SettingsButtonPressEvent += ShowSettingsPopup;
        optionsPopup.InfoButtonPressEvent += ShowInfoPopup;
        optionsPopup.ShopButtonPressEvent += ShowShopPopup;
        optionsPopup.GoToHomeButtonPressEvent += GoToHome;
        optionsPopup.RestartLevelButtonPressEvent += RestartLevel;
        optionsPopup.Show(new OptionsPopupData(true));
    }

    private void ShowShopPopup()
    {
        var shopPopup = _uiService.GetPopup<ShopPopup>(ConstPopups.ShopPopup);
        shopPopup.Show(default);
        shopPopup.DestroyPopupEvent += _screen.GetUserData;
    }

    private void ShowSettingsPopup()
    {
        SettingsPopup settingsPopup = _uiService.GetPopup<SettingsPopup>(ConstPopups.SettingsPopup);

        settingsPopup.SoundVolumeChangeEvent += OnChangeSoundVolume;
        settingsPopup.MusicVolumeChangeEvent += OnChangeMusicVolume;

        var userData = _userDataService.GetUserData();

        var isSoundVolume = userData.SettingsData.IsSoundVolume;
        var isMusicVolume = userData.SettingsData.IsMusicVolume;

        settingsPopup.Show(new SettingsPopupData(isSoundVolume, isMusicVolume));
    }

    private void ShowInfoPopup()
    {
        InfoPopup infoPopup = _uiService.GetPopup<InfoPopup>(ConstPopups.InfoPopup);

        infoPopup.Show(default);
    }

    private void OnChangeSoundVolume(bool state)
    {
        _audioService.SetVolume(AudioType.Sound, state ? 1 : 0);
        var userData = _userDataService.GetUserData();
        userData.SettingsData.IsSoundVolume = state;
    }

    private void OnChangeMusicVolume(bool state)
    {
        _audioService.SetVolume(AudioType.Music, state ? 1 : 0);
        var userData = _userDataService.GetUserData();
        userData.SettingsData.IsMusicVolume = state;
    }

}