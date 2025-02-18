using UnityEngine;

namespace Mechanics
{
    public class PatrolPath : MonoBehaviour
    {
        /// <summary>
        /// Один конец пути патрулирования.
        /// </summary>
        public Vector2 startPosition, endPosition;

        /// <summary>
        /// Создайте экземпляр Mover, который используется для перемещения объекта по пути с определённой скоростью.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public Mover CreateMover(float speed = 1) => new Mover(this, speed);

        void Reset()
        {
            startPosition = Vector3.left;
            endPosition = Vector3.right;
        }
    }
}