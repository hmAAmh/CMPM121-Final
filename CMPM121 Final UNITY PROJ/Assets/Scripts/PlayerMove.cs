using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]    

        public float moveSpeed;

        public Transform orientation;
    
        float horzInput;
        float vertInput;
        bool playerIsMoving;

        Vector3 moveDirection;

        Rigidbody rb;

    [Header("HeadBob")]    

        public float bobAmountIdle;
        public float bobAmountActive;
        public float bobTimeOffset;
        public float bobLerpTime;

        public Transform camHolder;

        float curBob;
        float curBobLerpTime;

        bool bobIsIdle;
        bool bobIsActive;

    // Audio
    [Header("Audio")] 

        FMOD.Studio.EventInstance footStepsSFX;

        bool footstepsPlaying;

    Vector3 bobRotation;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        curBob = bobAmountIdle;
        
        footStepsSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/footsteps");
    }

    private void Update()
    {
        MyInput();
        HeadBob();
        FootstepPlay();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horzInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        playerIsMoving = (horzInput != 0 || vertInput != 0);
    }

    private void MovePlayer()
    {   
        moveDirection = orientation.forward * vertInput + orientation.right * horzInput;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    private void HeadBob()
    {
        if(!playerIsMoving)
        {
            BobLerp(ref bobIsIdle, ref bobIsActive, bobAmountIdle);
        }
        else
        {
            BobLerp(ref bobIsActive, ref bobIsIdle, bobAmountActive);
        }
        bobRotation.Set(Mathf.Cos(Time.frameCount / bobTimeOffset) * curBob, 0, Mathf.Sin(Time.frameCount / bobTimeOffset) * curBob);
        
        camHolder.rotation = Quaternion.Euler(bobRotation);
    }

    private void BobLerp(ref bool toCheckPositive, ref bool toCheckNegative, float lerpDest)
    {
            if(!toCheckPositive)
            {
                toCheckPositive = true;
                toCheckNegative = false;

                curBobLerpTime = 0f;
            }
            curBob = Mathf.Lerp(curBob, lerpDest, curBobLerpTime / bobLerpTime);
            curBobLerpTime += Time.deltaTime;
    }

    private void FootstepPlay()
    {
        if(!footstepsPlaying && playerIsMoving)
        {
            footStepsSFX.start(); 
            footstepsPlaying = true;
        }
        else if(footstepsPlaying && !playerIsMoving)
        {
            footStepsSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //footStepsSFX.release();
            footstepsPlaying = false;
        }
    }
}
