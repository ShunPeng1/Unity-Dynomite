using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RowOfMap : MonoBehaviour
{
    

    private float speed;
    private Rigidbody2D _rb;
    private void Start()
    {
        speed = 1 / GameMap.Instance.spawnRate;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.velocity = Vector2.down * speed;
    }

    private void Update()
    {
        
    }
}
