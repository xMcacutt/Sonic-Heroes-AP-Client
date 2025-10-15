using System.Runtime.InteropServices;
using System.Text;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;

namespace Sonic_Heroes_AP_Client;

public enum MusicType
{
    Stage,
    Boss,
    StageShort,
    Theme,
    Jingle,
    Menu,
    Event,
    TeamBlast,
    Ignored
}

public static class MusicShuffleData
{
    public static List<(string name, MusicType type)> HeroesSongs = new()
    {
        ("SNG_BTL01.adx", MusicType.StageShort),            //2P Battle Sea Area
        ("SNG_BTL02.adx", MusicType.StageShort),            //2P Battle City Area
        ("SNG_BTL03.adx", MusicType.StageShort),            //2P Battle Casino Area
        ("SNG_BTL04.adx", MusicType.StageShort),            //2P Battle Ring Race
        ("SNG_BTL05.adx", MusicType.StageShort),            //2P Battle Quick Race
        ("SNG_E0017.adx", MusicType.Theme),                 //"We Can" Credits (Sonic Theme)
        ("SNG_E0117.adx", MusicType.Theme),                 //"This Machine" Credits (Dark Theme) Rose and Chaotix Dont have one but have the full theme)
        ("SNG_E0404.adx", MusicType.Theme),                 //"Sonic Heroes" Credits (Final Story)
        ("SNG_EV10.adx", MusicType.Event),                  //Event 9 "No Past To Remember" (Dark)
        ("SNG_EV11.adx", MusicType.Event),                  //Event 10 "Monkey Business"
        ("SNG_EV12.adx", MusicType.Event),                  //Event 11 "My World"
        ("SNG_EV13.adx", MusicType.Event),                  //Event 12 "My Ambition"
        ("SNG_INVNCBL.adx", MusicType.Jingle),               //Invincible Jingle
        ("SNG_RNDCLR.adx", MusicType.Jingle),                //Round Clear Jingle
        ("SNG_SPEEDUP.adx", MusicType.Jingle),               //Speed Shoes Jingle
        ("SNG_SPSTG1.adx", MusicType.Stage),                //Bonus Stage (No Emerald)
        ("SNG_SPSTG2.adx", MusicType.Stage),                //Bonus Stage (Emerald)
        ("SNG_STG01.adx", MusicType.Stage),                 //Seaside Hill
        ("SNG_STG02.adx", MusicType.Stage),                 //Ocean Palace
        ("SNG_STG03.adx", MusicType.Stage),                 //Grand Metropolis
        ("SNG_STG04.adx", MusicType.Stage),                 //Power Plant
        ("SNG_STG05.adx", MusicType.Stage),                 //Casino Park
        ("SNG_STG06.adx", MusicType.Stage),                 //Bingo Park
        ("SNG_STG07.adx", MusicType.Stage),                 //Rail Canyon
        ("SNG_STG08.adx", MusicType.Stage),                 //Bullet Station
        ("SNG_STG09.adx", MusicType.Stage),                 //Frog Forest
        ("SNG_STG10.adx", MusicType.Stage),                 //Lost Jungle
        ("SNG_STG11A.adx", MusicType.Stage),                //Hang Castle Regular Gravity
        ("SNG_STG11B.adx", MusicType.Stage),                //Hang Castle Upside Down Gravity
        ("SNG_STG12.adx", MusicType.Stage),                 //Mystic Mansion (Entire)
        ("SNG_STG12A.adx", MusicType.Jingle),                //Mystic Mansion Part
        ("SNG_STG12B.adx", MusicType.Jingle),                //Mystic Mansion Part
        ("SNG_STG12C.adx", MusicType.Jingle),                //Mystic Mansion Part
        ("SNG_STG12D.adx", MusicType.Jingle),                //Mystic Mansion Part
        ("SNG_STG12E.adx", MusicType.Jingle),                //Mystic Mansion Part
        ("SNG_STG12F.adx", MusicType.Jingle),                //Mystic Mansion Part
        ("SNG_STG13.adx", MusicType.Stage),                 //Egg Fleet
        ("SNG_STG14.adx", MusicType.Stage),                 //Final Fortress
        ("SNG_STG20.adx", MusicType.Boss),                  //Egg Hawk
        ("SNG_STG20PI.adx", MusicType.Event),               //Egg Hawk Intro (Cutscene)
        ("SNG_STG21.adx", MusicType.Boss),                  //Team Fight 1 (City) (prob not used)
        ("SNG_STG22.adx", MusicType.Boss),                  //Robot Carnival and Robot Storm
        ("SNG_STG23.adx", MusicType.Boss),                  //Egg Albatross
        ("SNG_STG24TC.adx", MusicType.Boss),                //Team Fight Chaotix
        ("SNG_STG24TD.adx", MusicType.Boss),                //Team Fight Dark
        ("SNG_STG24TR.adx", MusicType.Boss),                //Team Fight Rose
        ("SNG_STG24TS.adx", MusicType.Boss),                //Team Fight Sonic (instrumental?)
        ("SNG_STG26.adx", MusicType.Boss),                  //Egg Emperor
        ("SNG_STG27.adx", MusicType.Boss),                  //Metal Madness
        ("SNG_STG28.adx", MusicType.Boss),                  //Metal Overlord
        ("SNG_STG29.adx", MusicType.Stage),                 //Sea Gate
        ("SNG_SYS1.adx", MusicType.Menu),                   //Menu Main (after Save File Selected)
        ("SNG_SYS2.adx", MusicType.Menu),                   //Menu 1P (includes Level Select)
        ("SNG_SYS3.adx", MusicType.Menu),                   //Menu 2P
        ("SNG_SYS4.adx", MusicType.Menu),                   //Menu File Select Options
        ("SNG_TBSFA1.adx", MusicType.Jingle),               //Team Blast Sonic
        ("SNG_TBSFA2.adx", MusicType.Jingle),               //Team Blast Dark
        ("SNG_TBSFA3.adx", MusicType.Jingle),               //Team Blast Rose
        //("SNG_TBSFA4.adx", ),                             //duplicate of TBSFA4J (Japanese Voices)
        ("SNG_TBSFA4E.adx", MusicType.Jingle),              //Team Blast Chaotix (English)
        ("SNG_TBSFA4J.adx", MusicType.Jingle),              //Team Blast Chaotix (Japanese)
        ("SNG_TBSFA5.adx", MusicType.Jingle),               //Team Blast Super Sonic
        ("SNG_TITLE1.adx", MusicType.Jingle),               //Title Theme (with Intro) before cutscene
        ("SNG_TITLE2.adx", MusicType.Jingle),               //Title Theme (without Intro) after cutscene
        ("SNG_V01_MAIN.adx", MusicType.Theme),              //"Sonic Heroes" Main Theme
        ("SNG_V02_TS.adx", MusicType.Theme),                //"We Can" Sonic Theme
        ("SNG_V03_TD.adx", MusicType.Theme),                //"This Machine" Dark Theme
        ("SNG_V04_TR.adx", MusicType.Theme),                //"Follow Me" Rose Theme
        ("SNG_V05_TC.adx", MusicType.Theme)                 //"Team Chaotix" Chaotix Theme
    };

