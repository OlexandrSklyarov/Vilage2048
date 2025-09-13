using System;
using Assets.App.Code.Runtime.Core.Initializable;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Box;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Process
{
    public sealed class BoxCollisionHandleService : IAsyncInitializeProcess, ICleanup, IDisposable
    {
        private readonly AppConfig _appConfig;
        private readonly SignalBus _signalBus;
        private readonly BoxFactory _boxFactory;
        private readonly GameScoreServices _gameScoreServices;

        public BoxCollisionHandleService(
            AppConfig appConfig,
            SignalBus signalBus,
            BoxFactory boxFactory,
            GameScoreServices gameScoreServices)
        {
            _appConfig = appConfig;
            _signalBus = signalBus;
            _boxFactory = boxFactory;
            _gameScoreServices = gameScoreServices;
        }

        public async UniTask InitializeAsync()
        {
            _signalBus.Subscribe<Signal.GameEvent.BoxCollision>(OnBoxCollision);
            await UniTask.CompletedTask;
        }

        private void OnBoxCollision(Signal.GameEvent.BoxCollision evt)
        {
            var pair = evt.CollidePair;

            if (pair.other != null && pair.self.Number == pair.other.Number &&
                evt.CollidePair.impulseMagnitude >= _appConfig.BoxInfo.MinCollideImpulse)
            {
                var num = pair.self.Number;
                var nextNumber = num * 2;

                _gameScoreServices.AddScore(num);

                var newBox = _boxFactory.Create
                (
                    pair.other.transform.position,
                    pair.other.transform.rotation,
                    nextNumber
                );

                var force = (pair.other.transform.position - pair.self.transform.position).normalized +
                    Vector3.up * _appConfig.BoxInfo.CollideBoxForce;

                newBox.Push(force);

                pair.self.Reclaim();
                pair.other.Reclaim();

                CheckWin(nextNumber);
            }
        }

        private void CheckWin(int nextNumber)
        {
            if (nextNumber >= 2048)
            {
                UnSubscribe();
                _signalBus.Fire(new Signal.Gameplay.Win());
            }
        }

        private void UnSubscribe()
        {
            _signalBus.UnSubscribe<Signal.GameEvent.BoxCollision>(OnBoxCollision);
        }

        public void Cleanup()
        {
            UnSubscribe();
        }

        public void Dispose() => Cleanup();
    }
}

