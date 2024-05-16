using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalSpawnManager : MonoBehaviour
{
    public GameObject[] animals;
    // Start is called before the first frame update
    void Start()
    {
        AnimalSpawnArea();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnimalSpawnArea()
    {
        //Physics.OverlapSphere(gameObject.transform.position, 10f);
        for(int i = 0; i < animals.Length; i++)
        {
            Vector3 randomPositon =  GetRandomPosition(gameObject.transform.position,30,30);
            
            Instantiate(animals[i],randomPositon,Quaternion.identity);
        }
        
    }
    Vector3 GetRandomPosition(Vector3 center, float width,float length) 
    {
        float randomX = Random.Range(-width / 2, width / 2);
        float randomZ = Random.Range(-length / 2, length / 2);
        float yVlaue = 0;

        Vector3 randomPosition = new Vector3(randomX, yVlaue, randomZ) + center;        
        return randomPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = gameObject.transform.position;
        Vector3 size = new Vector3(30, 0.1f, 30);


            Gizmos.DrawWireCube(center, size);
    }
}
