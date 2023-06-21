using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [Header("AI Properties")]
    [SerializeField] private BoxCollider[] aiColliders;
    [SerializeField] private BoxCollider aiBase;
    [SerializeField] private BoxCollider aiRoamArea;
    [SerializeField] private Rigidbody rb;

    [Header("AI Values")]
    [SerializeField] private float timeToResetMovement;
    [SerializeField] private float movementSpeed;

    [Header("Misc")]
    private float timer;

    private Vector3 targetDestination;
    private Vector3 movementDirection;
    private Vector3 direction;
    private Vector3 moveDirection;

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //There's AI only when currentPartIndex is 2. (equals to Part 3).
        if (GameManager.Instance.levelStarted && LevelEditor.Instance.currentPartIndex == 2)
        {
            float distanceToTarget = (transform.position - targetDestination).magnitude;
            float distanceToBase = (transform.position - aiBase.transform.position).magnitude;

            timer += Time.fixedDeltaTime;

            if (timer >= timeToResetMovement)
            {
                timer = 0;
                targetDestination = GetRandomPosAtBase(aiBase);
            }
            else if (distanceToTarget <= 2)
            {
                timer = 0;
                targetDestination = GetRandomPosAtBase(aiBase);
            }
            else if (distanceToBase <= 9)
            {
                timer = 0;
                targetDestination = LevelEditor.Instance.GetRandomTransformPoint(aiRoamArea);
            }

            direction = targetDestination - transform.position;
            direction = Vector3.Normalize(direction);

            moveDirection.x = direction.x;
            moveDirection.z = direction.z;
            moveDirection.y = 0;

            // Handle the rotation
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, 360f, 0f) * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation * deltaRotation);

            // Movement
            rb.velocity = direction * movementSpeed * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// When scoped cubes, pick a point within the base area for AI to bring these cubes to the its own base.
    /// </summary>
    public Vector3 GetRandomPosAtBase(Collider collider)
    {
        return new Vector3(Random.Range(collider.bounds.min.x * 0.5f, collider.bounds.max.x * 0.5f), 0.5f, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
    }

    public void DeactivateAI()
    {
        aiBase.transform.parent.gameObject.SetActive(false);
        rb.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ActivateAI()
    {
        aiBase.transform.parent.gameObject.SetActive(true);
        RepositionAI();
        rb.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void RepositionAI()
    {
        transform.position = new Vector3(0f, 0f, 48f);
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    /// <summary>
    /// When a cube is being scobed by a player, these cubes will no longer collide with AI until they are either placed in base or out of player's scope.
    /// </summary>
    public void IsCollisionWithCubeIgnored(BoxCollider collider, bool isIgnored)
    {
        for(int i = 0; i < aiColliders.Length; i++)
        {
            Physics.IgnoreCollision(collider, aiColliders[i], isIgnored);
        }
    }
}
