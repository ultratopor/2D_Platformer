using UnityEngine;

namespace View
{
    public class Parallax : MonoBehaviour
    {
        [Tooltip("Вставлять слои, начиная с самого дальнего")]
        [SerializeField] private GameObject[] _layers;
        [SerializeField] private Camera _camera;
        
        [Tooltip("Сила параллакса. Должна быть чуть меньше коэффициента уменьшения")]
        [SerializeField] private float _parallaxEffect;
        
        [Tooltip("Коэффициент уменьшения эффекта параллакса от слоя к слою")]
        [SerializeField] private float _divideCoef = 4f;
        
        private float[] _startPosition;
        private float[] _lenght;
        private float _parallaxCoef, _cameraPos, _cameraCoef;
        
        private void Start()
        {
            _lenght = new float[_layers.Length];
            _startPosition = new float[_layers.Length];
            
            for (int i = 0; i < _layers.Length; i++)
            {
                _startPosition[i] = _layers[i].transform.position.x;
                _lenght[i] = _layers[i].GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }

        private void FixedUpdate()
        {
            _cameraPos = _camera.transform.position.x;
            for (int i = 0; i < _layers.Length; i++)
            {
                _cameraCoef = _cameraPos * _parallaxEffect/(_divideCoef * (i+1));
                _parallaxCoef = _cameraPos * (1 - _parallaxEffect/(_divideCoef * (i+1)));
                _layers[i].transform.position = new Vector3(_startPosition[i] + _cameraCoef, 
                                                            _layers[i].transform.position.y,
                                                            _layers[i].transform.position.z);
                
                if (_parallaxCoef > _startPosition[i] + _lenght[i]/2)
                {
                    _startPosition[i] += _lenght[i];
                }
                else if (_parallaxCoef < _startPosition[i] - _lenght[i]/2)
                {
                    _startPosition[i] -= _lenght[i];
                }
            }
        }
    }
}