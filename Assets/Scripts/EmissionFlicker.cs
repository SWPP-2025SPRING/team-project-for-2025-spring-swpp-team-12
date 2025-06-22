using UnityEngine;

public class EmissionFlicker : MonoBehaviour
{
    private Renderer rend;
    private Material mat;
    private Color baseEmissionColor;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material; // 개별 인스턴스 material
            baseEmissionColor = mat.GetColor("_EmissionColor"); // 현재 설정된 emission color
        }
    }

    void Update()
    {
        if (mat == null) return;

        float emissionStrength = Mathf.PingPong(Time.time * 2f, 0.5f) + 0.5f; // 1 ~ 2 사이
        Color emissionColor = baseEmissionColor * emissionStrength;
        mat.SetColor("_EmissionColor", emissionColor);
    }
}
