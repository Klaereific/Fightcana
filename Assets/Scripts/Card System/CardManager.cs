using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public enum rarity
{
    common,
    legendary
}

public class CardManager : MonoBehaviour
{
    public DeckDefinition deckDefinition;
    public HandDisplay handDisplay;
    public int deckSize = 16;
    public int batchSize = 4;
    public float cycleInterval = 5f; // seconds

    [Header("Draw Meter Settings")]
    public float drawMeterMax = 100f;
    public float meterGainPerSecond = 1f;   
    public float meterGainOnHitDealt = 15f; 
    public float meterGainOnHitTaken = 10f; 
    public Slider drawMeterSlider;

    private float currentDrawMeter = 0f;

    // Deck model: front is index 0
    public List<CardData> deck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public Queue<List<CardData>> graveyardBatches = new Queue<List<CardData>>();

    // Draw meter gate; must be set true by game logic (e.g., timer/UI) to allow neutral R1 draw
    void Start()
    {
        // Build deck from DeckDefinition and shuffle deterministically once
        List<CardData> allCards = new List<CardData>(deckDefinition != null ? deckDefinition.cards : new List<CardData>());

        System.Random rng = new System.Random(12345);
        for (int i = allCards.Count - 1; i > 0; i--)
        {
            int k = rng.Next(i + 1);
            (allCards[i], allCards[k]) = (allCards[k], allCards[i]);
        }
        deck = allCards;

        // Draw initial hand as a batch
        hand = DrawBatch(batchSize);

        if (handDisplay != null) handDisplay.Refresh();

        // Start cycle loop to return oldest graveyard batch to deck front periodically
        StartCoroutine(CycleLoop());

        currentDrawMeter = 0f;
        UpdateMeterUI();
    }

    void Update()
    {
        if (currentDrawMeter < drawMeterMax)
        {
            AddMeter(meterGainPerSecond * Time.deltaTime);
        }
    }

    private void AddMeter(float amount)
    {
        currentDrawMeter += amount;
        currentDrawMeter = Math.Clamp(currentDrawMeter, 0, drawMeterMax);
        UpdateMeterUI();
    }

    public void AddMeterOnHitDealt()
    {
        AddMeter(meterGainOnHitDealt);
    }
    public void AddMeterOnHitTaken()
    {
        AddMeter(meterGainOnHitTaken);
    }

    private void UpdateMeterUI()
    {
        if (drawMeterSlider != null)
        {
            drawMeterSlider.value = currentDrawMeter / drawMeterMax;
        }
    }

    private List<CardData> DrawBatch(int count)
    {
        int take = Mathf.Min(count, deck.Count);
        if (take <= 0) return new List<CardData>();
        List<CardData> batch = deck.GetRange(0, take);
        deck.RemoveRange(0, take);
        return batch;
    }

    private void ReturnBatchtoDeck(List<CardData> batch)
    {
        if (batch == null || batch.Count == 0) return;
        deck.AddRange(batch); 
    }

    public void ConsumeHandToGraveyard()
    {
        if (hand.Count == 0) return;
        graveyardBatches.Enqueue(new List<CardData>(hand));
        hand.Clear();

        if (handDisplay != null) handDisplay.Refresh();
    }

    public bool TryDrawNewHand()
    {
        if (currentDrawMeter < drawMeterMax) return false;
        if (hand.Count > 0) return false;

        hand.AddRange(DrawBatch(batchSize));
        currentDrawMeter = 0f;
        UpdateMeterUI();

        if (handDisplay != null) handDisplay.Refresh();
        return hand.Count > 0;
    }

    private IEnumerator CycleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(cycleInterval);
            if (graveyardBatches.Count > 0)
            {
                var batch = graveyardBatches.Dequeue();
                ReturnBatchtoDeck(batch);
            }
        }
    }
}