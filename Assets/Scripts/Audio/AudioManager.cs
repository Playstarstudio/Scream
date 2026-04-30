/*
    AudioManager developed by mawkeezy (https://github.com/marcus-ochoa) and jazzberryfields (https://github.com/jazzberry-jam).
*/

using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public Scene oldScene;

    private static Dictionary<string, EventInstance> registry;

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    private void Awake()
    {
        // Singleton Setup
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        oldScene = SceneManager.GetActiveScene();
        registry = new Dictionary<string, EventInstance>();
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    void OnSceneChange(Scene oldScene, Scene newScene)
    {
        KillAllBusInstances(AudioID.Bus.Master);
        Debug.Log($"AudioManager: Scene Change from {oldScene.name} to {newScene.name}");
        HandleMusicAmbienceChange(newScene);
        PlayOneShot(AudioID.SFX.Interface.room_transition);
    }

    /// <summary>
    /// Creates and returns an FMOD Event Instance and adds it to a list of all instances
    /// </summary>
    /// <param name="instance">FMOD Event Instance
    private bool CreateEventInstance(string path, out EventInstance instance)
    {
        FMOD.RESULT res = RuntimeManager.StudioSystem.getEvent(path, out _);
        if (res != FMOD.RESULT.OK)
        {
            Debug.LogWarning("[AudioManager] Invalid FMOD path: " + path);
            instance = default;
            return false;
        }

        instance = RuntimeManager.CreateInstance(path);
        return true;
    }

    /// <summary>
    /// Set the volume of an audio bus (0-1 range, acts as scalar to volume set in FMOD)
    /// </summary>
    /// <param name="id">Audio bus ID to set the volume of</param>
    /// <param name="val">New volume value to set (0-1 range)</param>
    public void SetBusVolume(AudioID id, float val)
    {
        if (val < 0.0f || val > 1.0f)
        {
            Debug.LogWarning("[AudioManager] Volume value out of range, val: " + val);
            return;
        }

        Bus bus = RuntimeManager.GetBus(id.Path);
        bus.setVolume(val);

        return;
    }

    /// <summary>
    /// Check to see if a certain instance exists in the registry
    /// </summary>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    public bool DoesInstanceExist(string key)
    {
        return registry.ContainsKey(key);
    }

    /// <summary>
    /// Return an audio instance based on the key
    /// </summary>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    public EventInstance GetInstance(string key)
    {
        if (!registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Key not registered: " + key);
            return default;
        }

        return registry[key];
    }

    /// <summary>
    /// Play the FMOD event associated with the passed audio ID
    /// </summary>
    /// <param name="id">Audio ID to play (ex. AudioID.SFX.Grapple)</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    public void PlayOneShot(AudioID id, GameObject obj = null)
    {
        if (!CreateEventInstance(id.Path, out EventInstance instance)) return;
        if (obj != null) RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        instance.release();
    }

    /// <summary>
    /// Play the FMOD event associated with the passed audio ID
    /// </summary>
    /// <param name="id">Audio ID to play (ex. AudioID.SFX.Grapple)</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Values of parameters to set by passed names</param>
    public void PlayOneShot(AudioID id, string[] paramNames, float[] paramVals, GameObject obj = null)
    {
        if (!CreateEventInstance(id.Path, out EventInstance instance)) return;
        if (obj != null) RuntimeManager.AttachInstanceToGameObject(instance, obj);
        SetEventInstanceParameters(instance, paramNames, paramVals);
        instance.start();
        instance.release();
    }

    /// <summary>
    /// Play the FMOD event associated with the passed audio ID
    /// </summary>
    /// <param name="id">Audio ID to play (ex. AudioID.SFX.Grapple)</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Values of parameters to set by passed names</param>
    public void PlayOneShot(AudioID id, string[] paramNames, string[] paramVals, GameObject obj = null)
    {
        if (!CreateEventInstance(id.Path, out EventInstance instance)) return;
        if (obj != null) RuntimeManager.AttachInstanceToGameObject(instance, obj);
        SetEventInstanceParameters(instance, paramNames, paramVals);
        instance.start();
        instance.release();
    }

    /// <summary>
    /// Generate a newly created FMOD event instance assigned to passed in key
    /// </summary>
    /// <param name="id">Audio ID to create an instance of (ex. AudioID.SFX.Grapple)</param>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    public void GenerateAudioInstance(AudioID id, string key, GameObject obj = null)
    {
        // Debug.Log(registry);

        if (registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Killing and replacing already registered key: " + key);
            KillAudioInstance(key);
            return;
        }

        if (!CreateEventInstance(id.Path, out EventInstance instance)) return;
        if (obj != null) RuntimeManager.AttachInstanceToGameObject(instance, obj);

        registry.Add(key, instance);
        return;
    }

    /// <summary>
    /// Generate a newly created FMOD event instance assigned to passed in key
    /// </summary>
    /// <param name="id">Audio ID to create an instance of (ex. AudioID.SFX.Grapple)</param>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Values of parameters to set by passed names</param>
    public void GenerateAudioInstance(AudioID id, string key, string[] paramNames, float[] paramVals, GameObject obj = null)
    {
        if (registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Killing and replacing already registered key: " + key);
            KillAudioInstance(key);
            return;
        }
        if (!CreateEventInstance(id.Path, out EventInstance instance)) return;
        if (obj != null) RuntimeManager.AttachInstanceToGameObject(instance, obj);
        SetEventInstanceParameters(instance, paramNames, paramVals);
        registry.Add(key, instance);
        return;
    }

    /// <summary>
    /// Generate a newly created FMOD event instance assigned to passed in key
    /// </summary>
    /// <param name="id">Audio ID to create an instance of (ex. AudioID.SFX.Grapple)</param>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Values of parameters to set by passed names</param>
    public void GenerateAudioInstance(AudioID id, string key, string[] paramNames, string[] paramVals, GameObject obj = null)
    {
        if (registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Killing and replacing already registered key: " + key);
            KillAudioInstance(key);
            return;
        }
        if (!CreateEventInstance(id.Path, out EventInstance instance)) return;
        if (obj != null) RuntimeManager.AttachInstanceToGameObject(instance, obj);
        SetEventInstanceParameters(instance, paramNames, paramVals);
        registry.Add(key, instance);
        return;
    }

    /// <summary>
    /// Generate a newly created FMOD event instance assigned to passed in key and start playing the instance
    /// </summary>
    /// <param name="id">Audio ID to create an instance of (ex. AudioID.SFX.Grapple)</param>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="timelinePos">FMOD timeline position to play the instance at (in milliseconds)</param>
    public void PlayGenerateAudioInstance(AudioID id, string key, GameObject obj = null, int timelinePos = -1)
    {
        GenerateAudioInstance(id, key, obj);
        StartAudioInstance(key, timelinePos);
        Debug.Log(key);
        return;
    }

    /// <summary>
    /// Generate a newly created FMOD event instance assigned to passed in key and start playing the instance
    /// </summary>
    /// <param name="id">Audio ID to create an instance of (ex. AudioID.SFX.Grapple)</param>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Values of parameters to set by passed names</param>
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="timelinePos">FMOD timeline position to play the instance at (in milliseconds)</param>
    public void PlayGenerateAudioInstance(AudioID id, string key, string[] paramNames = null, float[] paramVals = null, GameObject obj = null, int timelinePos = -1)
    {
        GenerateAudioInstance(id, key, paramNames, paramVals, obj);
        StartAudioInstance(key, timelinePos);
        Debug.Log(key);
        return;
    }

    /// <summary>
    /// Generate a newly created FMOD event instance assigned to passed in key and start playing the instance
    /// </summary>
    /// <param name="id">Audio ID to create an instance of (ex. AudioID.SFX.Grapple)</param>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Values of parameters to set by passed names</param>    
    /// <param name="obj">Game object to attach audio to if 3D</param>
    /// <param name="timelinePos">FMOD timeline position to play the instance at (in milliseconds)</param>
    public void PlayGenerateAudioInstance(AudioID id, string key, string[] paramNames = null, string[] paramVals = null, GameObject obj = null, int timelinePos = -1)
    {
        GenerateAudioInstance(id, key, paramNames, paramVals, obj);
        StartAudioInstance(key, timelinePos);
        Debug.Log(key);
        return;
    }

    /// <summary>
    /// Start playing an FMOD event instance identified by key
    /// </summary>
    /// <param name="key">The key that references the registered event instance</param>
    /// <param name="timelinePos">FMOD timeline position to play the instance at (in milliseconds)</param>
    public void StartAudioInstance(string key, int timelinePos = -1)
    {
        if (!registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Key not registered: " + key);
            return;
        }
        EventInstance instance = registry[key];
        if (timelinePos != -1) { instance.setTimelinePosition(timelinePos); }
        instance.start();
    }

    /// <summary>
    /// Stop playing an FMOD event instance identified by key
    /// </summary>
    /// <param name="key">The key that references the registered event instance</param>
    public void StopAudioInstance(string key)
    {
        if (!registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Key not registered: " + key);
            return;
        }
        EventInstance instance = registry[key];

        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    public void HandleMusicAmbienceChange(Scene newScene)
    {
        AudioID newMusic = AudioID.SceneToMusicAmbienceMap[newScene.name]?["music"];
        AudioID newAmbience = AudioID.SceneToMusicAmbienceMap[newScene.name]?["ambience"];
        // Debug.Log($"{newAmbience.Path}");
        if (oldScene.name == null)
        {
            Debug.Log("idk");
        }
        if (oldScene.name != null)
        {
            string oldSceneName = oldScene.name;

            AudioID oldMusic = AudioID.SceneToMusicAmbienceMap[oldSceneName]["music"];
            AudioID oldAmbience = AudioID.SceneToMusicAmbienceMap[oldSceneName]["ambience"];
            int oldMusicProgress;
            int oldAmbienceProgress;

            registry[$"mus_{oldMusic}"].getTimelinePosition(out oldMusicProgress);
            registry[$"amb_{oldAmbience}"].getTimelinePosition(out oldAmbienceProgress);
            AudioID.CurrentMusicProgress[oldMusic] = oldMusicProgress;
            AudioID.CurrentAmbienceProgress[oldAmbience] = oldAmbienceProgress;
        }

        if (newMusic.Path != "") { PlayGenerateAudioInstance(newMusic, $"mus_{newMusic.Path}", null, AudioID.CurrentMusicProgress[newMusic]); }
        if (newAmbience.Path != "") { PlayGenerateAudioInstance(newAmbience, $"amb_{newAmbience.Path}", GameObject.Find("Character"), AudioID.CurrentAmbienceProgress[newAmbience]); }

        // TODO: This doesn't work because the old scene does not save between audio manager instances
        oldScene = newScene;
    }

    /// <summary>
    /// Kill an FMOD event instance identified by key and remove it from registry
    /// </summary>
    /// <param name="key">The key that references the registered event instance</param>
    public void KillAudioInstance(string key)
    {
        if (!registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Key not registered: " + key);
            return;
        }
        EventInstance instance = registry[key];
        instance.release();
        registry.Remove(key);
    }

    /// <summary>
    /// Stop playing and kill an FMOD event instance identified by key and remove it from registry
    /// </summary>
    /// <param name="key">The key that references the registered event instance</param>
    public void StopKillAudioInstance(string key)
    {
        StopAudioInstance(key);
        KillAudioInstance(key);
    }

    /// <summary>
    /// Destroty all audio instances on an audio bus
    /// </summary>
    /// <param name="id">Audio bus ID to kill all instances on</param>
    public void KillAllBusInstances(AudioID id)
    {
        Bus bus = RuntimeManager.GetBus(id.Path);
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    /// <summary>
    /// Pause and resume a given audio bus (this currently does not work :P)
    /// </summary>
    /// <param name="id">Audio ID of the bus to pause it</param>
    public void ToggleBusPause(AudioID id)
    {

        if (id.GetType() != typeof(AudioID.Bus))
        {
            Debug.LogWarning("[AudioManager] AudioID given is not a bus. You passed in: " + id.GetType());
            return;
        }

        Bus bus = RuntimeManager.GetBus(id.Path);

        if (bus.getPaused(out _) == FMOD.RESULT.OK)
        {
            bus.setPaused(true);
            Debug.Log($"[AudioManager] {id.Path} now paused");
        }
        else
        {
            bus.setPaused(false);
            Debug.Log($"[AudioManager] {id.Path} now resumed");
        }
    }

    /// <summary>
    /// Pause an FMOD event instance identified by key
    /// </summary>
    /// <param name="key">The key that references the registered event instance</param>
    public void ToggleInstancePause(string key)
    {
        if (!registry.ContainsKey(key))
        {
            Debug.LogWarning("[AudioManager] Key not registered: " + key);
            return;
        }
        EventInstance instance = registry[key];

        if (instance.getPaused(out _) == FMOD.RESULT.OK)
        {
            instance.setPaused(true);
            Debug.Log($"[AudioManager] {key} now paused");
        }
        else
        {
            instance.setPaused(false);
            Debug.Log($"[AudioManager] {key} now resumed");
        }
    }

    /// <summary>
    /// Set parameters of an FMOD event instance identified by key
    /// </summary>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">Float values of parameters to set by passed names</param>
    public void SetParametersAudioInstance(string key, string[] paramNames, float[] paramVals)
    {
        EventInstance instance = registry[key];
        SetEventInstanceParameters(instance, paramNames, paramVals);
    }

    /// <summary>
    /// Set parameters of an FMOD event instance identified by key
    /// </summary>
    /// <param name="key">String key that references this instance (ex. "grappleLoop")</param>
    /// <param name="paramNames">Names of parameters to set</param>
    /// <param name="paramVals">String values of parameters to set by passed names</param>
    public void SetParametersAudioInstance(string key, string[] paramNames, string[] paramVals)
    {
        EventInstance instance = registry[key];
        SetEventInstanceParameters(instance, paramNames, paramVals);
    }

    private void SetEventInstanceParameters(EventInstance instance, string[] paramNames, float[] paramVals)
    {
        if (paramNames.Length != paramVals.Length)
        {
            Debug.LogWarning(
                "[AudioManager] Number of param names not equal to number of vals passed");
            return;
        }
        for (int i = 0; i < paramNames.Length; i++)
        {
            instance.setParameterByName(paramNames[i], paramVals[i]);
        }
    }

    private void SetEventInstanceParameters(EventInstance instance, string[] paramNames, string[] paramVals)
    {
        if (paramNames.Length != paramVals.Length)
        {
            Debug.LogWarning(
                "[AudioManager] Number of param names not equal to number of vals passed");
            return;
        }
        for (int i = 0; i < paramNames.Length; i++)
        {
            instance.setParameterByNameWithLabel(paramNames[i], paramVals[i]);
        }
    }

    public void SetGlobalParameter(string paramName, float paramVal)
    {
        RuntimeManager.StudioSystem.setParameterByName(paramName, paramVal);
    }

    public void SetGlobalParameter(string paramName, string paramVal)
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(paramName, paramVal);
    }
}

