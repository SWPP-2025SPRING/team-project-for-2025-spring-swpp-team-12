using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Restore Stamina")]
public class RestoreStaminaEffect : ScriptableObject, IEffect
{
    [SerializeField] protected int restoreAmount = 50;

    public void Apply(GameObject target)
    {
        if (target.TryGetComponent<Stamina>(out var stamina))
        {
            stamina.ChangeStamina(restoreAmount);
        }
    }
}