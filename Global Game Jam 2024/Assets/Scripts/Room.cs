using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new();

    [SerializeField] private GameObject entrance;
    [SerializeField] private GameObject exit;

    private void Start()
    {
        Open();

        foreach (Enemy enemy in enemies)
        {
            enemy.room = this;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Close();
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
        entrance.SetActive(true);
        exit.SetActive(true);
    }

    private void Open()
    {
        entrance.SetActive(false);
        exit.SetActive(false);
    }
}
