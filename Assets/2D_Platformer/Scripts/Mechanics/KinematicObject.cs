using UnityEngine;

namespace Mechanics
{
    public class KinematicObject : MonoBehaviour
{
    /// <summary>
    /// Минимальная норма (точечное произведение), которая считается подходящей для размещения объекта.
    /// </summary>
    public float minGroundNormalY = .65f;

    /// <summary>
    /// Пользовательский коэффициент тяжести, применяемый к этому объекту.
    /// </summary>
    public float gravityModifier = 1f;

    /// <summary>
    /// Текущая скорость объекта.
    /// </summary>
    public Vector2 velocity;

    /// <summary>
    /// Находится ли объект в данный момент на поверхности?
    /// </summary>
    /// <value></value>
    public bool IsGrounded { get; private set; }

    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected Rigidbody2D body;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;


    /// <summary>
    /// Измените вертикальную скорость объекта.
    /// </summary>
    /// <param name="value"></param>
    public void Bounce(float value)
    {
        velocity.y = value;
    }

    /// <summary>
    /// Изменяйте скорость движения объектов в определенном направлении.
    /// </summary>
    /// <param name="dir"></param>
    public void Bounce(Vector2 dir)
    {
        velocity.y = dir.y;
        velocity.x = dir.x;
    }

    /// <summary>
    /// Телепортируйся в какое-нибудь место.
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        body.position = position;
        velocity *= 0;
        body.linearVelocity *= 0;
    }

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        body.isKinematic = false;
    }

    protected virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        // если вы уже падаете, падайте быстрее, чем при прыжке, в противном случае используйте обычную гравитацию.
        if (velocity.y < 0)
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        else
            velocity += Physics2D.gravity * Time.deltaTime;

        velocity.x = targetVelocity.x;

        IsGrounded = false;

        var deltaPosition = velocity * Time.deltaTime;

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        var move = moveAlongGround * deltaPosition.x;

        PerformMovement(move, false);

        move = Vector2.up * deltaPosition.y;

        PerformMovement(move, true);

    }

    void PerformMovement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            // проверьте, не наткнемся ли мы на что-нибудь в текущем направлении движения
            var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            for (var i = 0; i < count; i++)
            {
                var currentNormal = hitBuffer[i].normal;

                //достаточно ли плоская эта поверхность для приземления?
                if (currentNormal.y > minGroundNormalY)
                {
                    IsGrounded = true;
                    // при движении вверх измените значение groundNormal на новое значение surface normal.
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                if (IsGrounded)
                {
                    // насколько наша скорость совпадает с нормалью к поверхности?
                    var projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                    {
                        // более низкая скорость при движении против нормы (вверх по склону).
                        velocity = velocity - projection * currentNormal;
                    }
                }
                else
                {
                    // Мы в воздухе, но во что-то врезались, поэтому отмените вертикальную и горизонтальную скорость.
                    velocity.x *= 0;
                    velocity.y = Mathf.Min(velocity.y, 0);
                }
                // уберите значение shellDistance из фактического расстояния перемещения.
                var modifiedDistance = hitBuffer[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        body.position = body.position + move.normalized * distance;
    }

}
}