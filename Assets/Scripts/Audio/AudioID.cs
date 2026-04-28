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
        public static class Environment // spatial environment sfx
        {            
            public static class Stinger
            {
                public static readonly AudioID cabin_shaking = new("event:/environment/ambience/cabin_shaking");
                public static readonly AudioID flash = new("event:/environment/ambience/flash");
                public static readonly AudioID ritual_powering_up = new("event:/environment/ambience/ritual_powering_up");
                public static readonly AudioID scary_stinger = new("event:/environment/ambience/scary_stinger");
            }
            
            public static class Objects
            {
                public static class Entity
                {
                    public static readonly AudioID idle = new("event:/environment/objects/entity/idle");
                }
                
                public static class Tentacle
                {
                    public static readonly AudioID idle = new("event:/environment/objects/tentacle/slithering");
                }
            }
        }
        
        public static class Ambience
        {
            public static readonly AudioID general = new("event:/ambience/general");
            public static readonly AudioID foyer = new("event:/ambience/foyer");
            public static readonly AudioID outdoors = new("event:/ambience/outdoors");
        }
        
        public static class Interface // non-spatial interface sfx
        {
            public static class HUD
            {
                public static class Inventory
                {
                    public static readonly AudioID close = new("event:/ui/hud/inventory/close");
                    public static readonly AudioID hover = new("event:/ui/hud/inventory/hover");
                    public static readonly AudioID open = new("event:/ui/hud/inventory/open");
                    public static readonly AudioID place = new("event:/ui/hud/inventory/place");
                    public static readonly AudioID select = new("event:/ui/hud/inventory/select");
                    public static readonly AudioID unselect = new("event:/ui/hud/inventory/unselect");
                }
                
                public static readonly AudioID game_over = new("event:/ui/hud/game_over");
                public static readonly AudioID room_transition = new("event:/ui/hud/room_transition");
            }
            
            public static class Settings
            {
                public static readonly AudioID confirm = new("event:/ui/hud/settings/confirm");
                public static readonly AudioID deny = new("event:/ui/hud/settings/deny");
            }
        }
        
        public static class Player // spatial player sfx
        {
            public static class Interact
            {
                public static class Generic
                {
                    public static readonly AudioID highlight = new("event:/player/interact/generic/highlight");
                }
                
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
                
                public static class Ritual_Circle
                {
                    public static readonly AudioID place_hand = new("event:/player/interact/ritual_circle/place_hand");
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
                
                public static class Typewriter
                {
                    public static readonly AudioID pull = new("event:/player/interact/typewriter/type");
                }
            }
            
            public static class Movement
            {
                public static readonly AudioID footsteps = new("event:/player/movement/footsteps");
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
        public static readonly AudioID credits = new("event:/music/credits");
        public static readonly AudioID exploration = new("event:/music/exploration");
        public static readonly AudioID fakeout = new("event:/music/fakeout");
        public static readonly AudioID title = new("event:/music/title");
    }
    #endregion
}
