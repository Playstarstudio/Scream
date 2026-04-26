/*
    AudioID structure developed by mawkeezy (https://github.com/marcus-ochoa). 
    Categorizes FMOD event paths with simple static classes for organization and x.y.z type reference.
*/

public sealed class AudioID
{
    public string Path { get; } // Read only

    private AudioID(string path)
    {
        Path = path;
    }
    
    #region Busses
    public static class Bus
    {
        public static readonly AudioID Master = new("bus:/");
        
        public static readonly AudioID gameplaySFX = new("bus:/gameplay_sfx");
        public static readonly AudioID interfaceSFX = new("bus:/ui_sfx");
        public static readonly AudioID music = new("bus:/music");
        
        public static class GameplaySFX
        {
            public static readonly AudioID ambience = new("bus:/gameplay_sfx/ambience");
            public static readonly AudioID objects = new("bus:/gameplay_sfx/objects");
            public static readonly AudioID player = new("bus:/gameplay_sfx/player");
        }
        
        public static class InterfaceSFX
        {
            public static readonly AudioID settings = new("bus:/ui_sfx/settings");
            public static readonly AudioID hud = new("bus:/ui_sfx/hud");
        }
    }
    #endregion
    
    #region Snapshots
    public static class Snapshot // various mixer states for environmental changes and pausing
    {
        public static readonly AudioID x = new("snapshot:/x");
    }
    #endregion

    #region SFX
    public static class SFX
    {
        public static class Interface // non-spatial interface sfx
        {
            public static class HUD
            {

            }
            
            public static class Settings
            {
                
            }
        }
        
        public static class Player // spatial player sfx
        {
            public static class Movement
            {
                
            }
        }
        
        public static class Environment // spatial environment sfx
        {
            public static class Ambience
            {

            }
            
            public static class Objects
            {
                
            }
        }
        
        public static class Test
        {
            public static readonly AudioID testOneShot = new("event:/tests/testOneShot");
            public static readonly AudioID testLoop = new("event:/tests/testLoop");
            public static readonly AudioID testOneShotSpatialized = new("event:/tests/testOneShotSpatialized");
        }
    }
    #endregion

    #region Music
    public static class Music
    {
        public static readonly AudioID x = new("event:/x");
    }
    #endregion
}
