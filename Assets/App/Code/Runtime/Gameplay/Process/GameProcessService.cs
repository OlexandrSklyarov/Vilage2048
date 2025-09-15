using System;
using UnityEngine;
using VContainer.Unity;
using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Core.Time;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.Map;
using Assets.App.Code.Runtime.Core.Audio;
using Assets.App.Code.Runtime.Data.Audio;

namespace App.Code.Runtime.Gameplay.Process
{
    public sealed class GameProcessService : ITickable, IDisposable
    {
        private readonly AppConfig _appConfig;
        private readonly IMapInfo _mapInfo;
        private readonly IInputService _inputService;
        private readonly ITimeService _timeService;
        private readonly BoxFactory _boxFactory;
        private readonly IAudioManager _audioManager;
        private State _gameState = State.None;
        private BoxView _currentBox;
        private Camera _camera;
        private Plane _worldPlane;
        private float _nextSpawnTimer;

        public GameProcessService(
            IMapInfo mapInfo,
            IInputService inputService,
            ITimeService timeService,
            AppConfig appConfig,
            BoxFactory boxFactory,
            IAudioManager audioManager)
        {
            _mapInfo = mapInfo;
            _inputService = inputService;
            _timeService = timeService;
            _appConfig = appConfig;
            _boxFactory = boxFactory;
            _audioManager = audioManager;
        }

        public void Run()
        {
            _gameState = State.Spawn;
            _nextSpawnTimer = _appConfig.BoxInfo.SpawnBoxDelay;
            _camera = Camera.main;
            _worldPlane = new Plane(Vector3.up, Vector3.zero);
        }

        public void Stop()
        {
            _gameState = State.None;
        }

        void ITickable.Tick()
        {
            switch (_gameState)
            {
                case State.Spawn:
                {
                    _currentBox = CreateBoxOnPushLine();
                    _gameState = (_currentBox != null) ? State.Wait : State.None;

                    break;
                }

                case State.Wait:
                {
                    if (_inputService.IsPressDown)
                    {
                        _gameState = State.Move;
                    }

                    break;
                }

                case State.Move:
                {
                    if (_currentBox != null)
                    {
                        if (_inputService.IsPressed)
                        {
                            _currentBox.Move(GetNextPosition(_currentBox.transform));
                        }
                        else if (_inputService.IsPressUp)
                        {
                            _gameState = State.Push;
                        }
                    }

                    break;
                }

                case State.Push:
                {
                    _audioManager.PlaySound((int)SfxType.PUSH_ITEM);
                    _currentBox.Push(GetPushForce(_currentBox.transform));
                    _currentBox = null;
                    _gameState = State.CheckResult;

                    break;
                }

                case State.CheckResult:
                {
                    if (IsCanSpawnBox())
                    {
                        _gameState = State.Spawn;
                    }

                    break;
                }
            }
        }

        private bool IsCanSpawnBox()
        {
            return _timeService.IsTimerEnd(ref _nextSpawnTimer, _appConfig.BoxInfo.SpawnBoxDelay) &&
                _gameState == State.CheckResult;
        }

        private Vector3 GetPushForce(Transform box)
        {
            return box.transform.forward * _appConfig.BoxInfo.PushForce;
        }

        private Vector3 GetNextPosition(Transform box)
        {
            var hitPoint = GetPointerPos(box.position, _inputService.InputData.EndPosition);
            hitPoint.x = Mathf.Clamp(hitPoint.x, _mapInfo.FieldBounds.min.x, _mapInfo.FieldBounds.max.x);
            var targetPos = box.transform.position;
            targetPos.x = hitPoint.x;            
       
            return Vector3.MoveTowards(box.position, targetPos, _appConfig.BoxInfo.MoveSpeed * _timeService.DeltaTime);
        }

        private Vector3 GetPointerPos(Vector3 curPos, Vector2 input)
        {
            var ray = _camera.ScreenPointToRay(input);

            if (_worldPlane.Raycast(ray, out var hitDistance))
            {
                curPos = ray.GetPoint(hitDistance);
            }

            return curPos;
        }       

        private BoxView CreateBoxOnPushLine()
        {
            var point = _mapInfo.BoxSpawnPoint;
            var num = (UnityEngine.Random.value < _appConfig.BoxInfo.SpawnBigNumChance) ? 2 : 4;
            var box = _boxFactory.Create(point.position, point.rotation, num);

            return box;
        }

        public void Dispose()
        {
            Stop();
        }

        private enum State
        {
            None,
            Spawn,
            Wait,
            Move,
            Push,
            CheckResult
        }
    }
}
