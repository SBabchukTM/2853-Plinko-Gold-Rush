using UnityEngine;
using Zenject;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        [SerializeField] private PyramidController _pyramid;
        public override void InstallBindings()
        {
            Container.Bind<AccountStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelSelectionStateController>().AsSingle();
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<SettingsStateController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopStateController>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();

            Container.Bind<PyramidController>().FromComponentInNewPrefab(_pyramid).AsSingle().NonLazy();
        }
    }
}