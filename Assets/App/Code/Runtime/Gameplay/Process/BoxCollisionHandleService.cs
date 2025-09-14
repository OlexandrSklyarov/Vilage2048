using System;
using Assets.App.Code.Runtime.Core.Initializable;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.Map;
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
        private readonly LevelInfoService _levelInfoService;
        private readonly VfxFactory _vfxFactory;

        public BoxCollisionHandleService(
            AppConfig appConfig,
            SignalBus signalBus,
            BoxFactory boxFactory,
            GameScoreServices gameScoreServices,
            LevelInfoService levelInfoService,
            VfxFactory vfxFactory)
        {
            _appConfig = appConfig;
            _signalBus = signalBus;
            _boxFactory = boxFactory;
            _gameScoreServices = gameScoreServices;
            _levelInfoService = levelInfoService;
            _vfxFactory = vfxFactory;
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
            var pos = evt.OtherItem.transform.position;
            var rot = evt.OtherItem.transform.rotation;

            var newBox = _boxFactory.Create(pos, rot, number);

            var force = (evt.OtherItem.transform.position - evt.SelfItem.transform.position).normalized +
                Vector3.up * _appConfig.BoxInfo.CollideBoxForce;

            newBox.Push(force);

            _vfxFactory.Create(GameVfxType.MergeItem, pos, rot);
        }

        private void CheckWin(int nextNumber)
        {
            //Check loss logic...            

            //check win
            if (nextNumber >= GetMaxNumber())
            {
                UnSubscribe();
                _signalBus.Fire(new Signal.Gameplay.Win());
            }
        }

        private int GetMaxNumber()
        {
            var indexLvl = _levelInfoService.GetCurrentLevelIndex();
            return _appConfig.Maps[indexLvl].MaxNumberToWin;
        }

        private void UnSubscribe()
        {
            _signalBus.UnSubscribe<Signal.GameEvent.BoxCollision>(OnBoxCollision);
        }       

        public void Dispose() => UnSubscribe();
    }
}

