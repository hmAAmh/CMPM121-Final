using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [Header("Position Tracking")]  

        public Transform orientation;

    //flickering

    [Header("Light Flickering")]  

        public float flickerAmount;
        private Light lightSource;
        private float lightInitIntensity;

    void Start()
    {
        lightSource = gameObject.GetComponent<Light>();
        lightInitIntensity = lightSource.intensity;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = orientation.rotation;

        Flicker();
    }

    private void Flicker()
    {
        float lightOffset = (Random.Range(0f, lightInitIntensity) * Mathf.Abs(Mathf.Tan(Time.time))) * flickerAmount;
        // Debug.Log(lightOffset);
        lightSource.intensity = lightInitIntensity - lightOffset;
    }
}
