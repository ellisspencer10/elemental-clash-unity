using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager playerDeck;   // Reference to the player's deck manager
    public DeckManager opponentDeck; // Reference to the opponent's deck manager
    public TextMeshProUGUI timerText;  // UI element to show the countdown timer
    public TextMeshProUGUI resultText;  // UI element to show result of each round
    public TextMeshProUGUI playerScoreText;  // UI to show player score
    public TextMeshProUGUI opponentScoreText;  // UI to show opponent score
    public Transform middleCardPlayerTransform; // UI area to display the player's selected card in the center
    public Transform middleCardOpponentTransform; // UI area to display the opponent's selected card in the center

    private int playerScore = 0;
    private int opponentScore = 0;
    private const int winningScore = 3;

    private float turnDuration = 15f;  // 15-second turn timer
    private float remainingTime;
    private bool isPlayerTurn = true;
    private bool playerHasPlayedCard = false;
    private Card playerCard;
    private Card opponentCard;

    void Start()
    {
        Debug.Log("Game started. Initializing both decks...");
        
        // Initialize both decks
        playerDeck.InitializeDeck();
        opponentDeck.InitializeDeck();
        StartPlayerTurn();
        UpdateScoreUI();
    }

    // Start the player's turn
    void StartPlayerTurn()
    {
        isPlayerTurn = true;
        playerHasPlayedCard = false;
        remainingTime = turnDuration;

        Debug.Log("Player's turn started!");
        playerDeck.DrawCards(3);  // Draw 3 cards for the player
        opponentDeck.DrawCards(3); // Draw 3 cards for the opponent
        StartCoroutine(PlayerTurnTimer());
    }

    // Coroutine to handle the player's turn timer
    IEnumerator PlayerTurnTimer()
    {
        while (remainingTime > 0 && !playerHasPlayedCard)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        if (!playerHasPlayedCard)
        {
            Debug.Log("Player did not play a card in time.");
            EndPlayerTurn();
        }
    }

    // Update the timer display in the UI
    void UpdateTimerUI()
    {
        timerText.text = "Time Remaining: " + Mathf.Ceil(remainingTime).ToString() + "s";
    }

    // Called when the player selects a card to play
    public void PlayPlayerCard(Card selectedCard)
    {
        if (!isPlayerTurn || playerHasPlayedCard)
            return;

        playerHasPlayedCard = true;
        playerCard = selectedCard;
        playerDeck.PlayCard(playerCard);
        Debug.Log("Player played card: " + playerCard.element + " with power: " + playerCard.power);

        // Move player card to the center
        playerCard.transform.SetParent(middleCardPlayerTransform, false);  // This ensures we are working with the instance, not the prefab

        EndPlayerTurn();
    }

    // End the player's turn and start the opponent's turn
    void EndPlayerTurn()
    {
        isPlayerTurn = false;
        Debug.Log("Ending player's turn. Starting opponent's turn.");
        StartOpponentTurn();
    }

    // Start the opponent's turn
    void StartOpponentTurn()
    {
        Debug.Log("Opponent's turn started!");

        // Opponent selects a card randomly
        opponentCard = opponentDeck.SelectRandomCard();
        
        if (opponentCard == null)
        {
            Debug.LogError("Opponent could not select a card!");
            return;
        }

        opponentDeck.PlayCard(opponentCard);
        Debug.Log("Opponent played card: " + opponentCard.element + " with power: " + opponentCard.power);

        // Move opponent card to the center
        opponentCard.transform.SetParent(middleCardOpponentTransform, false);  // Ensure we're using the instance

        // Compare player and opponent cards to determine the winner
        StartCoroutine(ShowRoundResult());
    }

    // Coroutine to handle showing round results with delay for animation
    IEnumerator ShowRoundResult()
    {
        yield return new WaitForSeconds(1);  // Simulate animation delay

        // Determine the winner
        if (playerCard != null && opponentCard != null)
        {
            if (playerCard.power > opponentCard.power)
            {
                resultText.text = "Player Wins the Round!";
                playerScore++;
            }
            else if (playerCard.power < opponentCard.power)
            {
                resultText.text = "Opponent Wins the Round!";
                opponentScore++;
            }
            else
            {
                resultText.text = "It's a Draw!";
            }

            UpdateScoreUI();
        }
        else
        {
            Debug.LogError("Player or opponent card is missing!");
        }

        // Check for a game winner
        if (playerScore >= winningScore || opponentScore >= winningScore)
        {
            DeclareWinner();
        }
        else
        {
            // Reset for next turn
            yield return new WaitForSeconds(2);  // Delay before resetting
            ResetRound();
            StartPlayerTurn();
        }
    }

    // Update the score UI
    void UpdateScoreUI()
    {
        playerScoreText.text = "Player Score: " + playerScore.ToString();
        opponentScoreText.text = "Opponent Score: " + opponentScore.ToString();
    }

    // Reset the cards in the middle after a round
    void ResetRound()
    {
        playerCard.gameObject.SetActive(false);  // Hide the player's card
        opponentCard.gameObject.SetActive(false);  // Hide the opponent's card
        playerCard = null;
        opponentCard = null;
        resultText.text = "";  // Clear result text
    }

    // Declare the final winner
    void DeclareWinner()
    {
        if (playerScore >= winningScore)
        {
            resultText.text = "Player Wins the Game!";
        }
        else if (opponentScore >= winningScore)
        {
            resultText.text = "Opponent Wins the Game!";
        }
    }
}
