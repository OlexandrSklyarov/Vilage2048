using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using VContainer.Unity;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Assets.App.Code.Runtime.Core.Input
{
    public sealed class TouchInputService : IInputService, ITickable, IInputCheckApplicationFocus
    {
        InputData IInputService.InputData => _inputData;
        bool IInputService.IsPressed => IsEnabled && Touchscreen.current.primaryTouch.press.isPressed; 
        bool IInputService.IsPressDown => IsEnabled && Touchscreen.current.primaryTouch.press.wasPressedThisFrame; 
        bool IInputService.IsPressUp => IsEnabled && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame; 
        Vector2 IInputService.Direction => Touchscreen.current.primaryTouch.value.delta;
        private Vector2 MousePosition => IsEnabled ? Touchscreen.current.primaryTouch.startPosition.value : Vector2.zero;
        public bool IsPressOnUIElement {get; private set;}
        public bool IsEnabled {get; private set;}
        
        private InputData _inputData;
        private int _touchCount;
        private float _prevMagnitude;

        public event Action<InputData> InputTouchEvent;
        public event Action<float> TouchScrollEvent;

        public TouchInputService()
        {
            _inputData = new InputData();
            EnhancedTouchSupport.Enable();
            
            Enable();

        #if !UNITY_EDITOR    
            BindMouseScroll();
        #endif
        }

        private void BindMouseScroll()
        {
            var scrollAction = new InputAction(binding: "<Mouse>/scroll");
		    scrollAction.Enable();
		    scrollAction.performed += (ctx) =>
            {
                if (!IsEnabled) return;
                TouchScrollEvent(ctx.ReadValue<Vector2>().y);
            };

            var touch0contact = new InputAction
            (
                type: InputActionType.Button,
                binding: "<Touchscreen>/touch0/press"
            );
            touch0contact.Enable();
            var touch1contact = new InputAction
            (
                type: InputActionType.Button,
                binding: "<Touchscreen>/touch1/press"
            );
            touch1contact.Enable();

            touch0contact.performed += _ => _touchCount++;
            touch1contact.performed += _ => _touchCount++;
            touch0contact.canceled += _ => 
            {
                _touchCount--;
                _prevMagnitude = 0;
            };

            touch1contact.canceled += _ => 
            {
                _touchCount--;
                _prevMagnitude = 0;
            };

            var touch0pos = new InputAction
            (
                type: InputActionType.Value,
                binding: "<Touchscreen>/touch0/position"
            );

            touch0pos.Enable();

            var touch1pos = new InputAction
            (
                type: InputActionType.Value,
                binding: "<Touchscreen>/touch1/position"
            );

            touch1pos.Enable();

            touch1pos.performed += _ => 
            {
                if (!IsEnabled) return;
                if(_touchCount < 2) return;

                var magnitude = (touch0pos.ReadValue<Vector2>() - touch1pos.ReadValue<Vector2>()).magnitude;

                if(_prevMagnitude == 0)
                {
                    _prevMagnitude = magnitude;
                }

                var difference = magnitude - _prevMagnitude;
                _prevMagnitude = magnitude;

                TouchScrollEvent?.Invoke(-difference);
            };
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
            ResetInput();
        }        

        private void ResetInput()
        {
            _inputData.Phase = UnityEngine.InputSystem.TouchPhase.Ended;
            _inputData.EndPosition = Vector2.zero;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;
            
            UpdateInputData();        
        }        

        void ITickable.Tick()
        {
            if (!IsEnabled) return;            

            UpdateScroll();
            
            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    OnBegan(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    OnStationary(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    OnMoved(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                {
                    OnEnded(touch);
                }
            }
        }

        private void UpdateScroll()
        {
        #if UNITY_EDITOR
            var value = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(value) > 0f)
            {
                TouchScrollEvent(value);
            }
        #endif
        }
        
        public InputData GetInputDataOnTouch()
        {
            OnBegan(Touch.activeTouches[0]);
            return _inputData;
        }
        
        private void OnBegan(Touch touch)
        {
            if (IsMultitouchInput(touch)) return;

            IsPressOnUIElement = false;

            if (IsSelectUIElement(MousePosition))
            {
                IsPressOnUIElement = true;
                return;
            }

            _inputData.Phase = touch.phase;
            _inputData.StartPosition = touch.startScreenPosition;
            _inputData.EndPosition = _inputData.StartPosition;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;

            UpdateInputData();
        }

        private bool IsSelectUIElement(Vector2 mousePosition)
        {
            //check ui element
            return false;
        }

        private void OnMoved(Touch touch)
        {
            if (IsMultitouchInput(touch)) return;

            _inputData.Phase = touch.phase;
            _inputData.EndPosition = touch.screenPosition;
            
            var result = _inputData.EndPosition - _inputData.StartPosition;
            _inputData.Distance = result.magnitude;
            _inputData.Direction = (result == Vector2.zero) ? Vector2.zero: result / _inputData.Distance;
            
            UpdateInputData();
        }

        private void OnEnded(Touch touch)
        {
            if (IsMultitouchInput(touch)) return;

            _inputData.Phase = touch.phase;
            _inputData.EndPosition = touch.screenPosition;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;
            
            UpdateInputData();
        }

        private void OnStationary(Touch touch)
        {
            if (IsMultitouchInput(touch)) return;

            _inputData.Phase = touch.phase;
            _inputData.EndPosition = touch.screenPosition;
            
            var result = _inputData.EndPosition - _inputData.StartPosition;
            _inputData.Distance = result.magnitude;
            _inputData.Direction = (result == Vector2.zero) ? Vector2.zero: result / _inputData.Distance;
            
            UpdateInputData();
        }

        private bool IsMultitouchInput(Touch touch)
        {
            return touch.finger.index > 0;
        }

        private void UpdateInputData() => InputTouchEvent?.Invoke(_inputData);        

        void IInputCheckApplicationFocus.OnApplicationFocus(bool isFocus)
        {
            if (!IsEnabled) return;

            if (!isFocus)
            {
                ResetInput();
            }
        }
    }
}