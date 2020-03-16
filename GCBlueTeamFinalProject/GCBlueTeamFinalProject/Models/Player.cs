using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCBlueTeamFinalProject.Models
{

    public class PlayerRootObject
    {
        public Result2[] Results { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public Self Self { get; set; }
    }

    public class Self
    {
        public string AuthorityId { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string RetryPolicyId { get; set; }
        public string TopicName { get; set; }
        public int AcknowledgementTypeId { get; set; }
        public bool AuthenticationLifetimeExtensionSupported { get; set; }
        public bool ClearanceAware { get; set; }
    }

    public class Result2
    {
        public string Id { get; set; }
        public int ResultCode { get; set; }
        public Result1 Result { get; set; }
    }

    public class Result1
    {
        public Arenastats ArenaStats { get; set; }
        public Playerid PlayerId { get; set; }
        public int SpartanRank { get; set; }
        public int Xp { get; set; }
    }

    public class Arenastats
    {
        public object[] ArenaPlaylistStats { get; set; }
        public Highestcsrattained HighestCsrAttained { get; set; }
        public Arenagamebasevariantstat[] ArenaGameBaseVariantStats { get; set; }
        public Topgamebasevariant[] TopGameBaseVariants { get; set; }
        public string HighestCsrPlaylistId { get; set; }
        public string HighestCsrSeasonId { get; set; }
        public string ArenaPlaylistStatsSeasonId { get; set; }
        public int TotalKills { get; set; }
        public int TotalHeadshots { get; set; }
        public float TotalWeaponDamage { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public Weaponwithmostkills WeaponWithMostKills { get; set; }
        public int TotalMeleeKills { get; set; }
        public float TotalMeleeDamage { get; set; }
        public int TotalAssassinations { get; set; }
        public int TotalGroundPoundKills { get; set; }
        public float TotalGroundPoundDamage { get; set; }
        public int TotalShoulderBashKills { get; set; }
        public float TotalShoulderBashDamage { get; set; }
        public float TotalGrenadeDamage { get; set; }
        public int TotalPowerWeaponKills { get; set; }
        public float TotalPowerWeaponDamage { get; set; }
        public int TotalPowerWeaponGrabs { get; set; }
        public string TotalPowerWeaponPossessionTime { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalAssists { get; set; }
        public int TotalGamesCompleted { get; set; }
        public int TotalGamesWon { get; set; }
        public int TotalGamesLost { get; set; }
        public int TotalGamesTied { get; set; }
        public string TotalTimePlayed { get; set; }
        public int TotalGrenadeKills { get; set; }
        public Medalaward1[] MedalAwards { get; set; }
        public Destroyedenemyvehicle1[] DestroyedEnemyVehicles { get; set; }
        public object[] EnemyKills { get; set; }
        public Weaponstat1[] WeaponStats { get; set; }
        public Impulse1[] Impulses { get; set; }
        public int TotalSpartanKills { get; set; }
        public string FastestMatchWin { get; set; }
    }

    public class Highestcsrattained
    {
        public int Tier { get; set; }
        public int DesignationId { get; set; }
        public int Csr { get; set; }
        public int PercentToNextTier { get; set; }
        public object Rank { get; set; }
    }

    public class Weaponwithmostkills
    {
        public Weaponid WeaponId { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public int TotalHeadshots { get; set; }
        public int TotalKills { get; set; }
        public float TotalDamageDealt { get; set; }
        public string TotalPossessionTime { get; set; }
    }

    public class Weaponid
    {
        public long StockId { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Arenagamebasevariantstat
    {
        public Flexiblestats FlexibleStats { get; set; }
        public string GameBaseVariantId { get; set; }
        public int TotalKills { get; set; }
        public int TotalHeadshots { get; set; }
        public float TotalWeaponDamage { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public Weaponwithmostkills1 WeaponWithMostKills { get; set; }
        public int TotalMeleeKills { get; set; }
        public float TotalMeleeDamage { get; set; }
        public int TotalAssassinations { get; set; }
        public int TotalGroundPoundKills { get; set; }
        public float TotalGroundPoundDamage { get; set; }
        public int TotalShoulderBashKills { get; set; }
        public float TotalShoulderBashDamage { get; set; }
        public float TotalGrenadeDamage { get; set; }
        public int TotalPowerWeaponKills { get; set; }
        public float TotalPowerWeaponDamage { get; set; }
        public int TotalPowerWeaponGrabs { get; set; }
        public string TotalPowerWeaponPossessionTime { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalAssists { get; set; }
        public int TotalGamesCompleted { get; set; }
        public int TotalGamesWon { get; set; }
        public int TotalGamesLost { get; set; }
        public int TotalGamesTied { get; set; }
        public string TotalTimePlayed { get; set; }
        public int TotalGrenadeKills { get; set; }
        public Medalaward[] MedalAwards { get; set; }
        public Destroyedenemyvehicle[] DestroyedEnemyVehicles { get; set; }
        public object[] EnemyKills { get; set; }
        public Weaponstat[] WeaponStats { get; set; }
        public Impulse[] Impulses { get; set; }
        public int TotalSpartanKills { get; set; }
        public string FastestMatchWin { get; set; }
    }

    public class Flexiblestats
    {
        public Medalstatcount[] MedalStatCounts { get; set; }
        public Impulsestatcount[] ImpulseStatCounts { get; set; }
        public object[] MedalTimelapses { get; set; }
        public Impulsetimelaps[] ImpulseTimelapses { get; set; }
    }

    public class Medalstatcount
    {
        public string Id { get; set; }
        public int Count { get; set; }
    }

    public class Impulsestatcount
    {
        public string Id { get; set; }
        public int Count { get; set; }
    }

    public class Impulsetimelaps
    {
        public string Id { get; set; }
        public string Timelapse { get; set; }
    }

    public class Weaponwithmostkills1
    {
        public Weaponid1 WeaponId { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public int TotalHeadshots { get; set; }
        public int TotalKills { get; set; }
        public float TotalDamageDealt { get; set; }
        public string TotalPossessionTime { get; set; }
    }

    public class Weaponid1
    {
        public long StockId { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Medalaward
    {
        public long MedalId { get; set; }
        public int Count { get; set; }
    }

    public class Destroyedenemyvehicle
    {
        public Enemy Enemy { get; set; }
        public int TotalKills { get; set; }
    }

    public class Enemy
    {
        public long BaseId { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Weaponstat
    {
        public Weaponid2 WeaponId { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public int TotalHeadshots { get; set; }
        public int TotalKills { get; set; }
        public float TotalDamageDealt { get; set; }
        public string TotalPossessionTime { get; set; }
    }

    public class Weaponid2
    {
        public long StockId { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Impulse
    {
        public long Id { get; set; }
        public int Count { get; set; }
    }

    public class Topgamebasevariant
    {
        public int GameBaseVariantRank { get; set; }
        public int NumberOfMatchesCompleted { get; set; }
        public string GameBaseVariantId { get; set; }
        public int NumberOfMatchesWon { get; set; }
    }

    public class Medalaward1
    {
        public long MedalId { get; set; }
        public int Count { get; set; }
    }

    public class Destroyedenemyvehicle1
    {
        public Enemy1 Enemy { get; set; }
        public int TotalKills { get; set; }
    }

    public class Enemy1
    {
        public long BaseId { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Weaponstat1
    {
        public Weaponid3 WeaponId { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public int TotalHeadshots { get; set; }
        public int TotalKills { get; set; }
        public float TotalDamageDealt { get; set; }
        public string TotalPossessionTime { get; set; }
    }

    public class Weaponid3
    {
        public long StockId { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Impulse1
    {
        public long Id { get; set; }
        public int Count { get; set; }
    }

    public class Playerid
    {
        public string Gamertag { get; set; }
        public object Xuid { get; set; }
    }

}
