using System;
using UnityEngine;
using VContainer.Unity;
using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.Time;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.Map;

namespace App.Code.Runtime.Gameplay.Process
{
    public sealed class GameProcessService : ITickable, IDisposable
    {
        private readonly AppConfig _appConfig;
        private readonly IMapInfo _mapInfo;
        private readonly IInputService _inputService;
        private readonly ITimeService _timeService;
        private readonly BoxFactory _boxFactory;
        private BoxView _currentBox;
        private State _gameState = State.None;
        private Vector2 _prevPosition;
        private float _nextSpawnTimer;

        public GameProcessService(
            IMapInfo mapInfo,
            IInputService inputService,
            ITimeService timeService,
            AppConfig appConfig,
            BoxFactory boxFactory,
            SignalBus signalBus)
        {
            _mapInfo = mapInfo;
            _inputService = inputService;
            _timeService = timeService;
            _appConfig = appConfig;
            _boxFactory = boxFactory;
        }

        public void Run()
        {
            _gameState = State.Spawn;
            _nextSpawnTimer = _appConfig.BoxInfo.SpawnBoxDelay;
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
                                _currentBox.Move(GetMoveVelocity(_currentBox.transform));
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
                        _currentBox.Push(GetPushForce(_currentBox.transform));

                        _currentBox = null;

                        _gameState = State.CheckResult;

                        break;
                    }
                case State.CheckResult:
                    {
                        if (!_timeService.IsTimerEnd(ref _nextSpawnTimer, _appConfig.BoxInfo.SpawnBoxDelay))
                        {
                            return;
                        }
                        
                        if (_gameState == State.CheckResult)
                        {
                            _gameState = State.Spawn;
                        }

                        break;
                    }
            }
        }        

        private Vector3 GetPushForce(Transform box)
        {
            return box.transform.forward * _appConfig.BoxInfo.PushForce;
        }

        private Vector3 GetMoveVelocity(Transform box)
        {
            var speed = _inputService.InputData.Phase == UnityEngine.InputSystem.TouchPhase.Moved
                ? _appConfig.BoxInfo.MoveSpeed
                : 0f;

            var dir = _inputService.InputData.EndPosition - _prevPosition;
            _prevPosition = _inputService.InputData.EndPosition;

            var velocity = box.right * dir.normalized.x * speed * _timeService.DeltaTime;

            return ClampVelocity(box.position, velocity);
        }

        private Vector3 ClampVelocity(Vector3 pos, Vector3 velocity)
        {
            var bounds = _mapInfo.FieldBounds;

            if (pos.x + velocity.x < bounds.min.x)
            {
                velocity.x = bounds.min.x - pos.x;
            }
            else if (pos.x + velocity.x > bounds.max.x)
            {
                velocity.x = bounds.max.x - pos.x;
            }

            return velocity;
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