    public static List<(string name, MusicType type)> SADXSongs = new()
    {
        ("advamy.adx", MusicType.Theme),                    //Amy Theme (Short)
        ("advbig.adx", MusicType.Theme),                    //Big Theme (Short)
        ("adve102.adx", MusicType.Theme),                   //Gamma Theme (Short)
        ("advknkls.adx", MusicType.Theme),                  //Knuckles Theme (Short)
        ("advmiles.adx", MusicType.Theme),                  //Tails Theme (Short)
        ("advsonic.adx", MusicType.Theme),                  //Sonic Theme (Short)
        ("amy.adx", MusicType.Theme),                       //Amy Theme
        ("big.adx", MusicType.Theme),                       //Big Theme
        ("bossall.adx", MusicType.Boss),                    //Boss Theme
        ("bosse101.adx", MusicType.Boss),                   //Beta Boss Theme
        ("bossevnt.adx", MusicType.Boss),                   //
        ("bosstrgt.adx", MusicType.Boss),                   //Boss Fight Against Playable Char (Sonic vs Knuckles)
        ("casino1.adx", MusicType.Stage),                   //Casinopolis
        ("casino2.adx", MusicType.Stage),                   //Casinopolis Sewers
        ("casino3.adx", MusicType.Stage),                   //Pinball Table
        ("casino4.adx", MusicType.Stage),                   //Nights Pinball Table
        ("chao.adx", MusicType.Stage),                      //Chao
        ("chaogoal.adx", MusicType.Jingle),
        ("chaohall.adx", MusicType.Ignored),                //Chao Hall (Not in Sadx)
        ("chaorace.adx", MusicType.Stage),                  //Chao Race
        ("chaos.adx", MusicType.Boss),
        ("chaos_6.adx", MusicType.Boss),
        ("chaos_p1.adx", MusicType.Boss),
        ("chaos_p2.adx", MusicType.Boss),
        ("chao_g_born_c.adx", MusicType.Ignored),
        ("chao_g_born_d.adx", MusicType.Ignored),
        ("chao_g_born_d2.adx", MusicType.Ignored),
        ("chao_g_born_h.adx", MusicType.Ignored),
        ("chao_g_born_h2.adx", MusicType.Ignored),
        ("chao_g_dance.adx", MusicType.Ignored),
        ("chao_g_dead.adx", MusicType.Ignored),
        ("chao_g_iede.adx", MusicType.Ignored),
        ("CHAO_K_M.adx", MusicType.Menu),                   //Black Market
        ("chao_k_net_fine.adx", MusicType.Ignored),
        ("CHAO_R_E.adx", MusicType.Menu),
        ("chao_r_gate_open.adx", MusicType.Ignored),        //
        ("charactr.adx", MusicType.Menu),                   //Choose Character
        ("circuit.adx", MusicType.StageShort),
        ("continue.adx", MusicType.Ignored),                //Run Out of Lives
        ("c_btl_cv.adx", MusicType.Menu),
        ("e102.adx", MusicType.Theme),
        ("ecoast1.adx", MusicType.Stage),
        ("ecoast2.adx", MusicType.Stage),
        ("ecoast3.adx", MusicType.Stage),
        ("egcarer1.adx", MusicType.Stage),
        ("egcarer2.adx", MusicType.Stage),
        ("eggman.adx", MusicType.Theme),
        ("eggmbl23.adx", MusicType.Boss),
        ("eggrobo.adx", MusicType.Theme),
        ("evtbgm00.adx", MusicType.Event),
        ("evtbgm01.adx", MusicType.Event),                  //
        ("evtbgm02.adx", MusicType.Event),
        ("evtbgm03.adx", MusicType.Event),
        ("evtbgm04.adx", MusicType.Event),
        ("evtbgm05.adx", MusicType.Event),
        ("finaleg1.adx", MusicType.Stage),
        ("finaleg2.adx", MusicType.Stage),
        ("fishget.adx", MusicType.Jingle),
        ("fishing.adx", MusicType.Jingle),
        ("fishmiss.adx", MusicType.Ignored),
        ("hammer.adx", MusicType.StageShort),
        ("highway1.adx", MusicType.Stage),
        ("highway2.adx", MusicType.StageShort),
        ("highway3.adx", MusicType.Stage),
        ("hurryup.adx", MusicType.Jingle),
        ("icecap1.adx", MusicType.Stage),
        ("icecap2.adx", MusicType.Stage),
        ("icecap3.adx", MusicType.Stage),
        ("invncibl.adx", MusicType.Jingle),                 //
        ("item1.adx", MusicType.Jingle),
        ("jingle_1.adx", MusicType.Menu),
        ("jingle_2.adx", MusicType.Menu),
        ("jingle_3.adx", MusicType.Menu),
        ("jingle_4.adx", MusicType.Menu),
        ("jingle_5.adx", MusicType.Menu),
        ("KNUCKLES.adx", MusicType.Theme),
        ("lstwrld1.adx", MusicType.Stage),
        ("lstwrld2.adx", MusicType.StageShort),
        ("lstwrld3.adx", MusicType.Stage),
        ("mainthem.adx", MusicType.Theme),
        ("MCLEAR_44.adx", MusicType.Jingle),
        ("MSTART_44.adx", MusicType.Jingle),
        ("mstcln.adx", MusicType.Stage),
        //("nights_k.adx", ),                               //Empty file
        ("one_up.adx", MusicType.Jingle),
        ("option.adx", MusicType.Menu),
        ("redmntn1.adx", MusicType.Stage),
        ("redmntn2.adx", MusicType.Stage),
        ("rndclear.adx", MusicType.Jingle),
        ("sandhill.adx", MusicType.Stage),
        ("scramble.adx", MusicType.StageShort),             //Tornado Music
        ("shelter1.adx", MusicType.Stage),
        ("shelter2.adx", MusicType.Stage),
        ("skydeck1.adx", MusicType.Stage),
        ("skydeck2.adx", MusicType.Stage),
        ("sonic.adx", MusicType.Theme),
        ("speedup.adx", MusicType.Jingle),
        ("sprsonic.adx", MusicType.Theme),
        ("s_square.adx", MusicType.Stage),
        ("tails.adx", MusicType.Theme),
        ("theamy.adx", MusicType.Theme),
        ("thebig.adx", MusicType.Theme),
        ("thee102.adx", MusicType.Theme),
        ("theknkls.adx", MusicType.Theme),
        ("themiles.adx", MusicType.Theme),
        ("thesonic.adx", MusicType.Theme),
        ("tical.adx", MusicType.Theme),
        ("timer.adx", MusicType.Jingle),
        ("title.adx", MusicType.Jingle),
        ("title2.adx", MusicType.Jingle),
        ("titl_egg.adx", MusicType.Ignored),
        ("titl_mr1.adx", MusicType.Ignored),
        ("titl_mr2.adx", MusicType.Ignored),
        ("titl_ss.adx", MusicType.Ignored),
        ("trial.adx", MusicType.Menu),
        ("twnklpk1.adx", MusicType.Stage),
        ("twnklpk2.adx", MusicType.Stage),
        ("twnklpk3.adx", MusicType.StageShort),
        ("wndyvly1.adx", MusicType.Stage),
        ("wndyvly2.adx", MusicType.StageShort),
        ("wndyvly3.adx", MusicType.Stage)
    };

