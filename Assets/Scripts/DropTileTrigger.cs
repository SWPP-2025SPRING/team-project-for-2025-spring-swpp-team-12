using UnityEngine;

public class DropTileTrigger : MonoBehaviour
{
    [SerializeField] private float extraHeight = 3f; // 위쪽으로 탐색할 추가 높이
    [SerializeField] private LayerMask tileLayer;
    private bool dropped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (dropped) return;

        //Debug.Log("Player triggered tile check");

        BoxCollider myCollider = GetComponent<Collider>() as BoxCollider;
        if (myCollider == null)
        {
            //Debug.LogWarning("DropTileTrigger needs a BoxCollider");
            return;
        }

        Bounds bounds = myCollider.bounds;

        Vector3 center = bounds.center + Vector3.up * (bounds.extents.y + extraHeight / 2f);
        Vector3 halfExtents = new Vector3(bounds.extents.x*0.9f, extraHeight / 2f, bounds.extents.z*0.9f);
        //x,z 값을 collider 기반, 추후 수정 가능

        Collider[] hits = Physics.OverlapBox(
            center,
            halfExtents,
            Quaternion.identity,
            ~0); //layer 만들면 tile layer 등으로 처리 가능

        foreach (Collider hit in hits)
        {
            DropTile tile = hit.GetComponent<DropTile>();
            if (tile != null)
            {
                tile.TriggerFall();
            }

        }

        dropped = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        BoxCollider myCollider = GetComponent<Collider>() as BoxCollider;
        if (myCollider == null) return;

        Bounds bounds = myCollider.bounds;
        Vector3 center = bounds.center + Vector3.up * (bounds.extents.y + extraHeight / 2f);
        Vector3 halfExtents = new Vector3(bounds.extents.x*0.9f, extraHeight / 2f, bounds.extents.z*0.9f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, halfExtents * 2f);
    }
#endif
}
