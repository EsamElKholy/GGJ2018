using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public PlayerController owner;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (owner.isPlaying == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
        }        
	}

    void DetectClick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.name == name)
            {
                if (transform.childCount > 0)
                {
                    var card = transform.GetChild(0).gameObject;

                    if (owner.selectedCard == null)
                    {
                        owner.selectedCardSelectionCount = 0;
                        owner.selectedCardSelectionCount++;
                    }
                    else if (owner.selectedCard.name == card.name)
                    {
                        owner.selectedCardSelectionCount++;
                    }

                    owner.selectedCard = card;
                }                
            }
        }
    }
}