    public static List<(string name, MusicType type)> SA2Songs = new()
    {
        ("advsng_1.adx", MusicType.Menu),
        ("advsng_2.adx", MusicType.Menu),
        ("advsng_3.adx", MusicType.Menu),
        ("advsng_4.adx", MusicType.Menu),
        ("advsng_5.adx", MusicType.Menu),
        ("a_mine.adx", MusicType.Stage),
        ("boss_01.adx", MusicType.Boss),
        ("BOSS_02A.ADX", MusicType.Boss),
        ("BOSS_02B.ADX", MusicType.Boss),
        ("boss_03.adx", MusicType.Boss),
        ("boss_04.adx", MusicType.Boss),
        ("boss_05.adx", MusicType.Boss),
        ("boss_06.adx", MusicType.Boss),
        ("BOSS_07.ADX", MusicType.Boss),
        ("btl_ce.adx", MusicType.StageShort),
        ("btl_hb.adx", MusicType.StageShort),
        ("btl_ig.adx", MusicType.StageShort),
        ("btl_mh.adx", MusicType.StageShort),
        ("btl_opng.adx", MusicType.Menu),
        ("btl_pc.adx", MusicType.StageShort),
        ("btl_rh.adx", MusicType.StageShort),
        ("btl_sel.adx", MusicType.StageShort),
        ("btl_so.adx", MusicType.StageShort),
        ("btl_wb.adx", MusicType.StageShort),
        ("btl_wj.adx", MusicType.StageShort),
        ("chao_g_bgm_d.adx", MusicType.Stage),
        ("chao_g_bgm_h.adx", MusicType.Stage),
        ("chao_g_bgm_n.adx", MusicType.Stage),
        ("chao_g_born_c.adx", MusicType.Ignored),
        ("chao_g_born_d.adx", MusicType.Ignored),
        ("chao_g_born_d2.adx", MusicType.Ignored),
        ("chao_g_born_h.adx", MusicType.Ignored),
        ("chao_g_born_h2.adx", MusicType.Ignored),
        ("chao_g_born_hc.adx", MusicType.Ignored),
        ("chao_g_dance.adx", MusicType.Ignored),
        ("chao_g_dead.adx", MusicType.Ignored),
        ("chao_g_gate_open.adx", MusicType.Ignored),
        ("chao_g_iede.adx", MusicType.Ignored),
        ("chao_g_new_garden.adx", MusicType.Ignored),
        ("chao_g_radicase1.adx", MusicType.Ignored),
        ("chao_g_radicase2.adx", MusicType.Ignored),
        ("chao_g_radicase3.adx", MusicType.Ignored),
        ("chao_g_radicase4.adx", MusicType.Ignored),
        ("chao_g_tv_cartoon.adx", MusicType.Ignored),
        ("chao_g_tv_drama.adx", MusicType.Ignored),
        ("chao_g_tv_sports.adx", MusicType.Ignored),
        ("chao_hall.adx", MusicType.StageShort),
        ("CHAO_K_M.ADX", MusicType.Menu),
        ("chao_k_m2.adx", MusicType.Menu),
        ("chao_k_net_connect.adx", MusicType.Menu),
        ("chao_k_net_fault.adx", MusicType.Ignored),
        ("chao_k_net_fine.adx", MusicType.Ignored),
        ("chao_k_sing_dark1.adx", MusicType.Ignored),
        ("chao_k_sing_dark2.adx", MusicType.Ignored),
        ("chao_k_sing_dark3.adx", MusicType.Ignored),
        ("chao_k_sing_hero1.adx", MusicType.Ignored),
        ("chao_k_sing_hero2.adx", MusicType.Ignored),
        ("chao_k_sing_hero3.adx", MusicType.Ignored),
        ("chao_l_m.adx", MusicType.Stage),
        ("chao_new_garden.adx", MusicType.Ignored),
        ("chao_r_a.adx", MusicType.Jingle),
        ("chao_r_b.adx", MusicType.Ignored),                //short version of chao_r_j
        ("chao_r_c.adx", MusicType.Stage),
        ("chao_r_d.adx", MusicType.Stage),
        ("CHAO_R_E.ADX", MusicType.Menu),
        ("chao_r_gate_open.adx", MusicType.Ignored),
        ("chao_r_h.adx", MusicType.Stage),
        ("chao_r_item_get.adx", MusicType.Jingle),
        ("chao_r_j.adx", MusicType.Stage),
        ("continue.adx", MusicType.Ignored),
        ("c_btl_ch.adx", MusicType.Jingle),
        ("c_btl_cv.adx", MusicType.Menu),
        ("c_btl_dr.adx", MusicType.Jingle),
        ("c_btl_gm.adx", MusicType.StageShort),
        ("c_btl_ls.adx", MusicType.Jingle),
        ("c_btl_sl.adx", MusicType.Menu),
        ("c_btl_wn.adx", MusicType.Jingle),
        ("c_core_1.adx", MusicType.Stage),
        ("c_core_2.adx", MusicType.Stage),
        ("c_core_5.adx", MusicType.Stage),
        ("C_CORE_6.ADX", MusicType.Event),
        ("c_escap1.adx", MusicType.Stage),
        ("c_escap2.adx", MusicType.Stage),
        ("c_escap3.adx", MusicType.Stage),
        ("c_gadget.adx", MusicType.Stage),
        ("c_wall.adx", MusicType.Stage),
        ("d_chambr.adx", MusicType.Stage),
        ("d_lagoon.adx", MusicType.Stage),
        ("e000_sng.adx", MusicType.Event),
        ("E006_SNG.ADX", MusicType.Event),
        ("e019_sng.adx", MusicType.Event),
        ("e021_sng.adx", MusicType.Event),
        ("E027_SNG.ADX", MusicType.Event),
        ("E028_SNG.ADX", MusicType.Event),
        ("E101_SNG.ADX", MusicType.Event),
        ("E106_SNG.ADX", MusicType.Event),
        ("E111_SNG.ADX", MusicType.Event),
        ("E112_SNG.ADX", MusicType.Event),
        ("E119_SNG.ADX", MusicType.Event),
        ("e127_sng.adx", MusicType.Event),
        ("E130_SNG.ADX", MusicType.Event),
        ("E205_SNG.ADX", MusicType.Event),
        ("e207_sng.adx", MusicType.Event),
        ("E208_SNG.ADX", MusicType.Event),
        ("E210_SN1.ADX", MusicType.Event),
        ("E210_SN2.ADX", MusicType.Event),
        ("e350_sng.adx", MusicType.Event),
        ("e_engine.adx", MusicType.Stage),
        ("e_quart.adx", MusicType.Stage),
        ("f_chase.adx", MusicType.Stage),
        ("f_rush.adx", MusicType.Stage),
        ("g_fores.adx", MusicType.Stage),
        ("g_hill.adx", MusicType.Stage),
        ("h_base.adx", MusicType.Stage),
        ("invncibl.adx", MusicType.Jingle),
        ("item_get.adx", MusicType.Jingle),
        ("i_gate.adx", MusicType.Stage),
        ("kart.adx", MusicType.Stage),
        ("l_colony.adx", MusicType.Stage),
        ("m_harb1.adx", MusicType.Stage),
        ("m_harb2.adx", MusicType.StageShort),
        ("m_herd.adx", MusicType.Stage),
        ("m_space.adx", MusicType.Stage),
        ("m_street.adx", MusicType.Stage),
        ("one_up.adx", MusicType.Jingle),
        ("p_cave.adx", MusicType.Stage),
        ("p_hill.adx", MusicType.Stage),
        ("p_lane.adx", MusicType.Stage),
        ("rndclear.adx", MusicType.Jingle),
        ("r_hwy.adx", MusicType.Stage),
        ("speedup.adx", MusicType.Jingle),
        ("s_hall.adx", MusicType.Stage),
        ("s_ocean.adx", MusicType.Stage),
        ("s_rail.adx", MusicType.Stage),
        ("t1_shado.adx", MusicType.Theme),
        ("t1_sonic.adx", MusicType.Theme),
        ("T2_MILES.ADX", MusicType.Theme),
        ("T2_ROUGE.ADX", MusicType.Theme),
        ("t3_eggma.adx", MusicType.Theme),
        ("T3_KNUCK.ADX", MusicType.Theme),
        ("T3_MILES.ADX", MusicType.Theme),
        ("T3_SHADO.ADX", MusicType.Theme),
        ("T4_KNUCK.ADX", MusicType.Theme),
        ("T4_ROUGE.ADX", MusicType.Theme),
        ("T4_SONIC.ADX", MusicType.Theme),
        ("t9_amy.adx", MusicType.Theme),
        ("T9_EGGMA.ADX", MusicType.Theme),
        ("T9_KNUCK.ADX", MusicType.Theme),
        ("T9_MILES.ADX", MusicType.Theme),
        ("T9_ROUGE.ADX", MusicType.Theme),
        ("T9_SHADO.ADX", MusicType.Theme),
        ("T9_SONIC.ADX", MusicType.Theme),
        ("timer.adx", MusicType.Jingle),
        ("title.adx", MusicType.Jingle),
        ("w_bed.adx", MusicType.Stage),
        ("w_canyon.adx", MusicType.Stage),
        ("w_jungl.adx", MusicType.Stage)
    };
}

