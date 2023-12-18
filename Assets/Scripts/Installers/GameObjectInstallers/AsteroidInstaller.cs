using AsteroidsGame.Data;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Enemies.Asteroid;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.Installers
{
    public class AsteroidInstaller : Installer<AsteroidInstaller>
    {
        readonly AsteroidData _Data;

        public AsteroidInstaller(AsteroidSize size,
            DiContainer container)
        {
            _Data = container.ResolveId<AsteroidData>(size);
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_Data).AsSingle();
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AsteroidHealth>().AsSingle();
            Container.BindInterfacesTo<AsteroidMovement>().AsSingle();
        }
    }
}