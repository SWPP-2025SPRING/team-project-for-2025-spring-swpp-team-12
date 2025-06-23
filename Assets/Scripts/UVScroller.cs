using UnityEngine;

public class UVScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.5f, 0f); // X축으로 흐르게

    private Renderer rend;
    private Material mat;
    private Vector2 currentOffset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material;
        }
    }

    void Update()
    {
        if (mat == null) return;

        currentOffset += scrollSpeed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", currentOffset); // 알베도 텍스처 기준

        // Emission Map도 함께 흐르게 하고 싶다면 이 줄도 추가
        mat.SetTextureOffset("_EmissionMap", currentOffset);
    }
}
