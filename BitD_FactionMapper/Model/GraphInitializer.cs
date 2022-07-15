using System.Collections.Generic;

namespace BitD_FactionMapper.Model
{
    public class GraphInitializer
    {
        private const int Billhooks = 10;
        private const int BlackDogs = 20;
        private const int Bluecoats = 30;
        private const int Brigade = 40;
        private const int Cabbies = 50;
        private const int CanalPatrol = 60;
        private const int ChurchOfEcstasy = 70;
        private const int CircleOfFlame = 80;
        private const int CityCouncil = 90;
        private const int Crows = 100;
        private const int Cyphers = 110;
        private const int DaggerIslesConsulate = 120;
        private const int DeathlandsScavengers = 130;
        private const int DimmerSisters = 140;
        private const int Dockers = 150;
        private const int FogHounds = 160;
        private const int ForgottenGods = 170;
        private const int Foundation = 180;
        private const int Gondoliers = 190;
        private const int GrayCloaks = 200;
        private const int Grinders = 210;
        private const int Hive = 220;
        private const int Horde = 230;
        private const int ImperialMilitary = 240;
        private const int InkRakes = 250;
        private const int Inspectors = 260;
        private const int IronhookPrison = 270;
        private const int IruvianConsulate = 280;
        private const int Laborers = 290;
        private const int Lampblacks = 300;
        private const int LeviathanHunters = 310;
        private const int LordScurlock = 320;
        private const int LordStrangford = 330;
        private const int Lost = 340;
        private const int MinistryOfPreservation = 350;
        private const int PathOfEchoes = 360;
        private const int RailJacks = 370;
        private const int Reconciled = 380;
        private const int RedSashes = 390;
        private const int Sailors = 400;
        private const int Servants = 410;
        private const int SeverosiConsulate = 420;
        private const int SilverNails = 430;
        private const int SkovlanConsulate = 440;
        private const int SkovlanderRefugees = 450;
        private const int Sparkwrights = 460;
        private const int SpiritWardens = 470;
        private const int UlfIronborn = 480;
        private const int Unseen = 490;
        private const int Vultures = 500;
        private const int WeepingLady = 510;
        private const int Wraiths = 520;
        private const int CoalridgeWorkhouseForemen = 900;
        private const int CoalridgeWorkhouseLaborers = 901;
        private const int CitizensOfBarrowcleft = 1001;
        private const int CitizensOfBrightstone = 1002;
        private const int CitizensOfCharhollow = 1003;
        private const int CitizensOfCharterhall = 1004;
        private const int CitizensOfCoalridge = 1005;
        private const int CitizensOfCrowsFoot = 1006;
        private const int CitizensOfTheDocks = 1007;
        private const int CitizensOfDunlough = 1008;
        private const int CitizensOfNightmarket = 1009;
        private const int CitizensOfSilkshore = 1010;
        private const int CitizensOfSixTowers = 1011;
        private const int CitizensOfWhitecrown = 1012;

        private int _id = 2000;
        private int GetNextId()
        {
            return _id++;
        }
        
