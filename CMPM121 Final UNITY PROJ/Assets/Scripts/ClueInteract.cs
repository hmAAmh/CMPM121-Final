using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using FMODUnity;

public class ClueInteract : MonoBehaviour
{
    [Header("References")]   

        // public Material mat1;
        public GameObject clue;
        [SerializeField] private GameObject clueManagerGameObject;
        private ClueManager clueManager;
        //private ScriptOne clueManagerScr;

        [SerializeField] private GameObject radialIndicatorObject;
        private RadialIndicator radialIndicator;

    [Header("Glow Effect")]   

        private PostProcessVolume ppVolume;
        private Bloom bloom;

    [Header("Proximity Detection")]   

        [SerializeField]
        private float detectionDistanceRange;
        
        public bool litUp = false;
        public bool closeEnough = false;
        public bool isFacing = false;
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Transform playerLight;

        private Vector3 dirFromPlayertoClue;

        public float fadeToRedAmount = 0f;

        public float fadingSpeed = 0.08f;

        public float detectionDotProductMin;
        public float detectionDotProductMax;

        public string myTag;

    [Header("Particles")]    

        public GameObject particleSystem;

    [Header("Sounds")]

        public string ambLayerToPlay;

    // Start is called before the first frame update
    void Start()
    {
        //make sure the color is reset
        gameObject.GetComponent<Renderer>().sharedMaterial.color = new Color(0.2f, 0.2f, 0.2f);

        //assign glow effect variables
        ppVolume = GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out bloom);
        clueManager = clueManagerGameObject.GetComponent<ClueManager>();
        radialIndicator = radialIndicatorObject.GetComponent<RadialIndicator>();

        //grabs tag of object
        myTag = clue.tag;
        // Debug.Log(myTag);
        // StartCoroutine(lightUp());
     
        // Debug.Log(bloom.intensity.value);

        particleSystem.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        // dirFromPlayertoClue = (this.transform.position - playerLight.transform.position).normalized;
        // float dotProd = Vector3.Dot(dirFromPlayertoClue, playerLight.transform.forward);

        // Debug.Log(dotProd);
        
        if (litUp == false){
            checkRuneTrigger();
        }
    }

    private void checkRuneTrigger(){
        //check the proximity to the player
        if (Vector3.Distance(player.position, this.transform.position) <= detectionDistanceRange){
            closeEnough = true;
            
        }
        else {
            closeEnough = false;
        }

        //calculate where the player is facing
        dirFromPlayertoClue = (this.transform.position - playerLight.transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromPlayertoClue, playerLight.transform.forward);

        if((dotProd > detectionDotProductMin) && (dotProd < detectionDotProductMax)){
            isFacing = true;
        }
        else {
            isFacing = false;
        }

        if(closeEnough && isFacing == true){
            radialIndicator.currentClueTag = myTag;
            radialIndicator.shouldUpdate = true;
            radialIndicator.radialIndicatorUI.SetActive(true);
            litUp = true;
            clueManager.numActive++;

            particleSystem.GetComponent<ParticleSystem>().enableEmission = true;
            Debug.Log("ACTIVATED RUNE! " + clueManager.numActive + " / 5");

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/clueFound");
            FMOD.Studio.EventInstance ambLayer = FMODUnity.RuntimeManager.CreateInstance("event:/AMB/clueLayer1");
            ambLayer.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            ambLayer.start();

            
            // StartCoroutine(LightUp());
        }
        // else {
        //     radialIndicator.shouldUpdate = false;
        // }
    }

    public void startLightUp(){
        StartCoroutine(LightUp());
        
    }

    IEnumerator LightUp(){ //changes intensity of glow effect over time
        
        StartCoroutine(FadeToRed());
        clueManager.clueOneOn = true;

        float bloomVal = bloom.intensity.value;
        for(float intensity = bloomVal; intensity < 10; intensity++){
            bloom.intensity.value = intensity;
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    //Coroutine to slowly fade to desired color
    IEnumerator FadeToRed(){
        for (float i = 0.2f; i <= 3f; i += 0.15f){
            Color c = gameObject.GetComponent<Renderer>().sharedMaterial.color;
            c.r += i;
            c.g -= 0.01f;
            c.b -= 0.01f;

            gameObject.GetComponent<Renderer>().sharedMaterial.color = c;

            // Debug.Log("working");

            yield return new WaitForSeconds(fadingSpeed);
        }
    }
}
