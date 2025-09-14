using System.Collections;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.VFX
{
    public sealed class VfxView : MonoBehaviour, IVfxItem
    {
        public bool IsAlive { get; private set; }
        [field: SerializeField] public GameVfxType Type { get; private set; }
        [field: SerializeField] public ParticleSystem Particles { get; private set; }
        [field: SerializeField, Min(0.01f)] public float Duration { get; private set; } = 1f;
        
        private VfxFactory _vfxFactory;
        private Coroutine _routine;    

        public void Init(VfxFactory vfxFactory)
        {
            _vfxFactory = vfxFactory;
        }

        void IVfxItem.Play()
        {
            if (_routine != null) StopCoroutine(_routine);

            _routine = StartCoroutine(Release(Duration));

            IsAlive = true;

            Particles.Play();

            IEnumerator Release(float time)
            {
                yield return new WaitForSeconds(time);

                _vfxFactory.Release(this);

                IsAlive = false;
            }
        }
    }
}

