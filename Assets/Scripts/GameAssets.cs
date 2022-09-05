using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject rowOfMap;
    [SerializeField] private Sprite[] ballsColor;
    
    
    private void Start()
    {
        Instance = this;
    }

    public GameObject BallPrefab()
    {
        return ball;
    }

    public GameObject RowPrefab()
    {
        return rowOfMap;
    } 
    
    public Sprite GetBallColor(BallColor ballColor)
    {
        
        return ballsColor[(int)ballColor];
        
    }

}
