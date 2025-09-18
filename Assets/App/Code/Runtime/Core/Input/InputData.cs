using UnityEngine;

namespace Assets.App.Code.Runtime.Core.Input
{
    public struct InputData
    {
        public TouchPhase Phase;
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public Vector2 Direction;
        public float Distance;
    }

}