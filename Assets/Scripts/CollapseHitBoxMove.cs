using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseHitBoxMove : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Direction> directions;
    private int currentIndex = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;
    [SerializeField] private float speed = 50f;
    void Start()
    {
        directions = MapLoader.DirectionList;
        directions.Insert(0, Direction.Front);
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving || currentIndex >= directions.Count)
        {
            Destroy(gameObject);
            return;
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            SetNextTarget();
        }

    }

    void SetNextTarget()
    {
        if (currentIndex >= directions.Count)
        {
            isMoving = false;
            return;
        }

        Vector3 directionVector = DirectionToVector(directions[currentIndex]);
        Quaternion rotation = DirectionToRotation(directions[currentIndex]);
        transform.rotation = rotation;
        targetPosition = transform.position + directionVector.normalized * 70f;

        currentIndex++;
        isMoving = true;
    }

    private Vector3 DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Vector3.up;
            case Direction.Down: return Vector3.down;
            case Direction.Left: return Vector3.left;
            case Direction.Right: return Vector3.right;
            case Direction.Front: return Vector3.forward;
            case Direction.Back: return Vector3.back;
            default: return Vector3.zero;
        }
    }
    
    private Quaternion DirectionToRotation(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left: return Quaternion.Euler(0, -90, 0);
            case Direction.Right: return Quaternion.Euler(0, 90, 0);
            case Direction.Front: return Quaternion.identity;
            case Direction.Back: return Quaternion.Euler(0, 180, 0);

            default: return Quaternion.identity;
        }
    }
}