        public List<Node> CreateDuskvolFactions()
        {
            var factions = new List<Node>
            {
                // Institutions
                new Node(Bluecoats, "Bluecoats", "III | Strong"),
                new Node(Brigade, "The Brigade", "II | Strong"),
                new Node(CanalPatrol, "Canal Patrol", "Subgroup of Bluecoats"),
                new Node(CityCouncil, "City Council", "V | Strong"),
                new Node(DaggerIslesConsulate, "Dagger Isles Consulate", "I | Strong"),
                new Node(ImperialMilitary, "Imperial Military", "VI | Strong"),
                new Node(Inspectors, "Inspectors", "III | Strong"),
                new Node(IronhookPrison, "Ironhook Prison", "IV | Strong"),
                new Node(IruvianConsulate, "Iruvian Consulate", "III | Strong"),
                new Node(LeviathanHunters, "Leviathan Hunters", "V | Strong"),
                new Node(LordStrangford, "Lord Strangford", "Member of Leviathan Hunters"),
                new Node(MinistryOfPreservation, "Ministry Of Preservation", "V | Strong"),
                new Node(SeverosiConsulate, "Severosi Consulate", "I | Strong"),
                new Node(SkovlanConsulate, "Skovlan Consulate", "III | Weak"),
                new Node(Sparkwrights, "Sparkwrights", "IV | Strong"),
                new Node(SpiritWardens, "Spirit Wardens", "IV | Strong"),
                
                // Labor
                new Node(Cabbies, "Cabbies", "II | Weak"),
                new Node(Cyphers, "Cyphers", "II | Strong"),
                new Node(Dockers, "Dockers", "III | Strong"),
                new Node(Foundation, "The Foundation", "IV | Strong"),
                new Node(Gondoliers, "Gondoliers", "III | Strong"),
                new Node(InkRakes, "Ink Rakes", "II | Weak"),
                new Node(Laborers, "Laborers", "III | Weak"),
                new Node(RailJacks, "Rail Jacks", "II | Weak"),
                new Node(Sailors, "Sailors", "III | Weak"),
                new Node(Servants, "Servants", "II | Weak"),
                
                // Underworld
                new Node(Billhooks, "The Billhooks", "II | Weak"),
                new Node(BlackDogs, "The Black Dogs", "I | Strong"),
                new Node(CircleOfFlame, "The Circle of Flame", "III | Strong"),
                new Node(Crows, "The Crows", "II | Weak"),
                new Node(DimmerSisters, "The Dimmer Sisters", "II | Strong"),
                new Node(FogHounds, "The Fog Hounds", "I | Weak"),
                new Node(GrayCloaks, "The Gray Cloaks", "II | Strong"),
                new Node(Grinders, "The Grinders", "II | Weak"),
                new Node(Hive, "The Hive", "IV | Strong"),
                new Node(Lampblacks, "The Lampblacks", "II | Weak"),
                new Node(LordScurlock, "Lord Scurlock", "III | Strong"),
                new Node(Lost, "The Lost", "I | Weak"),
                new Node(RedSashes, "The Red Sashes", "II | Weak"),
                new Node(SilverNails, "The Silver Nails", "III | Strong"),
                new Node(UlfIronborn, "Ulf Ironborn", "I | Strong"),
                new Node(Unseen, "The Unseen", "IV | Strong"),
                new Node(Vultures, "The Vultures", "I | Weak"),
                new Node(Wraiths, "The Wraiths", "II | Weak"),
                
                // Fringe
                new Node(ChurchOfEcstasy, "The Church of Ecstasy", "IV | Strong"),
                new Node(DeathlandsScavengers, "Deathlands Scavengers", "II | Weak"),
                new Node(ForgottenGods, "The Forgotted Gods", "III | Weak"),
                new Node(Horde, "The Horde", "III | Strong"),
                new Node(PathOfEchoes, "The Path of Echoes", "III | Strong"),
                new Node(Reconciled, "The Reconciled", "III | Strong"),
                new Node(SkovlanderRefugees, "Skovlander Refugees", "III | Weak"),
                new Node(WeepingLady, "The Weeping Lady", "II | Weak"),
                
                // Special Factions
                new Node(CoalridgeWorkhouseForemen, "Coalridge Workhouse Foremen", ""),
                new Node(CoalridgeWorkhouseLaborers, "Coalridge Workhouse Laborers", ""),
                
                // Citizens
                new Node(CitizensOfBarrowcleft, "Citizens of Barrowcleft", ""),
                new Node(CitizensOfBrightstone, "Citizens of Brightstone", ""),
                new Node(CitizensOfCharhollow, "Citizens of Charhollow", ""),
                new Node(CitizensOfCharterhall, "Citizens of Charterhall", ""),
                new Node(CitizensOfCoalridge, "Citizens of Coalridge", ""),
                new Node(CitizensOfCrowsFoot, "Citizens of Crow's Foot", ""),
                new Node(CitizensOfTheDocks, "Citizens of The Docks", ""),
                new Node(CitizensOfDunlough, "Citizens of Dunlough", ""),
                new Node(CitizensOfNightmarket, "Citizens of Nightmarket", ""),
                new Node(CitizensOfSilkshore, "Citizens of Silkshore", ""),
                new Node(CitizensOfSixTowers, "Citizens of Six Towers", ""),
                new Node(CitizensOfWhitecrown, "Citizens of Whitecrown", "")
            };

            return factions;
        }
        
