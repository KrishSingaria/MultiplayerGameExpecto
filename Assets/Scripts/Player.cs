using UnityEngine;
using Unity.Netcode;
public class Player : NetworkBehaviour
{

    [SerializeField] public float health = 100;

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
        Debug.Log($"Player {OwnerClientId} took {damage} damage from Player {attackerid}");
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
