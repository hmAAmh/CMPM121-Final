using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ClueInteract : MonoBehaviour
{
    public Material mat1;
    public GameObject clue;
    [SerializeField]
    private GameObject clueManagerGameObject;

    private ClueManager clueManager;

    //variables for glow effect
    private PostProcessVolume ppVolume;
    private Bloom bloom;

    //proximity detection variables
    [SerializeField]
    private float detectionRange;
    
    public bool closeEnough = false;
    public bool isFacing = false;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform playerLight;

    private Vector3 dirFromPlayertoClue;

    // Start is called before the first frame update
    void Start()
    {
        //assign glow effect variables
        ppVolume = GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out bloom);
        clueManager = clueManagerGameObject.GetComponent<ClueManager>();
     
        // Debug.Log(bloom.intensity.value);
    }

    // Update is called once per frame
    void Update()
    {
        //check the proximity to the player
        if (Vector3.Distance(player.position, this.transform.position) <= detectionRange){
            closeEnough = true;
            
        }
        else {
            closeEnough = false;
        }

        //calculate where the player is facing
        dirFromPlayertoClue = (this.transform.position - playerLight.transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromPlayertoClue, playerLight.transform.forward);

        if(dotProd > 0.7){
            isFacing = true;
        }
        else {
            isFacing = false;
        }

        if(closeEnough && isFacing == true){
            StartCoroutine(lightUp());
        }
        

        // Debug.Log("Player: " + player.position);
        // Debug.Log("Clue: " + transform.position);
        // if (closeEnough == true){
        //     Debug.Log("In range!");
        // }
        // else {
        //     Debug.Log("Not in range!");
        // }
    }

    // void OnMouseDown(){ //swap materials and turn on lightup 
    //     if (closeEnough == false){
    //         return;
    //     }
    //     else {
    //         
    //         StartCoroutine(lightUp());
    //         clueManager.clueOneOn = true;
    //         Debug.Log("Clue One Status: " + clueManager.clueOneOn);
    //     }
  
    // }



    IEnumerator lightUp(){ //changes intensity of glow effect over time
        gameObject.GetComponent<MeshRenderer>().material = mat1;
        clueManager.clueOneOn = true;

        float bloomVal = bloom.intensity.value;
        for(float intensity = bloomVal; intensity < 20; intensity++){
            bloom.intensity.value = intensity;
            yield return new WaitForSeconds(.3f);
        }
    }
}
