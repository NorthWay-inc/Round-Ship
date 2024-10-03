using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private List<Transform> spawnPosition;

    [Header("Prefbs Object")]
    [SerializeField] private List<GameObject> prefabsObj;

    [Header("Setting Params")]
    [SerializeField] private float spawnTime;

    private void Start()
    {
        StartCoroutine("ITimeSpawn");
    }

    private IEnumerator ITimeSpawn()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(spawnTime);
    }

    /// <summary>
    /// Compare and sets parameters for the child
    /// </summary>
    private void SpawnEnemy()
    {
        float _randomNumber = Random.Range(0.01f, 0.09f);
        GameObject _obj = Instantiate(prefabsObj[0], transform);

        _obj.GetComponent<EnemyMove>().Init(_randomNumber, main.transform);
    }

}
