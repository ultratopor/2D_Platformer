using UnityEngine;

namespace Mechanics
{
    public class KinematicObject : MonoBehaviour
{
    /// <summary>
    /// Минимальная норма, которая считается подходящей для размещения объекта.
    /// </summary>
    public float MinGroundNormalY = .65f;

    /// <summary>
    /// Коэффициент тяжести
    /// </summary>
    public float GravityModifier = 1f;

    /// <summary>
    /// Текущая скорость объекта.
    /// </summary>
     public Vector2 Velocity;

    /// <summary>
    /// Находится ли объект в данный момент на поверхности?
    /// </summary>
    /// <value></value>
    protected bool IsGrounded { get; private set; }

    protected Vector2 TargetVelocity;
    private Vector2 _groundNormal;
    private Rigidbody2D _body;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;


    /// <summary>
    /// Изменить вертикальную скорость объекта.
    /// </summary>
    /// <param name="value">Величина</param>
    public void Bounce(float value)
    {
        Velocity.y = value;
    }

    /// <summary>
    /// Изменить скорость движения объектов в определенном направлении.
    /// </summary>
    /// <param name="dir">Направление</param>
    public void Bounce(Vector2 dir)
    {
        Velocity.y = dir.y;
        Velocity.x = dir.x;
    }
    
    public void Teleport(Vector3 position)
    {
        _body.position = position;
        Velocity *= 0;
        _body.linearVelocity *= 0;
    }

    protected virtual void OnEnable()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.bodyType = RigidbodyType2D.Kinematic;
    }

    protected virtual void OnDisable()
    {
        _body.bodyType = RigidbodyType2D.Static;
    }

    protected virtual void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        TargetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        // усиление падения
        if (Velocity.y < 0)
            Velocity += Physics2D.gravity * (GravityModifier * Time.deltaTime);
        else
            Velocity += Physics2D.gravity * Time.deltaTime;

        Velocity.x = TargetVelocity.x;

        IsGrounded = false;

        var deltaPosition = Velocity * Time.deltaTime;

        var moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);

        var move = moveAlongGround * deltaPosition.x;
        // горизонтальный расчёт
        PerformMovement(move, false);

        move = Vector2.up * deltaPosition.y;
        // вертикальный расчёт
        PerformMovement(move, true);

    }

    void PerformMovement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            // кэширование каста коллайдера на расстояние передвижения за кадр
            var count = _body.Cast(move, _contactFilter, _hitBuffer, distance + shellRadius);
            for (var i = 0; i < count; i++)
            {
                var currentNormal = _hitBuffer[i].normal;

                //достаточно ли плоская эта поверхность для приземления?
                if (currentNormal.y > MinGroundNormalY)
                {
                    IsGrounded = true;
                    // при вертикальном расчёте кэшируем нормаль к поверхоности
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                if (IsGrounded)
                {
                    // насколько наша скорость совпадает с нормалью к поверхности?
                    var projection = Vector2.Dot(Velocity, currentNormal);
                    if (projection < 0)
                    {
                        // более низкая скорость при движении против нормы (вверх по склону).
                        Velocity -= projection * currentNormal;
                    }
                }
                else
                {
                    // обнуление скорости, если попался коллайдер во время прыжка
                    Velocity.x *= 0;
                    Velocity.y = Mathf.Min(Velocity.y, 0);
                }
                // уберите значение shellDistance из фактического расстояния перемещения.
                var modifiedDistance = _hitBuffer[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        _body.position += move.normalized * distance;
    }

}
}