using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;
    private static PlayerMovement instance;

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
    private Vector3 offset = new Vector3(0, 0, -10);

    public Vector2 FacingDir
    {
        get
        {
            return new Vector2(lastHorizontalVector, lastVerticalVector).normalized;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        float movex = Input.GetAxisRaw("Horizontal");
        float movey = Input.GetAxisRaw("Vertical");

        Vector2 inputDir = new Vector2(movex, movey).normalized;

        if (inputDir.x != 0 || inputDir.y != 0)
        {
            anim.horizontal = inputDir.x;
            anim.vertical = inputDir.y;

            lastHorizontalVector = inputDir.x;
            lastVerticalVector = inputDir.y;
        }

        anim.isMoving = inputDir.x != 0 || inputDir.y != 0;

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

        rb.linearVelocity = inputDir * currentSpeed;
    }

    void LateUpdate()
    {
        if (cam != null && player != null)
            cam.transform.position = player.position + offset;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Prevent an old player from suppressing the fresh player created when a
        // new game is launched from StartScene.
        if (scene.name == "StartScene")
        {
            Destroy(gameObject);
            return;
        }

        if (cam == null)
            cam = Camera.main;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (instance == this)
            instance = null;
    }
}
