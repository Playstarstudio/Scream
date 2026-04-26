using System.Security.Claims;
using UnityEngine;

public class AudioTest : MonoBehaviour
{

    [field: Header("One Shot Functions")]
    public bool oneShotNoParameter = false;
    public bool oneShotFloatParameter = false;
    public bool oneShotStringParameter = false;
    public bool oneShotSpatialized = false;
    
    [field: Header("Loop Functions")]
    public bool loopStart = false;
    public bool loopEnd = false;
    public bool loopFloatParameter = false;
    public bool loopStringParameter = false;
    public bool loopFloatGlobalParameter = false;
    public bool loopStringGlobalParameter = false;
    
    [field: Header("Helper Functions")]
    public bool clearAll = false;
    public bool resetLoopParameters = false;

    // Update is called once per frame
    void Update()
    {
        if (oneShotNoParameter)
        {
            AudioManager.Instance.PlayOneShot(AudioID.SFX.Test.testOneShot);
            oneShotNoParameter = false;
        }
        
        if (oneShotFloatParameter)
        {
            AudioManager.Instance.PlayOneShot(AudioID.SFX.Test.testOneShot, new string[] {"testParamFloatOneShot"}, new float[] {1f});
            oneShotFloatParameter = false;
        }
        
        if (oneShotStringParameter)
        {
            AudioManager.Instance.PlayOneShot(AudioID.SFX.Test.testOneShot, new string[] {"testParamStringOneShot"}, new string[] {"true"});
            oneShotStringParameter = false;
        }
        
        if (oneShotSpatialized)
        {
            AudioManager.Instance.PlayOneShot(AudioID.SFX.Test.testOneShotSpatialized, GameObject.Find("Spatial Object"));
            oneShotSpatialized = false;
        }
        
        if (loopStart)
        {
            AudioManager.Instance.PlayGenerateAudioInstance(AudioID.SFX.Test.testLoop, new ("key"), null, -1);
            loopStart = false;
        }
        
        if (loopEnd)
        {
            AudioManager.Instance.StopKillAudioInstance("key");
            loopEnd = false;
        }
        
        if (loopFloatParameter)
        {
            AudioManager.Instance.SetParametersAudioInstance("key", new string[] {"testParamFloatLoop"}, new float[] {1f});
            loopFloatParameter = false;
        }
        
        if (loopStringParameter)
        {
            AudioManager.Instance.SetParametersAudioInstance("key", new string[] {"testParamStringLoop"}, new string[] {"true"});
            loopStringParameter = false;
        }
        
        // I'm just gonna assume these work lol
        
        // if (loopFloatGlobalParameter)
        // {
        //     AudioManager.Instance.SetGlobalParameter("testParamFloatGlobal", 1f);
        //     loopFloatGlobalParameter = false;
        // }
        
        // if (loopStringGlobalParameter)
        // {
        //     AudioManager.Instance.SetGlobalParameter("testParamStringGlobal", "true");
        //     loopStringGlobalParameter = false;
        // }
        
        // if (clearAll)
        // {
        //     AudioManager.Instance.ClearAllInstances();
        //     clearAll = false;
        // }
        
        if (resetLoopParameters)
        {
            AudioManager.Instance.SetParametersAudioInstance("key", new string[] {"testParamFloatLoop"}, new float[] {0f});
            AudioManager.Instance.SetGlobalParameter("testParamFloatGlobal", 0f);
            
            AudioManager.Instance.SetParametersAudioInstance("key", new string[] {"testParamStringLoop"}, new string[] {"false"});
            AudioManager.Instance.SetGlobalParameter("testParamStringGlobal", "false");
            
            resetLoopParameters = false;
        }
    }
}

