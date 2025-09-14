using Assets.App.Code.Runtime.Core.Signals;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Box
{
    public sealed class BoxView : MonoBehaviour
    {
        public int Number { get; private set; }

        private BoxFactory _boxFactory;
        private Rigidbody _rb;
        private MeshRenderer _renderer;
        private BoxUI[] _numViews;
        private MaterialPropertyBlock _propertyBlock;
        private SignalBus _signalBus;
        private bool _isInteractable;
        private bool _isCanMove;
        private Vector3 _nextPosition;
        
        private readonly int _colorPropertyID = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            
            _renderer = GetComponentInChildren<MeshRenderer>();
            _numViews = GetComponentsInChildren<BoxUI>();
        }
   
        public void Init(SignalBus signalBus, BoxFactory boxFactory)
        {
            _signalBus = signalBus;
            _boxFactory = boxFactory;

            for (int i = 0; i < _numViews.Length; i++)
            {
                _numViews[i].Init();
            }

            _nextPosition = transform.position;

            _isInteractable = false;
            _isCanMove = true;

            _rb.isKinematic = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation |
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionZ;
        }

        public void Push(Vector3 force)
        {
            _isInteractable = true;
            _isCanMove = false;
            _rb.isKinematic = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX;
            _rb.AddForce(force, ForceMode.Impulse);
        }

        public void Move(Vector3 velocity)
        {
            _nextPosition = _rb.position + velocity;
        }

        private void FixedUpdate()
        {
            if (!_isCanMove) return;

            _rb.MovePosition(_nextPosition);
        }

        public void SetNumber(int num)
        {
            Number = num;

            for (int i = 0; i < _numViews.Length; i++)
            {
                _numViews[i].SetNumber(num);
            }
        }

        public void SetColor(Color color)
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            _propertyBlock.SetColor(_colorPropertyID, color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public void Reclaim()
        {
            _isInteractable = false;
            _isCanMove = false;

            if (!_rb.isKinematic)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _rb.isKinematic = true;
            }

            _boxFactory.Release(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isInteractable) return;

            if (collision.gameObject.TryGetComponent<BoxView>(out var other))
            {
                BoxCollidedEvent(this, other, collision.impulse.magnitude);
            }
        }

        private void BoxCollidedEvent(BoxView self, BoxView other, float impulseMagnitude)
        {
            _signalBus.Fire(new Signal.GameEvent.BoxCollision
            {
                SelfItem = self,
                OtherItem = other,
                ImpulseMagnitude = impulseMagnitude
            });
        }
    }
}

