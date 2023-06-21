using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{

    public float movementSpeed;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider[] playerColliders;
    private bool clickedToMove;
    private Vector2 initialClickPos;
    private Vector3 direction;
    private Vector2 clickSwipeRange;
    private Vector3 moveDirection;



    void Update()
    {
        ClickInput();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void ClickInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialClickPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            clickedToMove = true;
            direction = Input.mousePosition - (Vector3)initialClickPos;
            direction = Vector3.Normalize(direction);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopPlayer();
        }

        clickSwipeRange = Vector2.zero;

        if (clickedToMove)
        {
            clickSwipeRange = Input.mousePosition - (Vector3)initialClickPos;
        }
    }

    public void StopPlayer()
    {
        clickedToMove = false;
        direction = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        moveDirection = Vector3.zero;
        rb.freezeRotation = true;
    }
    public void RepositionPlayer()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void PlayerMovement()
    {
        if (clickSwipeRange.magnitude > GameManager.Instance.so_gameValues.clickSwipeMovementLimit) // if it is more than minimum limit, then player can move
        {
            moveDirection.x = direction.x;
            moveDirection.z = direction.y;
            moveDirection.y = 0f;

            // Rotation
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, 360f, 0f) * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation * deltaRotation);


            if (clickSwipeRange.magnitude > GameManager.Instance.so_gameValues.clickSwipeSpeedBoostLimit) // if it is more than  speed boost limit, then the player speed will be increased.
            {
                rb.velocity = moveDirection * movementSpeed * GameManager.Instance.so_gameValues.movementBoostMultiplier;
            }
            else // otherwise speed value will be regular movement speed.
            {
                rb.velocity = moveDirection * movementSpeed;
            }
        }
    }

    /// <summary>
    /// When a cube is being scobed by a AI, these cubes will no longer collide with Player until they are either placed in base or out of AI's scope.
    /// </summary>
    public void IsCollisionWithCubeIgnored(BoxCollider collider, bool isIgnored)
    {
        for (int i = 0; i < playerColliders.Length; i++)
        {
            Physics.IgnoreCollision(collider, playerColliders[i], isIgnored);
        }
    }
}
