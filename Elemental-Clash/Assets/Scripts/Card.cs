using TMPro;  // Ensure you have this namespace for TextMeshPro
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public string element;            // Element type (Fire, Water, Snow)
    public int power;                 // Power level (1-10)
    public TextMeshProUGUI cardText;  // Text component to display card info
    public Image cardImage;           // Image component for displaying the card's element image
    private DeckManager deckManager;  // Reference to the deck manager

    public Sprite fireSprite;         // Sprite for Fire element
    public Sprite waterSprite;        // Sprite for Water element
    public Sprite snowSprite;         // Sprite for Snow element

    // Initialize the card with an element, power, and reference to the DeckManager
    public void InitializeCard(string element, int power, DeckManager manager)
    {
        this.element = element;
        this.power = power;
        this.deckManager = manager;
        UpdateCardUI();
        AssignCardImage();
    }

    // Update the visual text of the card
    public void UpdateCardUI()
    {
        if (cardText == null)
        {
            Debug.LogError("Card text component (TextMeshPro) is not assigned!");
            return;
        }

        cardText.text = element + "\nPower: " + power;
    }

    // Assign the appropriate sprite based on the element
    public void AssignCardImage()
    {
        if (cardImage == null)
        {
            Debug.LogError("Card image component is not assigned!");
            return;
        }

        switch (element.ToLower())
        {
            case "fire":
                cardImage.sprite = fireSprite;
                break;
            case "water":
                cardImage.sprite = waterSprite;
                break;
            case "snow":
                cardImage.sprite = snowSprite;
                break;
            default:
                Debug.LogError("Unknown element: " + element);
                break;
        }
    }

    // This method is triggered when the card is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (deckManager != null)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.PlayPlayerCard(this);  // Pass this card to the GameManager
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }
        }
    }
}