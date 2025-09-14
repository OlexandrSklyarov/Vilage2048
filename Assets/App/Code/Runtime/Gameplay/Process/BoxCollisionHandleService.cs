using System;
using Assets.App.Code.Runtime.Core.Audio;
using Assets.App.Code.Runtime.Core.Initializable;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Data.Audio;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.VFX;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Process
{
    public sealed class BoxCollisionHandleService : IAsyncInitializeProcess, IDisposable
    {
        private readonly AppConfig _appConfig;
        private readonly SignalBus _signalBus;
        private readonly BoxFactory _boxFactory;
        private readonly GameScoreServices _gameScoreServices;        
        private readonly VfxFactory _vfxFactory;
        private readonly IAudioManager _audioManager;

        public BoxCollisionHandleService(
            AppConfig appConfig,
            SignalBus signalBus,
            BoxFactory boxFactory,
            GameScoreServices gameScoreServices,            
            VfxFactory vfxFactory,
            IAudioManager audioManager)
        {
            _appConfig = appConfig;
            _signalBus = signalBus;
            _boxFactory = boxFactory;
            _gameScoreServices = gameScoreServices;
            _vfxFactory = vfxFactory;
            _audioManager = audioManager;
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
            var pos = evt.OtherItem.transform.position;
            var rot = evt.OtherItem.transform.rotation;

            var newBox = _boxFactory.Create(pos, rot, number);

            var force = (evt.OtherItem.transform.position - evt.SelfItem.transform.position).normalized +
                Vector3.up * _appConfig.BoxInfo.CollideBoxForce;

            newBox.Push(force);

            _vfxFactory.Create(GameVfxType.MergeItem, pos, rot);
            _audioManager.PlaySound((int)SfxType.MERGE_HIT);
        }              

        private void UnSubscribe()
        {
            _signalBus.UnSubscribe<Signal.GameEvent.BoxCollision>(OnBoxCollision);
        }       

        public void Dispose() => UnSubscribe();
    }
}

