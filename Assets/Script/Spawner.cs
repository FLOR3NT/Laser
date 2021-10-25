using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    bool isUse = false;

    public bool IsUse
    {
        get => isUse;
        set
        {
            if (isUse != value)
            {
                if (value)
                {

                    gameManager.AvaibleSpawners.Remove(this);
                }
                else
                {
                    gameManager.AvaibleSpawners.Add(this);
                }
                isUse = value;
            }
        }
    }
}