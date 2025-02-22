using UnityEngine;

namespace Mechanics
{
    /// <summary>
    /// Нашёптывает доисторическому врагу, чтобы он бездумно челночил между двумя точками.
    /// </summary>
    public class Mover
    {
        private readonly PatrolPath _path;
        private float _position = 0;
        private readonly float _duration;
        private readonly float _startTime;

        public Vector2 Position
        {
            get
            {
                _position = Mathf.InverseLerp(0, _duration, Mathf.PingPong(Time.time - _startTime, _duration));
                return _path.transform.TransformPoint(Vector2.Lerp(_path.StartPosition, _path.EndPosition, _position));
            }
        }
        
        public Mover(PatrolPath path, float speed)
        {
            this._path = path;
            this._duration = (path.EndPosition - path.StartPosition).magnitude / speed;
            this._startTime = Time.time;
        }

    }
}