using AsteroidsGame.Data;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Enemies.Saucer;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.Installers
{
    public class SaucerInstaller : Installer<SaucerInstaller>
    {
        readonly SaucerSize _Size;
        readonly SaucerData _Data;

        public SaucerInstaller(SaucerSize size,
            DiContainer container)
        {
            _Size = size;
            _Data = container.ResolveId<SaucerData>(size);
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_Data).AsSingle();
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<SaucerHealth>().AsSingle();
            Container.BindInterfacesTo<SaucerMovement>().AsSingle();

            if (_Size == SaucerSize.Big)
            {
                Container.BindInterfacesTo<SaucerBigShoot>().AsSingle();
            }
            else
                Container.BindInterfacesTo<SaucerSmallShoot>().AsSingle();
        }
    }
}