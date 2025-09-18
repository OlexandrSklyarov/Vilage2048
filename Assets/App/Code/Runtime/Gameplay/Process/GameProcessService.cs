using System;
using UnityEngine;
using VContainer.Unity;
using Assets.App.Code.Runtime.Core.Time;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.Map;
using Assets.App.Code.Runtime.Core.Audio;
using Assets.App.Code.Runtime.Data.Audio;
using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Gameplay.Pause;

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
        private readonly IPauseService _pauseService;
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
            IAudioManager audioManager,
            IPauseService pauseService)
        {
            _mapInfo = mapInfo;
            _inputService = inputService;
            _timeService = timeService;
            _appConfig = appConfig;
            _boxFactory = boxFactory;
            _audioManager = audioManager;
            _pauseService = pauseService;

            _pauseService.ChangePauseEvent += OnChangePause;
        }        

        public void Dispose()
        {
            Stop();
            _pauseService.ChangePauseEvent -= OnChangePause;

        }

        private void OnChangePause(bool isPause)
        {
            if (isPause)
            {
                _gameState = State.None;
            }
            else
            { 
                _gameState = State.Wait;
            }
        }       

        public void Run()
        {
            SetState(State.Spawn);
            _nextSpawnTimer = _appConfig.BoxInfo.SpawnBoxDelay;
        }       

        public void Stop() => SetState(State.None);

        private void SetState(State state) => _gameState = state;

        void ITickable.Tick()
        {            
            switch (_gameState)
            {
                case State.Spawn:
                    {
                        _currentBox = CreateBoxOnPushLine();

                        SetState((_currentBox != null) ? State.Wait : State.None);

                        break;
                    }
                case State.Wait:
                    {
                        if (_currentBox == null)
                        {
                            SetState(State.Spawn);
                            return;
                        }

                        if (_inputService.IsPressDown)
                        {
                            SetState(State.Move);
                        }

                        break;
                    }
                case State.Move:
                    {
                        if (_currentBox != null)
                        {
                            if (_inputService.IsPressed)
                            {
                                _currentBox.Move(GetMovementOffset(_currentBox.transform));
                            }
                            else if (_inputService.IsPressUp)
                            {
                                SetState(State.Push);
                            }
                        }

                        break;
                    }
                case State.Push:
                    {
                        _audioManager.PlaySound((int)SfxType.PUSH_ITEM);

                        _currentBox.Push(GetPushForce(_currentBox.transform));

                        _currentBox = null;

                        SetState(State.CheckResult);

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
                            SetState(State.Spawn);
                        }

                        break;
                    }
            }
        }        

        private Vector3 GetPushForce(Transform box)
        {
            return box.transform.forward * _appConfig.BoxInfo.PushForce;
        }

        private Vector3 GetMovementOffset(Transform box)
        {
            var moveSpeed = _appConfig.BoxInfo.MoveSpeed;

        #if UNITY_EDITOR
            moveSpeed *= 1.5f;
        #endif

            var speed = _inputService.IsPressed ? moveSpeed : 0f;
            var dir = _inputService.Data.EndPosition - _prevPosition;
            _prevPosition = _inputService.Data.EndPosition;
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

