using UnityEngine;
using Unity.Netcode;
public class Player : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health = 100;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            UIManager.Instance.SetLocalPlayer(this);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ApplyServerRPC(float damage, ulong attackerid) 
    {
        health -= damage;
        Debug.Log($"Player {OwnerClientId} took {damage} damage from Player {attackerId}");
        if (health < 0) 
        {
            Respawn();
        }
    }
    public void Respawn() 
    {
        health = 100;
    }
}
