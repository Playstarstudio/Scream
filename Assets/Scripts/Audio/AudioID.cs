/*
    AudioID structure developed by mawkeezy (https://github.com/marcus-ochoa). 
    Categorizes FMOD event paths with simple static classes for organization and x.y.z type reference.
*/

using System.Collections.Generic;

public sealed class AudioID
{
    public string Path { get; } // Read only

    private AudioID(string path)
    {
        Path = path;
    }
    
    public static readonly Dictionary<string, Dictionary<string, AudioID>> SceneToMusicAmbienceMap = new()
    {
        { "AltarRoom",      new(){{"music", Music.exploration}, {"ambience", SFX.Ambience.altar}} },
        { "Bedroom",        new(){{"music", Music.exploration}, {"ambience", SFX.Ambience.bedroom}} },
        { "Foyer",          new(){{"music", Music.exploration}, {"ambience", SFX.Ambience.foyer}} },
        { "Kitchen",        new(){{"music", Music.exploration}, {"ambience", SFX.Ambience.kitchen}} },
        { "TentacleRoom",   new(){{"music", Music.eldritch},    {"ambience", SFX.Ambience.tentacle}} },
        { "MainMenu",       new(){{"music", Music.title},       {"ambience", new("")}} },
        { "CreditsScene",   new(){{"music", Music.credits},     {"ambience", new("")}} },
        { "Outside",        new(){{"music", new("")},           {"ambience", SFX.Ambience.outdoors}} }
    };
    
    public static readonly Dictionary<string, string> SceneToRoomMap = new()
    {
        {"Outside", "outdoors"},
        {"Bedroom", "bedroom"},
        {"Foyer", "foyer"},
        {"Kitchen", "kitchen"},
        {"AltarRoom", "altar"},
        {"TentacleRoom", "tentacle"}
    };
    
    public static readonly Dictionary<AudioID, int> CurrentMusicProgress = new()
    {
        {Music.exploration, 0},
        {Music.eldritch, 0},
        {Music.title, 0}
    };
    
    public static readonly Dictionary<AudioID, int> CurrentAmbienceProgress = new()
    {
        {SFX.Ambience.altar, 0},
        {SFX.Ambience.bedroom, 0},
        {SFX.Ambience.foyer, 0},
        {SFX.Ambience.tentacle, 0},
        {SFX.Ambience.kitchen, 0},
        {SFX.Ambience.outdoors, 0}
    };
    
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
            public static readonly AudioID stinger = new("bus:/gameplay_sfx/stinger");
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
        public static readonly AudioID domestic_room_verb = new("snapshot:/domestic_room_verb");
        public static readonly AudioID altar_room_verb = new("snapshot:/altar_room_verb");
        public static readonly AudioID tentacle_room_verb = new("snapshot:/tentacle_room_verb");
        public static readonly AudioID outdoors_verb = new("snapshot:/outdoors_verb");
    }
    #endregion

    #region SFX
    public static class SFX
    {
        public static class Environment // non-spatial environment sfx
        {            
            public static class Stinger
            {
                public static readonly AudioID cabin_shaking = new("event:/environment/stinger/cabin_shaking");
                public static readonly AudioID flash = new("event:/environment/stinger/flash");
                public static readonly AudioID ritual_powering_up = new("event:/environment/stinger/ritual_powering_up");
                public static readonly AudioID scary_stinger = new("event:/environment/stinger/scary_stinger");
            }
        }
        
        public static class Ambience
        {
            public static readonly AudioID general = new("event:/ambience/general");
            public static readonly AudioID altar = new("event:/ambience/altar");
            public static readonly AudioID foyer = new("event:/ambience/foyer");
            public static readonly AudioID outdoors = new("event:/ambience/outdoors");
            public static readonly AudioID tentacle = new("event:/ambience/tentacle");
            public static readonly AudioID bedroom = new("event:/ambience/bedroom");
            public static readonly AudioID kitchen = new("event:/ambience/kitchen");
        }
        
        public static class Interface // non-spatial interface sfx
        {
            public static class Inventory
            {
                public static readonly AudioID close = new("event:/ui/inventory/close");
                public static readonly AudioID hover = new("event:/ui/inventory/hover");
                public static readonly AudioID open = new("event:/ui/inventory/open");
                public static readonly AudioID place = new("event:/ui/inventory/place");
                public static readonly AudioID select = new("event:/ui/inventory/select");
                public static readonly AudioID unselect = new("event:/ui/inventory/unselect");
            }
            
            public static class Settings
            {
                public static readonly AudioID confirm = new("event:/ui/settings/confirm");
                public static readonly AudioID deny = new("event:/ui/settings/deny");
            }
            
            public static readonly AudioID room_transition = new("event:/ui/room_transition");
            public static readonly AudioID highlight = new("event:/ui/highlight");
            public static readonly AudioID typewriter = new("event:/ui/typewriter");
        }
        
        public static class Player // spatial player sfx
        {
            public static class Interact
            {   
                public static class Amulet
                {
                    public static readonly AudioID pick_up = new("event:/player/interact/amulet/pick_up");
                    public static readonly AudioID place = new("event:/player/interact/amulet/place");
                }
                
                public static class Candle
                {
                    public static readonly AudioID place = new("event:/player/interact/candle/place");
                }
                
                public static class Door
                {
                    public static readonly AudioID knob_turn = new("event:/player/interact/door/knob_turn");
                    public static readonly AudioID locked = new("event:/player/interact/door/locked");
                    public static readonly AudioID open = new("event:/player/interact/door/open");
                }
                
                public static class Lantern
                {
                    public static readonly AudioID fill = new("event:/player/interact/lantern/fill");
                    public static readonly AudioID turn_knob_down = new("event:/player/interact/lantern/turn_knob_down");
                    public static readonly AudioID turn_knob_up = new("event:/player/interact/lantern/turn_knob_up");
                }
                
                public static class Match
                {
                    public static readonly AudioID light = new("event:/player/interact/match/light");
                    public static readonly AudioID place = new("event:/player/interact/match/place");
                }
                
                public static class Teddy_Bear
                {
                    public static readonly AudioID place = new("event:/player/interact/teddy_bear/hand_place");
                    public static readonly AudioID place_bloody = new("event:/player/interact/teddy_bear/place_bloody");
                    public static readonly AudioID slice = new("event:/player/interact/teddy_bear/slice");
                    public static readonly AudioID smear = new("event:/player/interact/teddy_bear/smear");
                }
                
                public static class Tentacle
                {
                    public static readonly AudioID pull = new("event:/player/interact/tentacle/pull");
                }
            }
            
            public static class Movement
            {
                public static readonly AudioID footsteps = new("event:/player/movement/footsteps");
            }
            
            public static readonly AudioID death = new("event:/player/death");
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
        public static readonly AudioID credits = new("event:/music/credits");
        public static readonly AudioID exploration = new("event:/music/exploration");
        public static readonly AudioID eldritch = new("event:/music/eldritch");
        public static readonly AudioID fakeout = new("event:/music/fakeout");
        public static readonly AudioID title = new("event:/music/title");
    }
    #endregion
}
