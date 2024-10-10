using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Transform handTransform;  // Where cards will be displayed
    public GameObject cardPrefab;    // The card prefab to instantiate
    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite snowSprite;

    private List<Card> deck = new List<Card>();
    private List<Card> hand = new List<Card>();

    // Initialize the deck with some cards
    public void InitializeDeck()
    {
        Debug.Log("Initializing deck...");

        AddCardToDeck("Fire", 5);
        AddCardToDeck("Water", 3);
        AddCardToDeck("Snow", 7);
        AddCardToDeck("Fire", 6);
        AddCardToDeck("Water", 2);
        AddCardToDeck("Snow", 8);

        ShuffleDeck();
    }

    // Add a card to the deck
    public void AddCardToDeck(string element, int power)
    {
        Debug.Log($"Adding card to deck: {element} with power: {power}");

        GameObject newCardObject = Instantiate(cardPrefab, handTransform);
        Card newCard = newCardObject.GetComponent<Card>();

        if (newCard == null)
        {
            Debug.LogError("Card component missing on prefab!");
            return;
        }

        newCard.InitializeCard(element, power, this);

        // Assign card images based on element
        if (element == "Fire")
        {
            newCard.cardImage.sprite = fireSprite;
        }
        else if (element == "Water")
        {
            newCard.cardImage.sprite = waterSprite;
        }
        else if (element == "Snow")
        {
            newCard.cardImage.sprite = snowSprite;
        }

        deck.Add(newCard);
    }

    // Shuffle the deck
    public void ShuffleDeck()
    {
        Debug.Log("Shuffling the deck...");
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Debug.Log("Deck shuffled.");
    }

    // Draw cards to the player's hand
    public void DrawCards(int numberOfCards)
    {
        Debug.Log("Drawing " + numberOfCards + " cards...");
        for (int i = 0; i < numberOfCards; i++)
        {
            if (deck.Count > 0)
            {
                Card drawnCard = deck[0];
                hand.Add(drawnCard);
                deck.RemoveAt(0);
                Debug.Log("Drew card: " + drawnCard.element + " with power: " + drawnCard.power);
            }
            else
            {
                Debug.LogError("Deck is empty!");
            }
        }
    }

    // Select a random card (for the opponent)
    public Card SelectRandomCard()
    {
        if (hand.Count > 0)
        {
            int randomIndex = Random.Range(0, hand.Count);
            Card selectedCard = hand[randomIndex];
            Debug.Log("Opponent selected card: " + selectedCard.element + " with power: " + selectedCard.power);
            return selectedCard;
        }
        else
        {
            Debug.LogError("No cards in hand to select!");
            return null;
        }
    }

    // Play a selected card
    public void PlayCard(Card card)
    {
        if (card != null)
        {
            Debug.Log("Played card: " + card.element + " with power: " + card.power);
            hand.Remove(card);
        }
        else
        {
            Debug.LogError("No card to play!");
        }
    }
}
