using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isPlaying = false;
    private float newspawn = 0;
    private float nextIncreaseSpawn = 0;
    private int cubeToSpawn = 0;
    private int cubeSpawned = 0;
    [SerializeField] private List<Spawner> allSpawners = new List<Spawner>();
    private List<Spawner> avaibleSpawners = new List<Spawner>();
    [SerializeField] private float spawnFrequency = 1;
    [SerializeField] private float increaseSpawnFrequency = 3;
    [SerializeField] private int boostFrequency = 20;
    [SerializeField] private Color[] colorCubes = new Color[9];
    [SerializeField] private Color colorBoost = Color.green;

    [SerializeField] private CubeManager cubePrefab;
    private List<CubeManager> usedCubes = new List<CubeManager>();
    [SerializeField] private Transform usedCubeParent;
    private List<CubeManager> unusedCubes = new List<CubeManager>();
    [SerializeField] private Transform unusedCubeParent;

    private float score = 0;
    [SerializeField] private float step = 1;
    private float lastStep = 1;
    [SerializeField] private Text scoreText;
    [SerializeField] private Player player;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private MenuManager menuManager;

    public List<Spawner> AvaibleSpawners { get => avaibleSpawners; set => avaibleSpawners = value; }
    public Player Player { get => player; set => player = value; }
    public float Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = value.ToString();
            SetStep();
        }
    }

    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

    private void Start()
    {
        avaibleSpawners = new List<Spawner>(allSpawners);
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        if (nextIncreaseSpawn - Time.time <= 0)
        {
            nextIncreaseSpawn = Time.time + increaseSpawnFrequency;
            spawnFrequency += spawnFrequency * 0.1F;
        }

        if (newspawn - Time.time <= 0)
        {
            newspawn = Time.time + 1 / spawnFrequency;
            cubeToSpawn++;
        }

        if (cubeToSpawn > 0)
        {
            if (SpawnCube())
            {
                cubeSpawned++;
                cubeToSpawn--;
            }
        }
    }

    public void Initialize()
    {
        newspawn = 0;
        cubeToSpawn = cubeSpawned = 0;
        spawnFrequency = 1;
        for (int id = 0; id < usedCubes.Count; id++)
        {
            DisableCube(usedCubes[id]);
            id--;
        }
        Score = 0;

        player.Initialize();
        isPlaying = true;
    }

    private bool SpawnCube()
    {
        if (avaibleSpawners.Count > 0)
        {
            int randomSpot = Random.Range(0, avaibleSpawners.Count);
            Transform spawner = avaibleSpawners[randomSpot].transform;

            CubeManager newCube;
            if (unusedCubes.Count > 0)
            {
                newCube = unusedCubes[0];
                unusedCubes.Remove(newCube);
                newCube.transform.parent = usedCubeParent;
                newCube.gameObject.SetActive(true);
            }
            else
            {
                newCube = Instantiate(cubePrefab, usedCubeParent);
                newCube.GameManager = this;
            }

            usedCubes.Add(newCube);
            newCube.CurrentSpawner = avaibleSpawners[randomSpot];
            newCube.transform.position = avaibleSpawners[randomSpot].transform.position;
            newCube.transform.eulerAngles = Vector3.up * Random.Range(0, 360);

            if (cubeSpawned % boostFrequency == 0)
            {
                newCube.Boost = randomSpot % 2 == 0 ? Boost.Damage : Boost.AttackSpeed;
                newCube.MaxHealth = 1;
                newCube.MaxVelocity = 5;
                newCube.AddedScore = Mathf.Round(5 + 5 * (step - 1));
                newCube.SetColor(colorBoost);
            }
            else
            {
                newCube.Boost = Boost.None;
                newCube.MaxHealth = 50 + 50 * ((step - 1) / 2);
                newCube.MaxVelocity = 3 + Mathf.Log(step + 1);
                newCube.AddedScore = Mathf.Round(5 + 5 * (step - 1));
                newCube.SetColor(colorCubes[(int)(step % 9)]);
            }
            newCube.Rb.velocity = Vector3.zero;
            newCube.CurrentHealth = newCube.MaxHealth;
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
        usedCubes.Remove(cube);
        unusedCubes.Add(cube);
        cube.transform.parent = unusedCubeParent;
        cube.gameObject.SetActive(false);
    }

    public void Lose()
    {
        if (score > dataManager.MaxScore)
        {
            dataManager.MaxScore = score;
        }
        dataManager.TotalPoint += Mathf.Round(score / 5 + score / 5 * dataManager.BonusPoint / 10);
        Stop();
        menuManager.OpenMenu();
    }

    public void Stop()
    {
        isPlaying = false;
        for (int id = 0; id > usedCubes.Count; id++)
        {
            usedCubes[id].Rb.velocity = Vector3.zero;
        }
        player.Stop();
    }


    public void SetStep()
    {
        step = Mathf.Round(Mathf.Log(score+1, 2F));
        if (step < 1)
        {
            step = 1;
        }
        if (lastStep != step)
        {
            lastStep = step;
            spawnFrequency = 1;
            nextIncreaseSpawn = Time.time + increaseSpawnFrequency;
        }
    }
}