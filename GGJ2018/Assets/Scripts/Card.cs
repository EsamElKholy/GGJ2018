using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public STATE cardState;
    public pair front;
    public pair back;

    public pair currentPair;

    public int cost;

    PlayerController owner;

    // Use this for initialization
    void Start ()
    {
        cardState = STATE.FRONT;
        currentPair = front;
        cost = 50;
    }

    // Update is called once per frame
    void Update ()
    {
        Flip();
	}

    public void GenerateCard(CardGenerator generator)
    {
        //  pair[] frontBack = 
        //CardGenerator cardGeneratorScript = (CardGenerator)GameObject.FindObjectsOfType<CardGenerator>()[0];
        //cardGeneratorScript.
        pair[] frontBack = generator.Generate();
        front = frontBack[0];
        back = frontBack[1];

        front.cardsprite.transform.position = transform.position;
        front.cardsprite.transform.SetParent(gameObject.transform);

        back.cardsprite.transform.position = transform.position;
        back.cardsprite.transform.SetParent(gameObject.transform);
        back.cardsprite.SetActive(false);

        currentPair = front;
    }
    

    public void Flip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cardState == STATE.FRONT)
            {
                cardState = STATE.BACK;

                front.cardsprite.SetActive(false);
                back.cardsprite.SetActive(true);

                currentPair = back;
            }
            else if (cardState == STATE.BACK)
            {
                cardState = STATE.FRONT;
                front.cardsprite.SetActive(true);
                back.cardsprite.SetActive(false);

                currentPair = front;
            }
        }
    }

    public void DoOrder()
    {
        switch (currentPair.cardEffect)
        {
            case EFFECT.UP:                
            case EFFECT.DOWN:
            case EFFECT.RIGHT:
            case EFFECT.LEFT:
            case EFFECT.TRANSPORTATION:
                {
                    if (cardState == STATE.FRONT)
                    {
                        owner.DoOrder(currentPair.cardEffect);
                    }
                    else
                    {
                        owner.otherPlayer.DoOrder(currentPair.cardEffect);
                    }
                }
                break;

            case EFFECT.FREEZE:
            case EFFECT.MISLEADING:
            case EFFECT.PROTECTION:
                {
                    owner.DoOrder(currentPair.cardEffect);
                }
                break;

            case EFFECT.NONE:
                break;
            default:
                break;
        }
    }

    public void SetPlayer(PlayerController owner)
    {
        this.owner = owner;
    }
}
