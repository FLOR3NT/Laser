using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private float newspawn = 0;
    private int cubeToSpawn = 0;
    private int cubeSpawned = 0;
    [SerializeField] private List<Spawner> allSpawners = new List<Spawner>();
    private List<Spawner> avaibleSpawners = new List<Spawner>();
    [SerializeField] private float spawnFrequency = 1;
    [SerializeField] private Color[] colorCubes = new Color[9];
    [SerializeField] private Color colorBoost = Color.green;



    [SerializeField] private CubeManager cubePrefab;
    private List<CubeManager> usedCube = new List<CubeManager>();
    [SerializeField] private Transform usedCubeParent;
    private List<CubeManager> unusedCube = new List<CubeManager>();
    [SerializeField] private Transform unusedCubeParent;

    private float score = 0;
    [SerializeField]private float step = 1;
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
            SetStep();
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

        if (cubeToSpawn > 0)
        {
            if (SpawnCube())
            {
                cubeSpawned++;
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
            newCube.CurrentSpawner = avaibleSpawners[randomSpot];
            newCube.transform.position = avaibleSpawners[randomSpot].transform.position;
            newCube.transform.eulerAngles = Vector3.up * Random.Range(0, 360);
            float roundStep = Mathf.Round(Mathf.Exp(step/100));
            Debug.Log(step + " | " + Mathf.Exp(step)/100 + " | " + Mathf.Round(Mathf.Exp(step)));
            if (roundStep < 1)
            {
                roundStep = 1;
            }

            if (cubeSpawned % 5 == 0)
            {
                newCube.Boost = randomSpot % 2 == 0 ? Boost.Damage : Boost.AttackSpeed;
                newCube.MaxHealth = 1;
                newCube.MaxVelocity = 5;
                newCube.AddedScore = 5 * roundStep;
                newCube.SetColor(colorBoost);
            }
            else
            {
                newCube.Boost = Boost.None;
                newCube.MaxHealth = 50 + 50 * ((roundStep - 1) / 2);
                newCube.MaxVelocity = 3 + 3 * ((roundStep - 1) / 2);
                newCube.AddedScore = 5 * roundStep;
                newCube.SetColor(colorCubes[((int)((roundStep % 9) - 1))]);
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

    public void SetStep()
    {
        step = Mathf.Pow(score, 1F / 2F);
        if (step < 1)
        {
            step = 1;
        }
    }
}