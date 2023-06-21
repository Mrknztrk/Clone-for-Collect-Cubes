using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] ObstacleManager obstacleManager;
    private float minBoundaryValue = 0.5f;
    private float maxBoundaryValue = 0.5f;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetObstacleActive(false);
            obstacleManager.ResetObstacleSettings();
            GameManager.Instance.CreateParticleEffect(obstacleManager.explosionParticle, GameManager.Instance.player.transform.position);
            GameManager.Instance.player.StopPlayer();
            GameManager.Instance.player.RepositionPlayer();
        }
        else if (collision.gameObject.CompareTag("AI"))
        {
            SetObstacleActive(false);
            obstacleManager.ResetObstacleSettings();
            GameManager.Instance.CreateParticleEffect(obstacleManager.explosionParticle, GameManager.Instance.aiOpponent.transform.position);
            GameManager.Instance.aiOpponent.RepositionAI();
        }
    }
    
    public void SetObstacleActive(bool isActive)
    {
        if (isActive)
        {
            transform.position = GetRandomTransformPoint(obstacleManager.obstacleSpawnArea);
        }
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// Spawns obstacle randomly within the bounds of the spawn area.
    /// </summary>
    public Vector3 GetRandomTransformPoint(Collider collider)
    {
        return new Vector3(Random.Range(collider.bounds.min.x * minBoundaryValue, collider.bounds.max.x * maxBoundaryValue), 16f, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
    }
}
