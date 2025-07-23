using ShopGame.Managers;
using Zenject;

namespace ShopGame.Installers
{
    public class ManagersIntaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var inputStateManager = new InputStateManager();
            Container.Bind<PlayerInputActions>().FromInstance(inputStateManager.InputActions).AsSingle();
            Container.Bind<InputStateManager>().FromInstance(inputStateManager).AsSingle();
        }
    }
}
