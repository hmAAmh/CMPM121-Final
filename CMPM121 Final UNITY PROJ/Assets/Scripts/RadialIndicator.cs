using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadialIndicator : MonoBehaviour
{
    [SerializeField] private float indicatorTimer = 0f;
    [SerializeField] private float maxIndicatorTimer = 1.0f;

    public GameObject radialIndicatorUI;

    public Image radialIndicator;

    // [SerializeField] private GameObject clue1;
    // private ClueInteract clueInteract1;
    // private bool clue1LitUp;
    // [SerializeField] private GameObject clue2;
    // private ClueInteract clueInteract2;
    // private bool clue2LitUp;
    // [SerializeField] private GameObject clue3;
    // private ClueInteract clueInteract3;
    // private bool clue3LitUp;
    // [SerializeField] private GameObject clue4;
    // private ClueInteract clueInteract4;
    // private bool clue4LitUp;
    // [SerializeField] private GameObject clue5;
    // private ClueInteract clueInteract5;
    // private bool clue5LitUp;

    // [SerializeField] private UnityEvent myEvent = null;

    public bool shouldUpdate = false; 

    private GameObject[] clues;
    private GameObject clueToLightObject;
    private ClueInteract clueToLight;
    public string currentClueTag;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldUpdate == true){
                

            indicatorTimer += Time.deltaTime;
        
            radialIndicator.fillAmount = indicatorTimer;

            if (indicatorTimer >= 1.0f){
                indicatorTimer = 0f; 
                radialIndicator.fillAmount = 0f;
                radialIndicatorUI.SetActive(false);
                shouldUpdate = false;
                startLightUp();
            }
        }
        
        // else {
        //     if(shouldUpdate){
        //         indicatorTimer += Time.deltaTime;
        //         radialIndicatorUI.fillAmount = indicatorTimer;

        //         if  (indicatorTimer >= maxIndicatorTimer){
        //             indicatorTimer = maxIndicatorTimer;
        //             radialIndicatorUI.fillAmount = maxIndicatorTimer;
        //             radialIndicatorUI.enabled = false;
        //             shouldUpdate = false;
        //         }
        //     }
        // }
    }

    // private ClueInteract whichClue(){
    //     clue1LitUp = clueInteract1.litUp;
    //     clue2LitUp = clueInteract2.litUp;
    //     clue3LitUp = clueInteract3.litUp;
    //     clue4LitUp = clueInteract4.litUp;
    //     clue5LitUp = clueInteract5.litUp;
    // }

    public void startLightUp(){
        clues = GameObject.FindGameObjectsWithTag(currentClueTag);
        clueToLightObject = clues[0];
        clueToLight = clueToLightObject.GetComponent<ClueInteract>();
        clueToLight.startLightUp();
    }

}
