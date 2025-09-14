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
            if (IsCanMergeItems(evt))
            {
                var score = evt.SelfItem.Number;
                
                var newNumber = score * 2;

                AddScore(score);

                CreateNewBox(evt, newNumber);

                RemoveOldBoxes(evt);

                CheckWin(newNumber);
            }
        }

        private void AddScore(int score)
        {
            _gameScoreServices.AddScore(score);
        }

        private bool IsCanMergeItems(Signal.GameEvent.BoxCollision evt)
        {
            return evt.OtherItem != null && evt.SelfItem.Number == evt.OtherItem.Number &&
                evt.ImpulseMagnitude >= _appConfig.BoxInfo.MinCollideImpulse;
        }

        private static void RemoveOldBoxes(Signal.GameEvent.BoxCollision evt)
        {
            evt.SelfItem.Reclaim();
            evt.OtherItem.Reclaim();
        }

        private void CreateNewBox(Signal.GameEvent.BoxCollision evt, int number)
        {
            var newBox = _boxFactory.Create
            (
                evt.OtherItem.transform.position,
                evt.OtherItem.transform.rotation,
                number
            );

            var force = (evt.OtherItem.transform.position - evt.SelfItem.transform.position).normalized +
                Vector3.up * _appConfig.BoxInfo.CollideBoxForce;

            newBox.Push(force);
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

