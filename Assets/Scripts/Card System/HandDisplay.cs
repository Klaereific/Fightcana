using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    public CardManager cardManager;
    public Transform handArea;
    public GameObject cardUIPrefab;

    private readonly List<GameObject> spawned = new List<GameObject>();

    public void Refresh()
    {
        // Clear existing
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null) Destroy(spawned[i]);
        }
        spawned.Clear();

        if (cardManager == null || cardManager.hand == null) return;

        // Spawn current hand
        foreach (var card in cardManager.hand)
        {
            var go = Instantiate(cardUIPrefab, handArea);
            var display = go.GetComponent<CardDisplay>();
            if (display != null) display.Setup(card);
            spawned.Add(go);
        }
    }
}