        public List<Edge> CreateDuskvolRelations()
        {
            var edges = new List<Edge>
            {
                new Edge(GetNextId(), Billhooks, Bluecoats, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Billhooks, Crows, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Billhooks, CitizensOfTheDocks, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Billhooks, Lost, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Billhooks, MinistryOfPreservation, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Billhooks, UlfIronborn, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), Bluecoats, Billhooks, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Bluecoats, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Bluecoats, Crows, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Bluecoats, ImperialMilitary, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Bluecoats, IronhookPrison, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Bluecoats, LordScurlock, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Bluecoats, Unseen, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), CircleOfFlame, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CircleOfFlame, ForgottenGods, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CircleOfFlame, Foundation, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CircleOfFlame, Hive, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), CircleOfFlame, PathOfEchoes, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CircleOfFlame, SilverNails, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), CanalPatrol, Bluecoats, "Part Of", Edge.Relationship.Ally),
                
                new Edge(GetNextId(), CityCouncil, Bluecoats, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, Brigade, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, Cabbies, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, CircleOfFlame, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, ChurchOfEcstasy, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, Foundation, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, ImperialMilitary, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), CityCouncil, Inspectors, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), CityCouncil, LordScurlock, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), CityCouncil, MinistryOfPreservation, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), CityCouncil, Reconciled, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), CityCouncil, Sparkwrights, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), ChurchOfEcstasy, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), ChurchOfEcstasy, LeviathanHunters, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), ChurchOfEcstasy, PathOfEchoes, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), ChurchOfEcstasy, Reconciled, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), ChurchOfEcstasy, SpiritWardens, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), Crows, Bluecoats, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Crows, CitizensOfCrowsFoot, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Crows, Dockers, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Crows, Hive, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Crows, Inspectors, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Crows, Lost, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Crows, Sailors, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), DeathlandsScavengers, ForgottenGods, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), DeathlandsScavengers, Gondoliers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), DeathlandsScavengers, IronhookPrison, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), DeathlandsScavengers, SpiritWardens, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), DimmerSisters, ForgottenGods, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), DimmerSisters, Foundation, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), DimmerSisters, Reconciled, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), DimmerSisters, SpiritWardens, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), FogHounds, CanalPatrol, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), FogHounds, Dockers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), FogHounds, Lampblacks, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), FogHounds, Vultures, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), Gondoliers, Lampblacks, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Gondoliers, RedSashes, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Gondoliers, SpiritWardens, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), GrayCloaks, Bluecoats, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), GrayCloaks, Inspectors, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), GrayCloaks, LordStrangford, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), Grinders, Bluecoats, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Grinders, Dockers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Grinders, ImperialMilitary, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Grinders, LeviathanHunters, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Grinders, Sailors, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Grinders, SilverNails, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Grinders, UlfIronborn, "", Edge.Relationship.Friend),

                new Edge(GetNextId(), Hive, CircleOfFlame, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Hive, Crows, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Hive, DaggerIslesConsulate, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Hive, MinistryOfPreservation, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Hive, Unseen, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Hive, Wraiths, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), Lampblacks, Bluecoats, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Lampblacks, Cabbies, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Lampblacks, FogHounds, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lampblacks, Gondoliers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lampblacks, IronhookPrison, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lampblacks, RedSashes, "", Edge.Relationship.War),

                new Edge(GetNextId(), LeviathanHunters, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LeviathanHunters, ChurchOfEcstasy, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LeviathanHunters, Dockers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LeviathanHunters, MinistryOfPreservation, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), LeviathanHunters, PathOfEchoes, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), LeviathanHunters, Sailors, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LeviathanHunters, Sparkwrights, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LeviathanHunters, Grinders, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), LordScurlock, Bluecoats, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LordScurlock, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LordScurlock, ForgottenGods, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LordScurlock, Inspectors, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), LordScurlock, SpiritWardens, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), LordStrangford, CityCouncil, "Part Of", Edge.Relationship.Friend),
                new Edge(GetNextId(), LordStrangford, LeviathanHunters, "Part Of", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), Lost, Billhooks, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Lost, Bluecoats, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Lost, CitizensOfCoalridge, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lost, CitizensOfDunlough, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lost, CoalridgeWorkhouseLaborers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lost, CoalridgeWorkhouseForemen, "", Edge.Relationship.War),
                new Edge(GetNextId(), Lost, CoalridgeWorkhouseLaborers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lost, Crows, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Lost, DeathlandsScavengers, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), MinistryOfPreservation, Billhooks, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), MinistryOfPreservation, ImperialMilitary, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), MinistryOfPreservation, LeviathanHunters, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), MinistryOfPreservation, RailJacks, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), MinistryOfPreservation, Sparkwrights, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), Reconciled, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Reconciled, Gondoliers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Reconciled, ChurchOfEcstasy, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Reconciled, Sparkwrights, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Reconciled, SpiritWardens, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), RedSashes, Bluecoats, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), RedSashes, Cabbies, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), RedSashes, Dockers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), RedSashes, Gondoliers, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), RedSashes, Inspectors, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), RedSashes, IruvianConsulate, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), RedSashes, Lampblacks, "", Edge.Relationship.War),
                new Edge(GetNextId(), RedSashes, PathOfEchoes, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), SilverNails, CircleOfFlame, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SilverNails, Grinders, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SilverNails, ImperialMilitary, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), SilverNails, Sailors, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), SilverNails, SeverosiConsulate, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), SilverNails, SkovlanConsulate, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SilverNails, SkovlanderRefugees, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SilverNails, SpiritWardens, "", Edge.Relationship.Enemy),

                new Edge(GetNextId(), Sparkwrights, CityCouncil, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Sparkwrights, Foundation, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Sparkwrights, LeviathanHunters, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Sparkwrights, MinistryOfPreservation, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Sparkwrights, PathOfEchoes, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Sparkwrights, Reconciled, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), SpiritWardens, ChurchOfEcstasy, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), SpiritWardens, DimmerSisters, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SpiritWardens, Gondoliers, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SpiritWardens, LordScurlock, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SpiritWardens, PathOfEchoes, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SpiritWardens, Reconciled, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SpiritWardens, SilverNails, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), SpiritWardens, Unseen, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), UlfIronborn, Billhooks, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), UlfIronborn, CitizensOfCoalridge, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), UlfIronborn, Grinders, "", Edge.Relationship.Friend),
                
                new Edge(GetNextId(), Unseen, Bluecoats, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Unseen, Cyphers, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Unseen, ForgottenGods, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Unseen, Hive, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Unseen, InkRakes, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Unseen, IronhookPrison, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Unseen, SpiritWardens, "", Edge.Relationship.Enemy),
                
                new Edge(GetNextId(), Wraiths, Bluecoats, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Wraiths, Cabbies, "", Edge.Relationship.Friend),
                new Edge(GetNextId(), Wraiths, Hive, "", Edge.Relationship.Enemy),
                new Edge(GetNextId(), Wraiths, Inspectors, "", Edge.Relationship.Enemy),
            };

            return edges;
        }
    }
}