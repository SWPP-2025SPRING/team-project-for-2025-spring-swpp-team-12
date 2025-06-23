using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DropTile.cs

public class DropTile : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // 초기엔 안 움직이게
    }

    public void TriggerFall()
    {
        // Debug.Log("drop tile");

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        transform.position -= Vector3.up * 1.0f;

        // 아래로 힘을 직접 가함
        rb.AddForce(Vector3.down * 500f, ForceMode.Impulse); // 빠르게 툭 떨어지는 느낌

        // 살짝 회전력도 추가 (불규칙한 낙하 느낌)
        rb.AddTorque(Random.onUnitSphere * 150f, ForceMode.Impulse);

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBox"))
        {
            Invoke("TriggerFall", Random.Range(0.1f, 0.5f));
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by drop tile");
            other.GetComponent<Health>().ChangeHealth(100);
        }
    }
    
    
}

