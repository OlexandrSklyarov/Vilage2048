using Assets.App.Code.Runtime.Data.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Map
{
    public sealed class BuildGameLocationService : IMapInfo
    {
        public Transform BoxSpawnPoint => _map.BoxSpawnPoint;
        public Bounds FieldBounds => _map.FieldBounds;

        private readonly AppConfig _appConfig;
        private readonly LevelInfoService _levelInfoService;
        private MapViewProvider _map;

        public BuildGameLocationService(AppConfig appConfig, LevelInfoService levelInfoService)
        {
            _appConfig = appConfig;
            _levelInfoService = levelInfoService;
        }


        public async UniTask CreateLocationAsync()
        {
            var mapPrefab = _appConfig.Maps[_levelInfoService.GetCurrentLevelIndex()].MapPrefab;
            
            _map = Object.Instantiate(mapPrefab);

            await UniTask.CompletedTask;
        }
    }
}

