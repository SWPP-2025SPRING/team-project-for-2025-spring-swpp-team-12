using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Effect/Slow")]
public class SlowEffect : ScriptableObject, IEffect
{
    public float duration = 10f;

    public void Apply(GameObject player)
    {
        GameManager.Instance.isSlowed = true;
        MonoBehaviour mono = player.GetComponent<MonoBehaviour>();
        if (mono != null)
        {
            mono.StartCoroutine(ResetSlowAfterDelay(duration));
        }
    }

    private IEnumerator ResetSlowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.isSlowed = false;
    }
}
