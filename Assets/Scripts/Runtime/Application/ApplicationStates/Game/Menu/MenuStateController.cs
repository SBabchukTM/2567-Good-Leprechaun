using Core.StateMachine;
using Application.UI;
using Application.Services.UserData;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Core;
using System;
using System.Threading;

namespace Application.Game
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly ISettingProvider _settingProvider;
        private readonly ILogger _logger;

        private MenuScreen _menuScreen;

        private event Action GoToLevelEvent;

        public MenuStateController(ILogger logger,
            IUiService uiService,
            UserDataService userDataService,
            IAudioService audioService,
            ISettingProvider settingProvider
            ) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
            _settingProvider = settingProvider;
            _logger = logger;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            _menuScreen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _menuScreen.OptionsButtonPressEvent += ShowOptionsPopup;
            _menuScreen.PlayButtonPressEvent += GoLevel;
            _menuScreen.Initialize();
            _menuScreen.ShowAsync().Forget();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void ShowOptionsPopup()
        {
            OptionsPopup optionsPopup = _uiService.GetPopup<OptionsPopup>(ConstPopups.OptionsPopup);

            optionsPopup.SettingsButtonPressEvent += ShowSettingsPopup;
            optionsPopup.InfoButtonPressEvent += ShowInfoPopup;
            optionsPopup.ShopButtonPressEvent += ShowShopPopup;

            optionsPopup.Show(new OptionsPopupData(false));
        }

        private void ShowShopPopup()
        {
            var shopPopup = _uiService.GetPopup<ShopPopup>(ConstPopups.ShopPopup);
            shopPopup.Show(default);
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

        private void GoLevel(int level_id)
        {
            switch (level_id)
            {
                case 0:
                    GoTo<EasySlotGameStateController>();
                    break;
                case 1:
                    GoTo<MiddleSlotGameStateController>();
                    break;
                case 2:
                    GoTo<HardSlotGameStateController>();
                    break;
                default:
                    break;
            }

        }
    }
}