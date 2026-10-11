using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;
    
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float staminaRegenSpeed = 5;
    private float lastHorizontalVector;
    private float lastVerticalVector;
    public Vector3 movementVector;
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
        
        movementVector = movementVector.normalized;

        if (movementVector.x != 0 || movementVector.y != 0)
        {
            anim.horizontal =  movementVector.x;
            anim.vertical = movementVector.y;
            
            lastHorizontalVector = movementVector.x;
            lastVerticalVector = movementVector.y;
        }
        
        anim.isMoving = movementVector.x != 0 || movementVector.y != 0;
        
        float currentSpeed = speed;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (anim.isMoving && Player.Instance.currentStamina > 0)
            {
                currentSpeed = runSpeed;
                Player.Instance.currentStamina--;
            }
        }
        else if (Player.Instance.currentStamina < Player.Instance.maxStamina)
        {
           Player.Instance.currentStamina += staminaRegenSpeed * Time.deltaTime;
        }
        
        rb.linearVelocity = (movementVector * currentSpeed).normalized;
    }

    void LateUpdate()
    {
        cam.transform.position = player.position + offset;
    }
}