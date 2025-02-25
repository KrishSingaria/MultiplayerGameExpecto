using UnityEngine;

[System.Serializable]

public class Card
{
    public string Cardname;
    public float damage;

    public void ApplyCardEffect(Player target, ulong attackerId)
    {
        if (target != null)
        {
            target.ApplyServerRPC(damage, attackerId);
        }
    }
}

