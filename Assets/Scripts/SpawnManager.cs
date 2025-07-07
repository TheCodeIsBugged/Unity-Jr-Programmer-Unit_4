using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public TextMeshProUGUI waveText;
    private GameObject boss;
    private float spawnRange = 9f;
    private int enemyCount;
    private static int waveNumber;
    private int bossRound = 5;
    // Start is called before the first frame update
    void Start()
    {
        waveNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        boss = GameObject.Find("Boss(Clone)");

        if (enemyCount == 0)
        {
            if (waveNumber % bossRound == 0 && boss == null)
            {
                SetWaveText(waveNumber);
                SpawnEnemyWave(waveNumber);
                SpawnBoss();
            }
            else
            {
                SetWaveText(waveNumber);
                SpawnEnemyWave(waveNumber);
            }

            waveNumber++;
            SpawnPowerup();
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        int enemyIndex;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            enemyIndex = Random.Range(0, enemyPrefabs.Length - 1);
            Instantiate(enemyPrefabs[enemyIndex], GenerateSpawsPosition(), enemyPrefabs[enemyIndex].transform.rotation);
        }
    }

    void SpawnPowerup()
    {
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawsPosition(), powerupPrefabs[randomPowerup].transform.rotation);
    }

    Vector3 GenerateSpawsPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0f, spawnPosZ);

        return randomPos;
    }

    void SpawnBoss()
    {
        Instantiate(enemyPrefabs[enemyPrefabs.Length - 1], GenerateSpawsPosition(), enemyPrefabs[enemyPrefabs.Length - 1].transform.rotation);
    }

    void SetWaveText(int waveCount)
    {
        waveText.text = $"WAVE {waveCount}";
    }
}