public class MusicShuffleHandler
{
    public static Dictionary<string, string> Map;

    public void Shuffle(int seed)
    {
        if (!Mod.Configuration.MusicShuffleHeroes 
            && !Mod.Configuration.MusicShuffleSADX 
            && !Mod.Configuration.MusicShuffleSA2)
            return;
        //Console.WriteLine($"Shuffle Here: {seed}");
        Map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var allSongs = new List<(string name, MusicType type)>();
        var heroesSongs = MusicShuffleData.HeroesSongs;
        
        if (Mod.Configuration.MusicShuffleHeroes)
            allSongs.AddRange(heroesSongs);
        if (Mod.Configuration.MusicShuffleSADX)
            allSongs.AddRange(MusicShuffleData.SADXSongs);
        if (Mod.Configuration.MusicShuffleSA2)
            allSongs.AddRange(MusicShuffleData.SA2Songs);
        if (Mod.Configuration.MusicShuffleCustom)
        {
            if (Directory.Exists(Mod.Configuration.MusicShuffleCustomFolder))
            {
                allSongs.AddRange(
                    from type in Enum.GetValues(typeof(MusicType)).Cast<MusicType>() 
                    where Directory.Exists(Path.Combine(Mod.Configuration.MusicShuffleCustomFolder, type.ToString())) 
                    from file in Directory.GetFiles(Path.Combine(Mod.Configuration.MusicShuffleCustomFolder, type.ToString())) 
                    select (Path.GetFileName(file), type));
            }
        }

        var heroesGroups = heroesSongs.GroupBy(s => s.type);
        var allGroups = allSongs.GroupBy(s => s.type).ToDictionary(g => g.Key, g => g.Select(x => x.name).ToArray());
        var random = new Random(seed);
        foreach (var group in heroesGroups)
        {
            var songs = group.Select(x => x.name).ToList();
            var type = group.Key;
            var shuffled = allGroups[type].OrderBy(_ => random.Next()).ToList();
            if (songs.Count > shuffled.Count)
            {
                for (var i = 0; i < shuffled.Count; i++)
                {
                    Map[songs[i]] = shuffled[i];
                    Mod.SaveDataHandler!.CustomSaveData!.MusicRandoMapping[songs[i]] = shuffled[i];
                }

                for (var i = shuffled.Count; i < songs.Count; i++)
                {
                    Map[songs[i]] = songs[i];
                    Mod.SaveDataHandler!.CustomSaveData!.MusicRandoMapping[songs[i]] = songs[i];
                }
                continue;
            }

            for (var i = 0; i < songs.Count; i++)
            {
                Map[songs[i]] = shuffled[i];  
                Mod.SaveDataHandler!.CustomSaveData!.MusicRandoMapping[songs[i]] = shuffled[i];
            }
                
        }

        try
        {
            //Handle Mystic Mansion Here
            var tempStr = Map["SNG_STG12.adx"];
            Console.WriteLine($"Mystic Mansion Should Now Be: {tempStr}");
            

            unsafe
            {
                //MusicShuffleSpecialCases* shuffleSpecialCases = (MusicShuffleSpecialCases*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();

                IntPtr mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                //var shuffleSpecialCasesAddr = (IntPtr)mysticMansion;
                
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1090D5, BitConverter.GetBytes((int)(string*)mysticMansion));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x3FB3F, BitConverter.GetBytes((int)(string*)mysticMansion));
                
                /*
                //Try Individual Here
                tempStr = Map["SNG_STG12A.adx"];
                mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1090D5, BitConverter.GetBytes((int)(string*)mysticMansion));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x3FB3F, BitConverter.GetBytes((int)(string*)mysticMansion));
                
                tempStr = Map["SNG_STG12B.adx"];
                mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x109103, BitConverter.GetBytes((int)(string*)mysticMansion));
                
                tempStr = Map["SNG_STG12C.adx"];
                mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x109131, BitConverter.GetBytes((int)(string*)mysticMansion));
                
                tempStr = Map["SNG_STG12D.adx"];
                mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x10915F, BitConverter.GetBytes((int)(string*)mysticMansion));
                
                tempStr = Map["SNG_STG12E.adx"];
                mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x109185, BitConverter.GetBytes((int)(string*)mysticMansion));
                
                tempStr = Map["SNG_STG12F.adx"];
                mysticMansion = (IntPtr)(string*)Marshal.AllocHGlobal(tempStr.Length + 1).ToPointer();
                Memory.Instance.SafeWrite((UIntPtr)mysticMansion, Encoding.ASCII.GetBytes((tempStr + '\0').ToArray()));
                Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1091AB, BitConverter.GetBytes((int)(string*)mysticMansion));
                */
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }
}