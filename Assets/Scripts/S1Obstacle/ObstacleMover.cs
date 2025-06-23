using UnityEngine;

public class ObstacleMover : MonoBehaviour, IStunnable
{
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool moveHorizontally = true; // true면 x축, false면 y축

    private Vector3 startPos;
    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
                isStunned = false;
            return;
        }

        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        Vector3 moveOffset = moveHorizontally ? new Vector3(offset, 0f, 0f) : new Vector3(0f, offset, 0f);
        transform.position = startPos + moveOffset;
    }

    public void Stun(float duration)
    {
        transform.position = startPos;
        isStunned = true;
        stunTimer = duration;
    }
}