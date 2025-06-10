using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveCooldown = 0.2f; 
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";
    public LayerMask collisionLayer; 

    private float timer;
    private bool isMoving = false;
    private Vector2Int moveDirection;
    private Vector3 targetPosition;

    private float originalMoveCooldown;
    private bool isBoosted = false;


    void Start()
    {
        targetPosition = transform.position;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isMoving && timer >= moveCooldown)
        {
            int moveX = (int)Input.GetAxisRaw(horizontalInput);
            int moveY = (int)Input.GetAxisRaw(verticalInput);

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
            }
        }
    }

    bool IsBlocked(Vector3 target)
    {
        Collider2D hit = Physics2D.OverlapBox(target, new Vector2(0.4f, 0.4f), 0f, collisionLayer);
        return hit != null;
    }










    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (!isBoosted)
        {
            originalMoveCooldown = moveCooldown;
            moveCooldown *= multiplier;
            isBoosted= true;
            StartCoroutine(RemoveSpeedBoostAfterDelay(duration));
        }
    }

    private IEnumerator RemoveSpeedBoostAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveCooldown = originalMoveCooldown;
        isBoosted= false;
    }
}
