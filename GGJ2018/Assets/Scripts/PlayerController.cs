using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float distancePerStep = 10.0f;
    public PlayerController otherPlayer;

    public EFFECT currentOrder;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private float speed = 0.1f;

    public bool isPlaying = false;
    public bool isMisleaded = false;
    public bool isProtected = false;
    public bool isBeingControlled = false;

    public int playerNumber;

    public int turnsToSkip = 2;
    public int turnsToReverse = 2;
    public int turnsToProtect = 2;

    public GameManager manager;

    public Card[] cards;
    public GameObject[] spawners;
    public int remainingCards;

    public GameObject selectedCard;
    public int selectedCardSelectionCount = 0;

    public int energy = 100;


	// Use this for initialization
	void Start ()
    {
        currentOrder = EFFECT.NONE;
        cards = new Card[4];
        remainingCards = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isPlaying == false && !isBeingControlled)
        {
            return;
        }

        if (selectedCardSelectionCount == 2 && !isBeingControlled)
        {
            ActivateCard();
        }
                
        switch (currentOrder)
        {
            case EFFECT.UP:
                {
                    if (isMisleaded)
                    {
                        MoveDown();
                    }
                    else
                    {
                        MoveUp();
                    }
                break;
                }

            case EFFECT.DOWN:
                {
                    if (isMisleaded)
                    {
                        MoveUp();
                    }
                    else
                    {
                        MoveDown();
                    }
                }
                break;

            case EFFECT.RIGHT:
                {
                    if (isMisleaded)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }
                break;
                }

            case EFFECT.LEFT:
                {
                    if (isMisleaded)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                    break;
                }

            case EFFECT.FREEZE:
                {
                    Freeze();
                break;
                }

            case EFFECT.MISLEADING:
                {
                    Mislead();
                }
                break;

            case EFFECT.TRANSPORTATION:
                {
                    Transportation();
                }
                break;

            case EFFECT.PROTECTION:
                {
                    Protection();
                }
                break;

            default:
                {
                    return;
                }                
        }
    }

    public void DoOrder(EFFECT order)
    {
        currentOrder = order;
        oldPosition = transform.position;        
    }

    void MoveRight()
    {
        if (transform.position.x <= oldPosition.x + distancePerStep)
        {
            var pos = transform.position;
            pos.x += speed;
            transform.position = pos;
        }
        else
        {
            var pos = transform.position;
            pos.x = oldPosition.x + distancePerStep;
            transform.position = pos;

            currentOrder = EFFECT.NONE;
            isBeingControlled = false;
        }
    }

    void MoveLeft()
    {
        if (transform.position.x >= oldPosition.x - distancePerStep)
        {
            var pos = transform.position;
            pos.x -= speed;
            transform.position = pos;
        }
        else
        {
            var pos = transform.position;
            pos.x = oldPosition.x - distancePerStep;
            transform.position = pos;

            currentOrder = EFFECT.NONE;
            isBeingControlled = false;
        }
    }

    void MoveUp()
    {
        if (transform.position.y <= oldPosition.y + distancePerStep)
        {
            var pos = transform.position;
            pos.y += speed;
            transform.position = pos;
        }
        else
        {
            var pos = transform.position;
            pos.y = oldPosition.y + distancePerStep;
            transform.position = pos;

            currentOrder = EFFECT.NONE;
            isBeingControlled = false;
        }
    }

    void MoveDown()
    {
        if (transform.position.y >= oldPosition.y - distancePerStep)
        {
            var pos = transform.position;
            pos.y -= speed;
            transform.position = pos;
        }
        else
        {
            var pos = transform.position;
            pos.y = oldPosition.y - distancePerStep;
            transform.position = pos;

            currentOrder = EFFECT.NONE;
            isBeingControlled = false;
        }
    }

    void Freeze()
    {
        if (playerNumber == 1)
        {
            currentOrder = EFFECT.NONE;
            manager.SkipTurn(2, turnsToSkip);
        }
        else
        {
            currentOrder = EFFECT.NONE;
            manager.SkipTurn(1, turnsToSkip);
        }
    }

    void Mislead()
    {
        if (playerNumber == 1)
        {
            currentOrder = EFFECT.NONE;
            manager.MisleadPlayer(2, turnsToReverse);
        }
        else
        {
            currentOrder = EFFECT.NONE;
            manager.MisleadPlayer(1, turnsToReverse);
        }
    }

    void Transportation()
    {
        var pos1 = transform.position;
        var pos2 = otherPlayer.transform.position;

        transform.position = pos2;
        otherPlayer.transform.position = pos1;

        currentOrder = EFFECT.NONE;
    }

    void Protection()
    {
        if (playerNumber == 1)
        {
            currentOrder = EFFECT.NONE;
            manager.ProtectPlayer(1, turnsToProtect);
        }
        else
        {
            currentOrder = EFFECT.NONE;
            manager.ProtectPlayer(2, turnsToProtect);
        }
    }

    public void ActivateCard()
    {
        if (energy <= 0)
        {
            return;
        }

        var card = FindCard(selectedCard.name);
        int index = FindCardIndex(selectedCard.name);
        energy -= selectedCard.GetComponent<Card>().cost;

        if (cards[0].cardState == STATE.FRONT)
        {
            DoOrder(card.cardEffect);
        }
        else
        {
            switch (card.cardEffect)
            {
                case EFFECT.UP:
                case EFFECT.DOWN:
                case EFFECT.RIGHT:
                case EFFECT.LEFT:
                    otherPlayer.isBeingControlled = true;
                    otherPlayer.DoOrder(card.cardEffect);
                    break;
                
                case EFFECT.NONE:
                    break;
                default:
                    break;
            }
        }

        selectedCard = null;
        selectedCardSelectionCount = 0;

        remainingCards--;
        Destroy(cards[index].gameObject);
    }

    pair FindCard(string name)
    {
        for (int i = 0; i < 4; i++)
        {
            if (cards[i] == null)
            {
                continue;
            }

            //if (cards[i].gameObject == null)
            //{
            //    continue;
            //}

            if (cards[i].currentPair.cardsprite.transform.parent.name == name)
            {
                return cards[i].currentPair;
            }
        }

        return null;
    }

    int FindCardIndex(string name)
    {
        for (int i = 0; i < 4; i++)
        {
            if (cards[i] == null)
            {
                continue;
            }

            if (cards[i].currentPair.cardsprite.transform.parent.name == name)
            {
                return i;
            }
        }

        return -1;
    }
}
