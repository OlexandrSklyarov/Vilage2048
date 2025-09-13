using Assets.App.Code.Runtime.Data.Configs;

namespace Assets.App.Code.Runtime.Gameplay.Map
{
    public sealed class LevelInfoService
    {
        private readonly AppConfig _appConfig;
        private int _currentLevelIndex = 0;

        public LevelInfoService(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public int GetCurrentLevelIndex()
        {
            //get from saves (_currentLevelIndex = GameModelService.GetLocationModel().CurrentLocationIndex)
            return _currentLevelIndex;
        }

        public void UpdateToNextLevelIndex()
        {
            _currentLevelIndex++;

            _currentLevelIndex %= _appConfig.Maps.Length;

            //Set to saves (GameModelService.GetLocationModel().CurrentLocationIndex == _currentLevelIndex)
        }
    }
}

