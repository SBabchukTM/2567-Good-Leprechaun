using Application.Services.Audio;
using Core.Services.Audio;
//using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.UI
{
    [RequireComponent(typeof(Animation), typeof(Button))]
    public class SimpleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Animation _pressAnimation;

        private IAudioService _audioService;
        public Button Button => _button;

        private void Reset()
        {
            _button = GetComponent<Button>();
            _pressAnimation = GetComponent<Animation>();

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(PlayPressAnimation);
            _pressAnimation.playAutomatically = false;

            _pressAnimation.clip = Resources.Load<AnimationClip>("ButtonClickAnim");
            _pressAnimation.AddClip(Resources.Load<AnimationClip>("ButtonClickAnim"), "ButtonClickAnim");
        }

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public void PlayPressAnimation()
        {
            _pressAnimation.Play();
            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }
    }
}