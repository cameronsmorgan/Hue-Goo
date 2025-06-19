using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveCooldown = 0.2f;
    // public string horizontalInput = "Horizontal";
    //public string verticalInput = "Vertical";

    public string controlSchemeName = "Player1";
    public LayerMask collisionLayer;

    private float timer;
    private bool isMoving = false;
    private Vector2Int moveDirection;
    private Vector3 targetPosition;

    private float originalMoveCooldown;
    private bool isBoosted = false;
    public bool canMove = true;

    public Animator SnailController;

    private PlayerControls controls;
    private Vector2 rawInput;

    void Awake()
    {
        controls = new PlayerControls();

        if (controlSchemeName == "Player1")
        {
            controls.Player1.Move.performed += ctx => rawInput = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => rawInput = Vector2.zero;
        }
        else
        {
            controls.Player2.Move.performed += ctx => rawInput = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => rawInput = Vector2.zero;
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        targetPosition = transform.position;
        timer = 0f;
    }

    void Update()
    {
        if (!canMove) return;

        timer += Time.deltaTime;

        if (!isMoving && timer >= moveCooldown)
        {
            int moveX = Mathf.RoundToInt(rawInput.x);
            int moveY = Mathf.RoundToInt(rawInput.y);

            if (moveX != 0)
                moveY = 0;

            moveDirection = new Vector2Int(moveX, moveY);

            if (moveDirection != Vector2Int.zero)
            {
                Vector3 proposedPosition = targetPosition + new Vector3(moveDirection.x * 0.5f, moveDirection.y * 0.5f, 0f);

                if (!IsBlocked(proposedPosition))
                {
                    targetPosition = proposedPosition;
                    isMoving = true;
                    timer = 0f;

                    // Rotate to face the direction of movement
                    RotateTowardsDirection(moveDirection);

                    IsMoving();
                }
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 20f * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;

                IsStill();
            }
        }
    }

    bool IsBlocked(Vector3 target)
    {
        Collider2D hit = Physics2D.OverlapBox(target, new Vector2(0.2f, 0.2f), 0f, collisionLayer);
        return hit != null;
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (!isBoosted)
        {
            originalMoveCooldown = moveCooldown;
            moveCooldown *= multiplier;
            isBoosted = true;
            StartCoroutine(RemoveSpeedBoostAfterDelay(duration));
        }
    }

    private IEnumerator RemoveSpeedBoostAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveCooldown = originalMoveCooldown;
        isBoosted = false;
    }

    //rotate 
    private void RotateTowardsDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.zero) return;

        if (direction == Vector2Int.up)
            transform.rotation = Quaternion.Euler(0, 0, 0);       // Face up
        else if (direction == Vector2Int.down)
            transform.rotation = Quaternion.Euler(0, 0, 180);     // Face down
        else if (direction == Vector2Int.left)
            transform.rotation = Quaternion.Euler(0, 0, 90);      // Face left
        else if (direction == Vector2Int.right)
            transform.rotation = Quaternion.Euler(0, 0, -90);     // Face right
    }

    public void IsMoving()
    {
        SnailController.SetBool("isMoving", true);
    }

    public void IsStill()
    {
        SnailController.SetBool("isMoving", false);
    }
}

