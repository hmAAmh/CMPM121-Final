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

    private PostProcessVolume ppVolume;
    private Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        ppVolume = GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out bloom);
        clueManager = clueManagerGameObject.GetComponent<ClueManager>();
     
        // Debug.Log(bloom.intensity.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        gameObject.GetComponent<MeshRenderer>().material = mat1;
        StartCoroutine(lightUp());
        clueManager.clueOneOn = true;
        Debug.Log("Clue One Status: " + clueManager.clueOneOn);
    }

    IEnumerator lightUp(){ //changes intensity over time
        float bloomVal = bloom.intensity.value;
        for(float intensity = bloomVal; intensity < 20; intensity++){
            bloom.intensity.value = intensity;
            yield return new WaitForSeconds(.3f);
        }
    }
}
