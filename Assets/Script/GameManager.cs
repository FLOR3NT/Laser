using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private List<Spawner> allSpawners = new List<Spawner>();
    private List<Spawner> avaibleSpawners = new List<Spawner>();
    [SerializeField] private float spawnFrequency = 1;
    private float newspawn = 0;
    private int cubeToSpawn = 0;
    [SerializeField] private Transform cubeParent;
    [SerializeField] private List<CubeManager> cubes = new List<CubeManager>();
    [SerializeField] private CubeManager cubePrefab;
    [SerializeField] private Player player;

    public List<Spawner> AvaibleSpawners { get => avaibleSpawners; set => avaibleSpawners = value; }

    private void Start()
    {
        avaibleSpawners = new List<Spawner>(allSpawners);
    }

    private void Update()
    {
        if (Time.time - newspawn >= spawnFrequency)
        {
            newspawn = Time.time + spawnFrequency;
            cubeToSpawn++;
        }

        if (cubeToSpawn>0)
        {
            if (SpawnCube())
            {
                cubeToSpawn--;
            }
        }
    }

    private bool SpawnCube()
    {
        if (avaibleSpawners.Count > 0)
        {
            int randomSpot = Random.Range(0, avaibleSpawners.Count);
            Transform spawner = avaibleSpawners[randomSpot].transform;
            CubeManager cube = Instantiate(cubePrefab, avaibleSpawners[randomSpot].transform.position, Quaternion.identity, cubeParent);
            cube.Player = player;
            cubes.Add(cube);
            avaibleSpawners[randomSpot].IsUse = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Lose()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}