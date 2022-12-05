using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject clueOne;
    public bool clueOneOn = false;

    public int numActive;
    public int numClues;

    public GameObject doorToOpen;

    public bool allCluesFound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(numActive >= numClues && !allCluesFound)
        {
            allCluesFound = true;
            doorToOpen.GetComponent<Animator>().Play("DoorOpen");
            Debug.Log("All clues activated!");

        }
    }
}
