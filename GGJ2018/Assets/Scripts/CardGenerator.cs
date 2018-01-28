using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pair
{
    public EFFECT cardEffect;
    public GameObject cardsprite;
}

public class CardGenerator : MonoBehaviour
{
    public EFFECT[] effects;
    public GameObject[] spriteofcards;
    

   public pair[] Generate()
    {
        int indexFront = Random.Range(0, spriteofcards.Length);

        EFFECT currentEffectFront = effects[indexFront];
        GameObject currentCardsspriteFront = spriteofcards[indexFront];
        GameObject cloneF = Instantiate(currentCardsspriteFront, new Vector3(), Quaternion.identity);

        pair front = new pair();
        front.cardEffect = currentEffectFront;
        front.cardsprite = cloneF;

        int indexBack = Random.Range(0, spriteofcards.Length);

        while (indexFront == indexBack)
        {
            indexBack = Random.Range(0, spriteofcards.Length);
        }

        EFFECT currentEffectBack = effects[indexBack];
        GameObject currentCardsspriteBack = spriteofcards[indexBack];
        GameObject cloneB = Instantiate(currentCardsspriteBack, new Vector3(), Quaternion.identity);

        pair back = new pair(); 
        back.cardEffect = currentEffectBack;
        back.cardsprite = cloneB;

        pair[] HoleCard = { front, back };
        return HoleCard;
    }  

}
