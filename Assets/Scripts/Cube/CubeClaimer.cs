using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClaimer : MonoBehaviour
{
    public int cubeClaimerIndex; //0 player's claimer index, 1 AI's claimer index


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (GameManager.Instance.levelStarted)
            {
                CubeManager cube = other.GetComponentInParent<CubeManager>();
                if (cube != null)
                {
                    if (cube.takenByWhichSide == -1) // if it is not taken by anyone, then it can be taken by player.
                    {
                        if (cubeClaimerIndex == 0) //player
                        {
                            cube.takenByWhichSide = 0;
                            cube.canBeTaken = false;
                            GameManager.Instance.aiOpponent.IsCollisionWithCubeIgnored(cube.boxCollider, true);
                        }
                        if (cubeClaimerIndex == 1) // ai
                        {
                            cube.takenByWhichSide = 1;
                            cube.canBeTaken = false;
                            GameManager.Instance.player.IsCollisionWithCubeIgnored(cube.boxCollider, true);
                        }
                    }
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (GameManager.Instance.levelStarted)
            {
                CubeManager cube = other.GetComponentInParent<CubeManager>();
                if (cube != null)
                {
                    cube.canBeTaken = true;
                    cube.takenByWhichSide = -1;
                    if (cubeClaimerIndex == 0) // player
                    {
                        GameManager.Instance.aiOpponent.IsCollisionWithCubeIgnored(cube.boxCollider, false);
                    }
                    else if (cubeClaimerIndex == 1) // ai
                    {
                        GameManager.Instance.player.IsCollisionWithCubeIgnored(cube.boxCollider, false);
                    }
                }
            }
        }
    }
}
