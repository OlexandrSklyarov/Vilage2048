using System;
using Player.Input;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.App.Code.Runtime.Core.Input
{
    public sealed class TouchInputService : IInputService, IDisposable
    {        
        public InputData Data => _inputData;
        public Vector2 PointerPosition => IsGameplaySchema() ? Data.EndPosition : Vector2.zero;
        public bool IsPressDown => IsGameplaySchema() && _control.Player.TouchContact.WasPressedThisFrame();
        public bool IsPressed => IsGameplaySchema() && _isTouching;
        public bool IsPressUp => IsGameplaySchema() && _control.Player.TouchContact.WasReleasedThisFrame();
       
        private readonly TouchControl _control;
        private InputData _inputData;
        private bool _isTouching;

        public event Action<InputData> InputUpdateEvent;
        public event Action<Vector2> SwipeEvent;

        public TouchInputService()
        {
            _inputData = new InputData();
            _control = new TouchControl();

            EnableGameplay();
        }

        public void Dispose()
        {
            _control.Player.Disable();
            _control.UI.Disable();
        }

        public void EnableGameplay()
        {
            _control.Player.Enable();
            _control.UI.Disable();

            _control.Player.TouchContact.started += OnBegan;
            _control.Player.TouchPosition.performed += OnMoved;
            _control.Player.TouchContact.canceled += OnEnded;
        }

        public void EnableUI()
        {
            _control.Player.TouchContact.started -= OnBegan;
            _control.Player.TouchPosition.performed -= OnMoved;
            _control.Player.TouchContact.canceled -= OnEnded;

            _control.Player.Disable();
            _control.UI.Enable();

            ResetInput();
        }      
       
        private bool IsGameplaySchema() => _control != null && _control.Player.enabled;
               
        private void OnBegan(CallbackContext context)
        {
            if (!IsGameplaySchema()) return;

            _isTouching = true;          

            _inputData.Phase = TouchPhase.Began;
            _inputData.StartPosition = _control.Player.TouchPosition.ReadValue<Vector2>();
            _inputData.EndPosition = _inputData.StartPosition;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;

            UpdateInputData();
        }       

        private void OnMoved(CallbackContext context)
        {
            if (!IsGameplaySchema()) return;

            _inputData.Phase = TouchPhase.Move;
            _inputData.EndPosition = context.ReadValue<Vector2>();
            
            var result = _inputData.EndPosition - _inputData.StartPosition;
            _inputData.Distance = result.magnitude;
            _inputData.Direction = (result == Vector2.zero) ? Vector2.zero: result / _inputData.Distance;
            
            UpdateInputData();
        }

        private void OnEnded(CallbackContext context)
        {
            if (!IsGameplaySchema()) return;

            if (!_isTouching) return;

            _inputData.Phase = TouchPhase.Ended;
            _inputData.EndPosition = _control.Player.TouchPosition.ReadValue<Vector2>();
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;

            _isTouching = false;

            UpdateInputData();
            Swipe();
        }  

        private void ResetInput()
        {
            _inputData.Phase = TouchPhase.Ended;
            _inputData.EndPosition = Vector2.zero;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;

            UpdateInputData();
        }

        private void UpdateInputData() => InputUpdateEvent?.Invoke(_inputData);

        private void Swipe() => SwipeEvent?.Invoke(_inputData.EndPosition - _inputData.StartPosition);            
    }
}