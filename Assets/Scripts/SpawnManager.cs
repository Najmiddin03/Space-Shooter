using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private float _enemyCycle;

    private bool _stopSpawning = false;

    void Start()
    {
        _enemyCycle = Random.Range(3f, 4f);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemyCycle);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void Leveling(int level)
    {
        switch (level)
        {
            case 0:
                _enemyCycle = Random.Range(1f, 2f);
                break;
            case 1:
                _enemyCycle = Random.Range(0.5f, 1f);
                break;
            case 2:
                _enemyCycle = Random.Range(0.2f, 0.5f);
                break;
        }        
    }
}
