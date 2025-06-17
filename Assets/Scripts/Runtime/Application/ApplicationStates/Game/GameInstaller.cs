using UnityEngine;
using Zenject;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<EasySlotGameStateController>().AsSingle();
            Container.Bind<MiddleSlotGameStateController>().AsSingle();
            Container.Bind<HardSlotGameStateController>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();
        }
    }
}