using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite thumbnail;
    public GameObject prefab;

    [Header("Effect")]
    public ScriptableObject effectAsset;

    public void ApplyEffect(GameObject player)
    {
        if (effectAsset is IEffect effect)
        {
            effect.Apply(player);
        }
        else
        {
            Debug.LogWarning($"{itemName} has no valid effect assigned.");
        }
    }

    public abstract void OnPickup(GameObject player);
    public abstract void OnUse(GameObject player);
}
