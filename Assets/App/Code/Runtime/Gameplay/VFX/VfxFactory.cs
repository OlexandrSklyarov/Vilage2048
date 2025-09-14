using System;
using System.Collections.Generic;
using System.Linq;
using Assets.App.Code.Runtime.Core.ObjectPool;
using Assets.App.Code.Runtime.Data.Configs;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.VFX
{
    public sealed class VfxFactory : IDisposable
    {
        private readonly AppConfig _appConfig;
        private readonly Dictionary<GameVfxType, GeneralPool<VfxView>> _pools = new();

        public VfxFactory(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public IVfxItem CreateWithoutPlay(GameVfxType type, Vector3 pos, Quaternion rot)
        {
            if (!_pools.TryGetValue(type, out var pool))
            {
                _pools.Add(type, CreateNewPool(type));
            }

            var vfx = _pools[type].Get(pos, rot);
            vfx.Init(this);
            return vfx;
        }

        public void Dispose()
        {
            foreach (var item in _pools)
            {
                item.Value.Clear();
            }

            _pools.Clear();
        }

        private GeneralPool<VfxView> CreateNewPool(GameVfxType type)
        {
            return new GeneralPool<VfxView>
            (
                $"[VFX_POOL - {type}]",
                _appConfig.Factory.Vfx.First(x => x.Type == type).Prefab,
                1
            );
        }

        public IVfxItem Create(GameVfxType type, Vector3 pos, Quaternion rot)
        {
            var vfx = CreateWithoutPlay(type, pos, rot);
            vfx.Play();
            return vfx;
        }

        public void Release(VfxView item)
        {
            if (!_pools.TryGetValue(item.Type, out var pool))
            {
                Util.DebugLog.PrintError($"Vfx with type {item.Type} not found");
                UnityEngine.Object.Destroy(item.gameObject);
                return;
            }

            pool.Reclaim(item);            
        }
    }
}

