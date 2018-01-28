using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;

    public int currentTurn;

    public float turnTime;
    private float turnTimeCounter;

    private int turnsToSkip;
    private int playerToSkip;

    private int turnsToMislead;
    private int playerToMislead;

    private int turnsToProtect;
    private int playerToProtect;

    CardGenerator generator;

    public Card card;

    // Use this for initialization
    void Start ()
    {
        currentTurn = 1;
        turnTime = 20;
        turnTimeCounter = 0;

        player1.isPlaying = true;

        playerToSkip = 0;
        turnsToSkip = 0;

        playerToMislead = 0;
        turnsToMislead = 0;

        generator = GetComponent<CardGenerator>();

        GenerateRoundCards();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GenerateRoundCards();
       
        if ((playerToSkip == 1 || playerToSkip == 2) && turnsToSkip <= 0)
        {
            playerToSkip = 0;
            turnsToSkip = 0;
        }

        if ((playerToMislead == 1 || playerToMislead == 2) && turnsToMislead <= 0)
        {
            playerToMislead = 0;
            turnsToMislead = 0;
        }

        if ((playerToProtect == 1 || playerToProtect == 2) && turnsToProtect <= 0)
        {
            playerToProtect = 0;
            turnsToProtect = 0;
        }

        if (player1.isProtected)
        {
            if (player1.isMisleaded)
            {
                playerToMislead = 0;
                turnsToMislead = 0;
            }

            if (playerToSkip == 1)
            {
                playerToSkip = 0;
                turnsToSkip = 0;
            }
        }
        else if(player2.isProtected)
        {
            if (player2.isMisleaded)
            {
                playerToMislead = 0;
                turnsToMislead = 0;
            }

            if (playerToSkip == 2)
            {
                playerToSkip = 0;
                turnsToSkip = 0;
            }
        }

        if (turnTimeCounter >= turnTime)
        {
            turnTimeCounter = 0;

            if (playerToMislead == 1)
            {
                player1.isMisleaded = true;
                turnsToMislead--;
            }
            else if (playerToMislead == 2)
            {
                player2.isMisleaded = true;
                turnsToMislead--;
            }

            if (playerToProtect == 1)
            {
                player1.isProtected = true;
                turnsToProtect--;
            }
            else if (playerToProtect == 2)
            {
                player2.isProtected = true;
                turnsToProtect--;
            }

            if (currentTurn == 1 && playerToSkip != 1)
            {
                if (playerToSkip == 2)
                {
                    turnsToSkip--;
                }

                player1.isPlaying = false;
                player2.isPlaying = true;

                player2.energy = 100;

                currentTurn = 2;
            }
            else
            {
                if (playerToSkip == 1)
                {
                    turnsToSkip--;
                }

                player1.isPlaying = true;
                player2.isPlaying = false;

                player1.energy = 100;

                currentTurn = 1;
            }
        }
        else
        {
            turnTimeCounter += Time.deltaTime;
        }
	}

    public void SkipTurn(int playerNum, int turnNum)
    {
        turnsToSkip = turnNum;
        playerToSkip = playerNum;
    }

    public void MisleadPlayer(int playerNum, int turnNum)
    {
        playerToMislead = playerNum;
        turnsToMislead = turnNum;
    }

    public void ProtectPlayer(int playerNum, int turnNum)
    {
        playerToProtect = playerNum;
        turnsToProtect = turnNum;
    }

    public void GenerateRoundCards()
    {
        if (player1.remainingCards == 0)
        {
            Card[] cards = new Card[4];

            for (int i = 0; i < 4; i++)
            {
                cards[i] = Instantiate(card, player1.spawners[i].transform.position, Quaternion.identity, player1.spawners[i].transform);
                cards[i].GenerateCard(generator);
                cards[i].name += (i + "");
            }

            player1.cards = cards;
            player1.remainingCards = 4;
        }

        if (player2.remainingCards == 0)
        {
            Card[] cards = new Card[4];

            for (int i = 0; i < 4; i++)
            {
                cards[i] = Instantiate(card, player2.spawners[i].transform.position, Quaternion.identity, player2.spawners[i].transform);
                cards[i].GenerateCard(generator);
                cards[i].name += (i + "");
            }

            player2.cards = cards;
            player2.remainingCards = 4;
        }
    }
}