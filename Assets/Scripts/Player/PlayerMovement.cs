using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;
    
    [Header("Movement")]
    [SerializeField] private float speed;
    private float lastHorizontalVector;
    private float lastVerticalVector;
    private Vector3 movementVector;
    Rigidbody2D rb;
    
    Animation anim;
    
    [Header("Camera")]
    public Camera cam;
    private Transform player;
    private Vector3 offset =  new Vector3(0, 0, -10);

    public Vector2 FacingDir
    {
        get
        {
            return new Vector2(lastHorizontalVector, lastVerticalVector).normalized;
        }
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
        anim = GetComponent<Animation>();
    }
    
    void Update()
    {
        movementVector.x = Input.GetAxis("Horizontal");
        movementVector.y = Input.GetAxis("Vertical");

        if (movementVector.x != 0)
        {
            lastHorizontalVector = movementVector.x;
        }

        if (movementVector.y != 0)
        {
            lastVerticalVector = movementVector.y;
        }
<<<<<<< HEAD:Assets/Scripts/Player/PlayerMovement.cs
=======

<<<<<<< Updated upstream:Assets/Scripts/PlayerMovement.cs
        if (Input.GetKey(KeyCode.LeftShift) && _player.currentStamina != 0)
        {
            _player.currentStamina --;
        }
=======
        if (movementVector.x != 0 || movementVector.y != 0)
        {
            anim.horizontal = movementVector.x;
            anim.vertical = movementVector.y;
        }
        
        anim.isMoving = movementVector.x != 0 || movementVector.y != 0;
>>>>>>> Stashed changes:Assets/Scripts/Player/PlayerMovement.cs
>>>>>>> npc:Assets/Scripts/PlayerMovement.cs
        
        movementVector *= speed;
        rb.linearVelocity = movementVector;
        anim.horizontal = movementVector.x;
        anim.vertical = movementVector.y;
    }

    void LateUpdate()
    {
        cam.transform.position = player.position + offset;
    }
}