using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    [SerializeField] private GameObject entrance;
    [SerializeField] private GameObject exit;
    [SerializeField] private int amountOfEnemiesToSpawn;

    private bool hasSpawnedEnemies = false;

    private List<Enemy> enemies = new();

    private void Start()
    {
        Open();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSpawnedEnemies) return;

        hasSpawnedEnemies = true;

        Close();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < amountOfEnemiesToSpawn; i++)
        {
            Enemy spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
            spawnedEnemy.room = this;
            enemies.Add(spawnedEnemy);
        }

        yield return null;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private void Update()
    {
        if (enemies.Count <= 0)
        {
            Open();
        }
    }

    private void Close()
    {
        print("CLOSE!");
        entrance.SetActive(true);
        exit.SetActive(true);
    }

    private void Open()
    {
        print("OPEN!");
        entrance.SetActive(false);
        exit.SetActive(false);
    }
}
