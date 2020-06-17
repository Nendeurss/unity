using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LIM_TRAN_HOUACINE_NGUYEN;
using SDD.Events;

public class GroundObject : SimpleGameStateObserver
{
    // Start is called before the first frame update
    [SerializeField] Transform chunkPos;
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] GameObject Obstacle;

    void Start()
    {
        Debug.Log("Ground debug "+chunkPos.position);
        Debug.Log("Ground debug "+chunkPos.position);
        setObstaclePosition(Obstacle);
        GameObject obs = Instantiate(Obstacle, Obstacle.transform.position, Quaternion.identity);
        obs.transform.SetParent(chunkPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /***
     * Permet de définir aléatoirement la position d'un obstacle dans son chunk respectif
     */
    private void setObstaclePosition(GameObject Obstacle)
    {
        Vector3 chunkPosVector = chunkPos.position;
        float randomX = Random.Range(chunkPosVector.x - 4, chunkPosVector.x + 4);
        float randomZ = Random.Range(chunkPosVector.z - 4, chunkPosVector.z + 4);
        Vector3 v = new Vector3(randomX, 0, randomZ);
        Obstacle.transform.position = v;
    }
}
