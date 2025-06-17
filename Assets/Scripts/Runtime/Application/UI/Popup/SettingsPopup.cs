using System;
using System.Threading;
using Application.Services.Audio;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _soundVolumeOnButton;
        [SerializeField] private Button _soundVolumeOffButton;
        [SerializeField] private Button _musicVolumeOnButton;
        [SerializeField] private Button _musicVolumeOffButton;

        public event Action<bool> SoundVolumeChangeEvent;
        public event Action<bool> MusicVolumeChangeEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SettingsPopupData settingsPopupData = data as SettingsPopupData;

            var isSoundVolume = settingsPopupData.IsSoundVolume;
            var isMusicVolume = settingsPopupData.IsMusicVolume;

            if (isSoundVolume)
            {
                _soundVolumeOnButton.onClick?.Invoke();
            }
            else
            {
                _soundVolumeOffButton.onClick?.Invoke();
            }

            if (isMusicVolume)
            {
                _musicVolumeOnButton.onClick?.Invoke();
            }
            else
            {
                _musicVolumeOffButton.onClick?.Invoke();
            }

            _soundVolumeOnButton.onClick.AddListener(OnSoundVolumeValueOn);
            _soundVolumeOffButton.onClick.AddListener(OnSoundVolumeValueOff);

            _musicVolumeOnButton.onClick.AddListener(OnMusicVolumeValueOn);
            _musicVolumeOffButton.onClick.AddListener(OnMusicVolumeValueOff);

            _backButton.onClick.AddListener(DestroyPopup);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            return base.Show(data, cancellationToken);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.ClosePopupSound);
            Destroy(gameObject);
        }

        private void OnSoundVolumeValueChanged(bool value)
        {
            SoundVolumeChangeEvent?.Invoke(value);
        }

        private void OnSoundVolumeValueOn()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            OnSoundVolumeValueChanged(true);
        }

        private void OnSoundVolumeValueOff()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            OnSoundVolumeValueChanged(false);
        }

        private void OnMusicVolumeToggleValueChanged(bool value)
        {
            MusicVolumeChangeEvent?.Invoke(value);
        }

        private void OnMusicVolumeValueOn()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            OnMusicVolumeToggleValueChanged(true);
        }

        private void OnMusicVolumeValueOff()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            OnMusicVolumeToggleValueChanged(false);
        }
    }
}