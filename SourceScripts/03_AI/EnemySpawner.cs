using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Queue<Monster> monsters;

    public int MonsterCount;
    // Start is called before the first frame update
    void Start()
    {
        Initialize(MonsterCount);
    }
    private void Initialize(int initCount)
    {
        if(monsterPrefab != null)
        {
            for(int i = 0; i < initCount; i++)
                monsters.Enqueue(CreateNewObject());
        }
    }

    private Monster CreateNewObject()
    {
        GameObject obj = GameObject.Instantiate(monsterPrefab, this.transform);
        obj.transform.position = obj.transform.position + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));

        Monster monster = obj.GetComponent<Monster>();

        return monster;
    }
}
