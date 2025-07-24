using ShopGame.Managers;
using Zenject;

namespace ShopGame.Installers
{
    public class SceneManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIStateManager>().FromInstance(new UIStateManager()).AsSingle();
        }
    }
}
