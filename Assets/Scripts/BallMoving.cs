using System;
using System.Linq.Expressions;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BallMoving : MonoBehaviour
{
    public Vector2 movingPosition;
    [SerializeField] private float speed;

    private Rigidbody2D _rb;
    public BallColor ballColor { get; set; }
    public BallState ballState { get; set; }

    public void ChangeStateToShooting(Vector3 direction )
    {
        movingPosition = new Vector2(direction.x * speed, direction.y * speed);
        _rb.velocity = movingPosition;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        ballState = BallState.Shooting;
    }

    public void InitialSpawnMapWaiting()
    {
        movingPosition = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        ballState = BallState.MapWaiting;
    }

    public void InitialSpawnHanging()
    {
        movingPosition = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        ballState = BallState.PlayerHanging;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (ballState == BallState.Shooting)
            {
                Vector3 delta = other.transform.position - transform.position;  
                //Debug.Log("this "+transform.position+" , other "+other.gameObject.transform +" delta "+delta);
                GameMap.Instance.InsertNewBall(gameObject, other.gameObject);
            }
        }
    }

    void Start()
    {
        ballColor = (BallColor) Random.Range(0, (int) BallColor.MaxSize);
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.velocity = movingPosition;
        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.GetBallColor(ballColor);
    }

    void Update()
    {
        switch (ballState)
        {
            case BallState.Shooting:
                _rb.velocity = _rb.velocity;
                break;
            default:
                _rb.velocity = Vector2.zero;;
                break;
        }
        
    }
    
}
