using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameMap : MonoBehaviour
{
    public static GameMap Instance;

    [SerializeField] private int row, col;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float scale;
    [SerializeField] private int ChanceHavingHoleWhenSpawnNewRow;
    public float spawnRate;

    private float currentTime;

    private List<GameObject[]> _mainMap;
    private List<GameObject> _mainRow;
    private bool _isEvenRow;

    private (int, int) IndexOfNewInsertedBall(Vector3 deltaPosition)
    {
        float angle = Mathf.Atan2(deltaPosition.y, deltaPosition.x) * Mathf.Rad2Deg;

        if (angle < 45.0f && angle >= -45.0f)
        {
            return (0, 1);
        }
        else if (angle < 135.0f && angle >= 45.0f)
        {
            return (1, 0);
        }
        else if (angle < -45.0f && angle >= -135.0f)
        {
            return (-1, 0);
        }
        else
        {
            return (0, -1);
        }
    }

    private IEnumerator MainDeepFirstSearch(GameObject startingBall, int x, int y)
    {
        Dictionary<GameObject, (int, int)> mainDictionary = null;
        Stack<GameObject> mainStack = null;
        mainDictionary.Add(startingBall, (x, y));
        mainStack.Push(startingBall);

        while (mainStack.Count != 0)
        {
            var checkingBall = mainStack.Peek();
            if (x>0)
            {
                if (_mainMap[x - 1][y] != null)
                {
                    //if(mainDictionary.)
                }
            }
        }

        yield break;
    }


    private (int, int) IndexOfBeingHitBall(GameObject hitBall)
    {
        int xHit = 0, yHit = 0;
        for (int i = 0; i < _mainRow.Count; i++)
        {
            if (_mainRow[i] == hitBall.transform.parent.gameObject) xHit = i;
        }

        for (int i = 0; i < _mainMap[xHit].Length; i++)
        {
            if (_mainMap[xHit][i] == hitBall) yHit = i;
        }

        return (xHit, yHit);
    }

    public void InsertNewBall(GameObject insertingBall, GameObject hitBall)
    {
        //Finding index of the hit object
        (int xHit, int yHit) = IndexOfBeingHitBall(hitBall);

        (int xDelta, int yDelta) =
            IndexOfNewInsertedBall(insertingBall.transform.position - hitBall.transform.position);
        Debug.Log(xHit + " " + yHit + " + " + xDelta + " " + yDelta + " = " + xHit + xDelta + ' ' + yHit + yDelta);
        if (xHit + xDelta < 0)
        {
            CreateRow(0); //Create front row
            xHit++;
        }

        _mainMap[xHit + xDelta][yHit + yDelta] = insertingBall;
        insertingBall.GetComponent<BallMoving>().InitialSpawnMapWaiting();
        insertingBall.transform.position = hitBall.transform.position + new Vector3(yDelta, xDelta);
        insertingBall.transform.parent = _mainRow[xHit + xDelta].transform;
    }

    private void FillBackRow()
    {
        int lastIndex = _mainRow.Count - 1;
        for (int i = 0; i < _mainMap[lastIndex].Length; i++)
        {
            if (Random.Range(0, ChanceHavingHoleWhenSpawnNewRow) == 0)
            {
                _mainMap[lastIndex][i] = null;
                continue;
            }

            Vector3 ballPosition = new Vector3(initialPosition.x + i * scale, initialPosition.y);
            GameObject columnBallGo = Instantiate(GameAssets.Instance.BallPrefab(), ballPosition, Quaternion.identity,
                _mainRow[lastIndex].transform);
            columnBallGo.GetComponent<BallMoving>().InitialSpawnMapWaiting();
            _mainMap[lastIndex][i] = columnBallGo;
        }
    }

    private void DeleteFrontRow()
    {
        if (_mainMap.Count > row)
        {
            GameObject[] frontRowBall = _mainMap[0];
            for (int i = 0; i < frontRowBall.Length; i++)
            {
                Destroy(frontRowBall[i]);
            }

            Destroy(_mainRow[0]);
            _mainMap.RemoveAt(0);
            _mainRow.RemoveAt(0);
        }
    }

    private void CreateRow(int index)
    {
        GameObject nextRowGo = Instantiate(GameAssets.Instance.RowPrefab(), initialPosition, Quaternion.identity);
        GameObject[] rowBallGo = new GameObject[col];

        _mainRow.Insert(index, nextRowGo);
        _mainMap.Insert(index, rowBallGo);
    }

    void Start()
    {
        currentTime = 0;

        _mainMap = new List<GameObject[]>();
        _mainRow = new List<GameObject>();
        initialPosition = new Vector3(transform.position.x - col / 2, transform.position.y - 1);

        Instance = this;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= spawnRate)
        {
            DeleteFrontRow();
            CreateRow(_mainMap.Count); //Create New Back Row
            FillBackRow();
            currentTime -= spawnRate;
        }
    }
}