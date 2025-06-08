using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Effect/Speed Boost")]
public class SpeedBoostEffect : ScriptableObject, IEffect
{
    public float duration = 10f;

    public void Apply(GameObject player)
    {
        GameManager.Instance.SetBoost(true);
        MonoBehaviour mono = player.GetComponent<MonoBehaviour>();
        if (mono != null)
        {
            mono.StartCoroutine(ResetBoostAfterDelay(duration));
        }
    }

    private IEnumerator ResetBoostAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.SetBoost(false);
    }
}
