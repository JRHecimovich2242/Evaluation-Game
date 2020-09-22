using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float ActiveGameTime = 0f;
    public int CurrentWave = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentWave(int currWave)
    {
        CurrentWave = currWave;
    }
}
