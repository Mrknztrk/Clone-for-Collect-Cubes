using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public BoxCollider boxCollider;
    public MeshRenderer meshRenderer;

    [SerializeField] private Rigidbody rb;

    public bool canBeTaken = true; // true is default. if it is within the collector, it can't be taken.
    [HideInInspector] public int takenByWhichSide = -1; // -1 is default, indicating it has not been taken yet. 0 taken by player, 1 taken by AI. Whenever this value changes to some number, it will prevent getting taken once again.
    [HideInInspector] public int placedInWhichBase = -1; // which of the bases the cube ended up.  0 player base, 1 ai base.

    private bool placedInBase; // when the cube is in either the Player base or AI base.
    private float magnetEffectSeconds = 0f; // if placedInBase, magnetEFfectSEconds will be 1.
    private bool frozenCube; //a boolean created to stop prevent repeating the same process within update.



    private void FixedUpdate()
    {
        MagnetEffectOnCollect();
    }


    public void MagnetEffectOnCollect()
    {
        if (magnetEffectSeconds > 0)
        {
            if (placedInBase)
            {
                if (takenByWhichSide == 0) // by player.
                {
                    magnetEffectSeconds -= Time.deltaTime;
                    rb.AddForce((LevelEditor.Instance.cubeBases[placedInWhichBase].transform.position - rb.position) * GameManager.Instance.so_gameValues.magnetForceValue * Time.fixedDeltaTime);
                }
            }
        }
        else if (magnetEffectSeconds <= 0 && placedInBase)
        {
            if (!frozenCube)
            {
                frozenCube = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }



    /// <summary>
    /// Arranging all the variables to get the cube properly spawn in the middle area of the spawn point.
    /// </summary>
    public void CubeRandomSpawn()
    {
        magnetEffectSeconds = 0;
        takenByWhichSide = -1;
        placedInWhichBase = -1;
        placedInBase = false;
        frozenCube = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(LevelEditor.Instance.middlePos.x, 0, LevelEditor.Instance.middlePos.z);
        boxCollider.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
        meshRenderer.material = GameManager.Instance.so_gameValues.cubeColors[0];
        IgnoreCollisions(false);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// This will be done before the each level starts.
    /// </summary>
    public void ResetCubeValues()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        boxCollider.transform.localPosition = new Vector3(0, 0, 0);

        transform.position = new Vector3(0, 0.5f, 0);
        magnetEffectSeconds = 0;
        takenByWhichSide = -1;
        placedInWhichBase = -1;
        placedInBase = false;
        frozenCube = false;
        IgnoreCollisions(false);
        GameManager.Instance.player.IsCollisionWithCubeIgnored(boxCollider, false);
        GameManager.Instance.aiOpponent.IsCollisionWithCubeIgnored(boxCollider, false);
    }

    public void TakenCube()
    {
        if (!placedInBase)
        {
            magnetEffectSeconds = 0.6f;

            IgnoreCollisions(true);

            placedInBase = true; // it is now in base, this will trigger a magnet effect from the CubeBase for a few sec.

            //which base is this cube in? color will change depending on that.
            if (placedInWhichBase == 0)
            {
                meshRenderer.material = GameManager.Instance.so_gameValues.playerColor;
            }
            else if (placedInWhichBase == 1)
            {
                meshRenderer.material = GameManager.Instance.so_gameValues.aiColor;
            }
        }

    }

    /// <summary>
    /// If ignored, this will ensure that it will not collide with already collected cubes and the player/AI. Only the wall.
    /// </summary>
    public void IgnoreCollisions(bool isIgnored)
    {
        if (isIgnored)
        {
            boxCollider.gameObject.layer = 10;
        }
        else
        {
            boxCollider.gameObject.layer = 6;
        }
    }
}
