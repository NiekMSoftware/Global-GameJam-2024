using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        Enemy[] temp = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in temp)
        {
            GameObject tempObj = enemy.gameObject;
            enemies.Add(tempObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().isDead)
            {
                enemies.Remove(enemy);
            }
        }
        if(enemies.Count == 0)
        {
            StartCoroutine(WaitTillBoss());
        }
    }

    IEnumerator WaitTillBoss()
    {
        yield return new WaitForSeconds(20f);

        SceneLoader.LoadScene("EndBoss");
    }
}
