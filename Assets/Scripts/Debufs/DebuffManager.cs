using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebuffManager : MonoBehaviour
{
    public enum Debuffs
    {
        [InspectorName("Don't use!")]
        NONE,
        [InspectorName("Honey Platform")]
        STICKY,
        [InspectorName("Faster Player('s)")]
        QUICK_MOV,
        [InspectorName("Slower Player('s)")]
        SLOW_MOV,
        [InspectorName("Falling projectiles")]
        TOMATO,
        [InspectorName("Inverted controlls for Player('s)")]
        INVERTED,
        [InspectorName("Smaller Platforms")]
        SMOL_PLOTFORM,
        [InspectorName("Bigger Spikes")]
        BIG_SPIKES,
        [InspectorName("Brittle Platforms")]
        BRITTLE,
        [InspectorName("Clown Suit")]
        CLOWN,
        [InspectorName("Screen shake")]
        COLD_BEANS,
        [InspectorName("Don't use")]
        MAX_SIZE
    }

    [Serializable]
    class Debuffer
    {
        public bool isActive;
        public bool isBothPlayers;
        public CrowdPleaser.Player player;
        public float timer;
        public float threshold;
        public Debuffs debuff;

        public Debuffer(bool players, float Threshold, CrowdPleaser.Player p, Debuffs set)
        {
            isActive = true;
            isBothPlayers = players;
            player = p;
            timer = 0.0f;
            debuff = set;
            threshold = Threshold;
        }

        public void UpdateTimer()
        {
            this.timer += Time.deltaTime;
            if (timer >= threshold)
                isActive = false;
        }
    }

    //public Debuffs CurrentDebuff => currentDebuff;
    //[SerializeField] private Debuffs currentDebuff = Debuffs.NONE;

    [Serializable]
    public struct TimeOfDebuff
    {
        public Debuffs type;
        public float time;
    }


    [SerializeField] private TimeOfDebuff[] SpecialDebuffTime;

    private King theKing;
    private Platform_Manager_Script theKingsServant;
    private GameObject[] players = new GameObject[2];
    private Camera cam;
    private GameObject spikes;
    private Vector3 spikesScale;
    private TomatoThrowers[] TT;
    
    private Debuffs prevDebuffKing;
    private Debuffs prevDebuffPlayers;

    [SerializeField] private Debuffs[] kingsDebuffs;
    [SerializeField] private Debuffs[] playersDebuffs;
    [SerializeField] private List<Debuffer> debuffs = new List<Debuffer>();
    [SerializeField] private float spikesScaleMod = 1.5f;
    [SerializeField] private float defaultTimeOfDebuff = 10.0f; //temp
    [SerializeField] private float playerSpeedUpMod = 1.5f; //temp
    [SerializeField] private float playerSpeedDownMod = .75f; //temp
    [SerializeField] private float clownDebuffMod = .75f; //temp
    private int playerOneDebuffCount = 0;
    private int playerTwoDebuffCount = 0;
    private int kingDebuffCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<Camera>();
        theKing = GameObject.FindGameObjectWithTag("King").GetComponent<King>();
        theKingsServant = GameObject.FindGameObjectWithTag("PlatformManager").GetComponent<Platform_Manager_Script>();
        spikes = GameObject.FindGameObjectWithTag("Spikes");
        spikesScale = spikes.gameObject.GetComponent<Transform>().localScale;
        TT = FindObjectsOfType<TomatoThrowers>();

        var temp = GameObject.FindGameObjectsWithTag("Player");
        if (temp[0].gameObject.GetComponent<CrowdPleaser>().player == CrowdPleaser.Player.PLAYER_ONE)
        {
            players[0] = temp[0];
            players[1] = temp[1];
        }
        else
        {
            players[1] = temp[1];
            players[0] = temp[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < debuffs.Count; ++i)
        {
            if (debuffs[i].isActive)
                debuffs[i].UpdateTimer();
            else
            {
                if (debuffs[i].isBothPlayers)
                {
                    KingsClearDebuffs(debuffs[i]);
                    theKing.DebuffDone();
                    theKingsServant.ClearDebuffAll(debuffs[i].debuff);
                }
                else
                {
                    PlayersClearDebuffs(debuffs[i]);
                    theKingsServant.ClearDebuffPlayer(debuffs[i].player, debuffs[i].debuff); 
                }
                debuffs.RemoveAt(i);
            }
        }
    }

    private void PlayersDebuffs(Debuffer db)
    {
        switch (db.debuff)
        {
            case Debuffs.SLOW_MOV:
                if (db.player == CrowdPleaser.Player.PLAYER_ONE)
                    players[0].GetComponent<PlayerManager>().movementSpeedMod = playerSpeedDownMod;
                else
                    players[1].GetComponent<PlayerManager>().movementSpeedMod = playerSpeedDownMod;
                break;
            case Debuffs.QUICK_MOV:
                if (db.player == CrowdPleaser.Player.PLAYER_ONE) 
                    players[0].GetComponent<PlayerManager>().movementSpeedMod = playerSpeedUpMod;
                else
                    players[1].GetComponent<PlayerManager>().movementSpeedMod = playerSpeedUpMod;
                break;
            case Debuffs.CLOWN:
                if (db.player == CrowdPleaser.Player.PLAYER_ONE)
                    players[0].GetComponent<CrowdPleaser>().crowdMoodModModMod = clownDebuffMod;
                else
                    players[1].GetComponent<CrowdPleaser>().crowdMoodModModMod = clownDebuffMod;
                break;
        }
    }

    private void PlayersClearDebuffs(Debuffer db)
    {
        switch (db.debuff)
        {
            case Debuffs.SLOW_MOV:
            case Debuffs.QUICK_MOV:
                if (db.player == CrowdPleaser.Player.PLAYER_ONE)
                    players[0].GetComponent<PlayerManager>().movementSpeedMod = 1.0f;
                else
                    players[1].GetComponent<PlayerManager>().movementSpeedMod = 1.0f;
                break;
            case Debuffs.CLOWN:
                if (db.player == CrowdPleaser.Player.PLAYER_ONE)
                    players[0].GetComponent<CrowdPleaser>().crowdMoodModModMod = 1.0f;
                else
                    players[1].GetComponent<CrowdPleaser>().crowdMoodModModMod = 1.0f;
                break;
        }
    }

    private void KingsDebuffs(Debuffer db)
    {
        switch (db.debuff)
        {
            case Debuffs.COLD_BEANS:
                cam.GetComponent<CameraShake>().StartShake();
                break;
            case Debuffs.INVERTED:
                players[0].GetComponent<PlayerManager>().invert = -1.0f;
                players[1].GetComponent<PlayerManager>().invert = -1.0f;
                break;
            case Debuffs.BIG_SPIKES:
                spikes.GetComponent<Transform>().localScale =
                    new Vector3(spikesScale.x, spikesScale.y * spikesScaleMod, 1.0f);
                break;
            case Debuffs.TOMATO:
                TT[0].StartTomoto();
                TT[1].StartTomoto();
                break;
        }
    }

    private void KingsClearDebuffs(Debuffer db)
    {
        switch (db.debuff)
        {
            case Debuffs.COLD_BEANS:
                cam.GetComponent<CameraShake>().ShakeDone();
                break;
            case Debuffs.INVERTED:
                players[0].GetComponent<PlayerManager>().invert = 1.0f;
                players[1].GetComponent<PlayerManager>().invert = 1.0f;
                break;
            case Debuffs.BIG_SPIKES:
                spikes.GetComponent<Transform>().localScale = spikesScale;
                break;
            case Debuffs.TOMATO:
                TT[0].StopTomoto();
                TT[1].StopTomoto();
                break;
        }
    }


    private float SetTime(Debuffs debuff)
    {
        float tempDuration = defaultTimeOfDebuff;
        foreach (var newTime in SpecialDebuffTime)
        {
            if (debuff != newTime.type) continue;
            tempDuration = newTime.time;
            break;
        }

        return tempDuration;
    }


    public void SetKingsDebuff()
    {
        ++kingDebuffCount;

        Debuffs debuff = kingsDebuffs[Random.Range(0, kingsDebuffs.Length - 1)];
        if (debuff == prevDebuffKing)
        {
            ++debuff;
            if ((int)(debuff) == kingsDebuffs.Length)
                debuff -= 2;
        }

        prevDebuffKing = debuff;
        debuffs.Add(new Debuffer(true, SetTime(debuff), CrowdPleaser.Player.PLAYER_ONE, debuff));
        theKingsServant.SetDebuffAll(debuff);
        KingsDebuffs(debuffs.Last());
    }

    public void SetPlayerDebuff(CrowdPleaser.Player player)
    {
        Debuffs debuff = playersDebuffs[Random.Range(0, playersDebuffs.Length - 1)];
        if (debuff == prevDebuffPlayers)
        {
            ++debuff;
            if ((int)(debuff) == playersDebuffs.Length)
                debuff -= 2;
        }

        player = player == CrowdPleaser.Player.PLAYER_ONE
            ? CrowdPleaser.Player.PLAYER_TWO
            : CrowdPleaser.Player.PLAYER_ONE;
        if (player == CrowdPleaser.Player.PLAYER_TWO)
        {
            ++playerOneDebuffCount;
        }
        else
        {
            ++playerTwoDebuffCount;
        }
        theKingsServant.SetDebuffPlayer(player, debuff);


        debuffs.Add(new Debuffer(false, SetTime(debuff), player, debuff));
        PlayersDebuffs(debuffs.Last());
    }
}
