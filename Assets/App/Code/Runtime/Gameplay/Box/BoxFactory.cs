using UnityEngine;
using Assets.App.Code.Runtime.Data.Configs;
using VContainer;
using Assets.App.Code.Runtime.Core.ObjectPool;
using Assets.App.Code.Runtime.Core.Signals;
using System;

namespace Assets.App.Code.Runtime.Gameplay.Box
{
    public sealed class BoxFactory
    {
        private readonly AppConfig _appConfig;
        private readonly SignalBus _signalBus;
        private GeneralPool<BoxView> _boxPool;

        public BoxFactory(AppConfig appConfig, SignalBus signalBus, IObjectResolver resolver)
        {
            _appConfig = appConfig;
            _signalBus = signalBus;
            _boxPool = new GeneralPool<BoxView>("BoxPool", _appConfig.Factory.BoxPrefab, 1, resolver);
        }

        public BoxView Create(Vector3 pos, Quaternion rot, int num)
        {
            var box = _boxPool.Get(pos, rot);
            box.Init(_signalBus, this);
            box.SetColor(GetColor(num));
            box.SetNumber(num);

            _signalBus.Fire(new Signal.GameEvent.CreateBox() { Number = num });
            
            return box;
        }

        private Color GetColor(int num)
        {
            return num switch
            {
                2 => _appConfig.BoxInfo.Box2,
                4 => _appConfig.BoxInfo.Box4,
                8 => _appConfig.BoxInfo.Box8,
                16 => _appConfig.BoxInfo.Box16,
                32 => _appConfig.BoxInfo.Box32,
                64 => _appConfig.BoxInfo.Box64,
                128 => _appConfig.BoxInfo.Box128,
                256 => _appConfig.BoxInfo.Box256,
                512 => _appConfig.BoxInfo.Box512,
                1024 => _appConfig.BoxInfo.Box1024,
                2048 => _appConfig.BoxInfo.Box2048,
                _ => _appConfig.BoxInfo.DefaultBoxColor
            };
        }

        public void Release(BoxView boxView)
        {
            _boxPool.Reclaim(boxView);
            _signalBus.Fire(new Signal.GameEvent.ReleaseBox());
        }
    }
}

