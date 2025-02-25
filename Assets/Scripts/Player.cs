using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100f);

    public override void OnNetworkSpawn()
    {
        if (IsServer && !IsClient)
        {
            Debug.Log($"Player {OwnerClientId} is running as SERVER.");
        }
        else if (IsHost)
        {
            Debug.Log($"Player {OwnerClientId} is running as HOST.");
        }
        else if (IsClient)
        {
            Debug.Log($"Player {OwnerClientId} is running as CLIENT.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ApplyServerRPC(float damage, ulong attackerId)
    {
        health.Value -= damage;
        Debug.Log($"Player {OwnerClientId} took {damage} damage from Player {attackerId}");

        if (health.Value <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        health.Value = 100;
        Debug.Log($"Player {OwnerClientId} has respawned.");
    }
}

