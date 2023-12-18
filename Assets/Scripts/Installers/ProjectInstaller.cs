using AsteroidsGame.Input;

using Zenject;

namespace AsteroidsGame.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputManager>().AsSingle().NonLazy();
        }
    }
}