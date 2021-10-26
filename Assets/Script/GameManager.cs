using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private float newspawn = 0;
    private int cubeToSpawn = 0;
    [SerializeField] private List<Spawner> allSpawners = new List<Spawner>();
    private List<Spawner> avaibleSpawners = new List<Spawner>();
    [SerializeField] private float spawnFrequency = 1;

    [SerializeField] private CubeManager cubePrefab;
    private List<CubeManager> usedCube = new List<CubeManager>();
    [SerializeField] private Transform usedCubeParent;
    private List<CubeManager> unusedCube = new List<CubeManager>();
    [SerializeField] private Transform unusedCubeParent;

    private float score = 0;
    [SerializeField] private Text scoreText;
    [SerializeField] private Player player;

    public List<Spawner> AvaibleSpawners { get => avaibleSpawners; set => avaibleSpawners = value; }
    public Player Player { get => player; set => player = value; }
    public float Score 
    { 
        get => score;
        set
        {
            score = value;
            scoreText.text = value.ToString();
        } 
    }

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

            CubeManager newCube;
            if (unusedCube.Count > 0)
            {
                newCube = unusedCube[0];
                unusedCube.Remove(newCube);
                newCube.transform.parent = usedCubeParent;
                newCube.gameObject.SetActive(true);
            }
            else
            {
                newCube = Instantiate(cubePrefab, usedCubeParent);
                newCube.GameManager = this;
            }

            usedCube.Add(newCube);
            newCube.transform.position = avaibleSpawners[randomSpot].transform.position;
            newCube.transform.eulerAngles = Vector3.up * Random.Range(0,360);
            newCube.CurrentHealth = newCube.MaxHealth;
            //newCube.MaxHealth = 
            //newCube.MaxVelocity = 
            //newCube.AddedScore = 
            avaibleSpawners[randomSpot].IsUse = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisableCube(CubeManager cube)
    {
        usedCube.Remove(cube);
        unusedCube.Add(cube);
        cube.transform.parent = unusedCubeParent;
        cube.gameObject.SetActive(false);
    }

    public void Lose()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}