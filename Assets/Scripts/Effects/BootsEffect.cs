using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Boots")]
public class BootsEffect : ScriptableObject, IEffect
{

    [SerializeField] private float duration = 10f;

    public void Apply(GameObject player)
    {
        player.GetComponent<MonoBehaviour>().StartCoroutine(GrantImmunity());
    }

    private IEnumerator GrantImmunity()
    {
        GameManager.Instance.isSlowImmune = true;
        Debug.Log("슬로우 면역 시작");

        yield return new WaitForSeconds(duration);

        GameManager.Instance.isSlowImmune = false;
        Debug.Log("슬로우 면역 종료");
    }
}
