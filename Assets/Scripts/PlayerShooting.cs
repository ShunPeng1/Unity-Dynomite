using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject shootingPlatform;
    private Vector3 _platformPosition;
    private GameObject _holdingBall;
    private Vector3 ShootingDirection(Vector3 mousePosition)
    {
        Vector3 deltaPosition = mousePosition - _platformPosition;
        float deltaLenght =Mathf.Pow( Mathf.Pow(deltaPosition.x, 2f) + Mathf.Pow(deltaPosition.y, 2f), 0.5f);
        return deltaPosition / deltaLenght;
    }

    private void Awake()
    {
        _platformPosition = shootingPlatform.transform.position;
        _holdingBall = null;
    }

    private void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        
        if(_holdingBall == null) 
        {
            _holdingBall = Instantiate(GameAssets.Instance.BallPrefab(), _platformPosition, Quaternion.identity);
            _holdingBall.GetComponent<BallMoving>().InitialSpawnHanging();
            
        }
        
        if (Input.GetMouseButtonDown(0))//Left Button
        {
            StartCoroutine( DelayShooting(mousePosition));
        }
    }

    IEnumerator DelayShooting(Vector3 mousePosition)
    {
        _holdingBall.GetComponent<BallMoving>().ChangeStateToShooting( ShootingDirection(mousePosition));
        _holdingBall = null;
        yield return new WaitForSeconds(1f);
    }

}
