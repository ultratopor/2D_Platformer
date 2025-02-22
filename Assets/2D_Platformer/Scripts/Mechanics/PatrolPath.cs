using UnityEngine;

namespace Mechanics
{
    public class PatrolPath : MonoBehaviour
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public Mover CreateMover(float speed = 1) => new Mover(this, speed);

        private void Reset()
        {
            StartPosition = Vector3.left;
            EndPosition = Vector3.right;
        }
    }
}