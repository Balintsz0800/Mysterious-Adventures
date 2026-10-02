 using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stoppingDistance = 0.05f;

    private Rigidbody2D rb;
    private Animation anim;

    private Transform target;
    private bool moving;
    private bool randomMovement;
    private float randomRadius = 2f;
    private Vector2 randomTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (!moving)
        {
            rb.linearVelocity = Vector2.zero;
            anim.isMoving = false;
            return;
        }
        
        if (randomMovement)
        {
            MoveRandomly();
        }
        else
        {
            MoveToTarget();
        }
    } 

    public void  GoTo(Transform destination, bool useRandomMovement = false, float radius = 2f)
    {
        if (destination == null)
            return;
        
        target = destination;
        randomMovement = useRandomMovement;
        randomRadius = radius;
        moving = true;

        if (randomMovement)
        {
            SetRandomTarget();
        }
    }

    private void MoveToTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            anim.isMoving = false;
            moving = false;
            return;
        }

        rb.linearVelocity = direction * speed;

        anim.horizontal = direction.x;
        anim.vertical = direction.y;
        anim.isMoving = true; 
    }

    private void MoveRandomly()
    {
        Vector2 direction = (randomTarget - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, randomTarget);

        if (distance <= stoppingDistance)
        {
            SetRandomTarget();
            return;
        }

        rb.linearVelocity = direction * speed;

        anim.horizontal = direction.x;
        anim.vertical = direction.y;
        anim.isMoving = true;
    }

    private void SetRandomTarget()
    {
        Vector2 center = target.position;
        randomTarget = center + Random.insideUnitCircle * randomRadius;
    }
}