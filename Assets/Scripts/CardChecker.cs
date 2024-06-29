using System.Collections.Generic;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class CardChecker : MonoSingleton<CardChecker>
    {
        // Create a function called GetCards() that gets all the children of the object and adds them to a list
        public void GetCards()
        {
            // Create a new list of type GameObject called cards
            List<GameObject> cards = new List<GameObject>();

            // Loop through all the children of the object
            for (int i = 0; i < transform.childCount; i++)
            {
                // Add the child to the list
                cards.Add(transform.GetChild(i).gameObject);
                cards[i].name = "" + i;
            }

            Debug.Log("Number of cards: " + cards.Count);
        }
    }
}