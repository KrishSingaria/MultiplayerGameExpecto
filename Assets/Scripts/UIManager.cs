using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public Dropdown playerDropdown;
    public Button playCardButton;

    [Header("Card System")]
    public List<Card> availableCards;
    public Player localPlayer;

    private List<Player> allPlayers = new List<Player>();

    void Awake()
    {
        Instance = this;
    }

    public void SetLocalPlayer(Player player)
    {
        localPlayer = player;
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        allPlayers.Clear();
        allPlayers.AddRange(FindObjectsByType<Player>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        playerDropdown.ClearOptions();
        List<string> playerNames = new List<string>();

        foreach (var player in allPlayers)
        {
            if (player != localPlayer)
            {
                playerNames.Add($"Player {player.OwnerClientId}");
            }
        }

        playerDropdown.AddOptions(playerNames);
    }

    public void PlayCard()
    {
        if (localPlayer == null) return;

        int selectedIndex = playerDropdown.value;
        Player targetPlayer = allPlayers[selectedIndex];

        Card selectedCard = availableCards[0]; // Use first card for now

        Debug.Log($"Playing {selectedCard.CardName} on Player {targetPlayer.OwnerClientId}");
        selectedCard.ApplyCardEffect(targetPlayer, localPlayer.OwnerClientId);
    }
}
