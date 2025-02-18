using UnityEngine;

namespace Mechanics
{
    public class Mover
    {
        PatrolPath path;
        float p = 0;
        float duration;
        float startTime;

        public Mover(PatrolPath path, float speed)
        {
            this.path = path;
            this.duration = (path.endPosition - path.startPosition).magnitude / speed;
            this.startTime = Time.time;
        }

        /// <summary>
        /// Получите положение движка для текущего кадра.
        /// </summary>
        /// <value></value>
        public Vector2 Position
        {
            get
            {
                p = Mathf.InverseLerp(0, duration, Mathf.PingPong(Time.time - startTime, duration));
                return path.transform.TransformPoint(Vector2.Lerp(path.startPosition, path.endPosition, p));
            }
        }
    }
}