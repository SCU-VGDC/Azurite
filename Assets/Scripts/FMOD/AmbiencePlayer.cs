using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    FMOD.Studio.EventInstance AmbienceInst;

    // Start is called before the first frame update
    void Start()
    {
        AmbienceInst = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Ambience");
        AmbienceInst.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        AmbienceInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
} 
