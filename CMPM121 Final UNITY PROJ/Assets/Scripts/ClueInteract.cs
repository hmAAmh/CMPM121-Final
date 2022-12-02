using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ClueInteract : MonoBehaviour
{
    public Material mat1;
    public GameObject clue;
    [SerializeField]
    GameObject clueManagerGameObject;

    private ClueManager clueManager;

    //variables for glow effect
    private PostProcessVolume ppVolume;
    private Bloom bloom;

    //proximity detection variables
    public float detectionRange;
    public bool closeEnough = false;
    public Transform playerLocation;

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
        if (Vector3.Distance(playerLocation.position, transform.position) <= detectionRange){
            closeEnough = true;
        }
        else {
            closeEnough = false;
        }

        // Debug.Log("Player: " + playerLocation.position);
        // Debug.Log("Clue: " + transform.position);
        // if (closeEnough == true){
        //     Debug.Log("In range!");
        // }
        // else {
        //     Debug.Log("Not in range!");
        // }
    }

    void OnMouseDown(){ //swap materials and turn on lightup 
        if (closeEnough == false){
            return;
        }
        else {
            gameObject.GetComponent<MeshRenderer>().material = mat1;
            StartCoroutine(lightUp());
            clueManager.clueOneOn = true;
            Debug.Log("Clue One Status: " + clueManager.clueOneOn);
        }
  
    }

    IEnumerator lightUp(){ //changes intensity of glow effect over time
        float bloomVal = bloom.intensity.value;
        for(float intensity = bloomVal; intensity < 20; intensity++){
            bloom.intensity.value = intensity;
            yield return new WaitForSeconds(.3f);
        }
    }
}
