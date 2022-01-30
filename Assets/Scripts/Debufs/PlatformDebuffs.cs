using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Debuffs = DebuffManager.Debuffs;
public class PlatformDebuffs : MonoBehaviour
{
    public List<Debuffs> currentDebuff = new List<Debuffs>();

    private Transform tm;
    private Vector3 defaultScale;
    private float brittleTimer = 0.0f;
    private GameObject player;

    [SerializeField] private float brittleTime;
    [SerializeField] private float honeyJumpForceMod;
    [SerializeField] private Vector3 smolPlotform;
    [SerializeField] private bool playerIsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        tm = this.gameObject.GetComponent<Transform>();
        defaultScale = tm.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var VARIABLE in currentDebuff)
        {
            switch (VARIABLE)
            {
                case Debuffs.SMOL_PLOTFORM:
                    tm.localScale = smolPlotform;
                    break;
                case Debuffs.BRITTLE:
                    if (playerIsOn)
                    {
                        brittleTimer += Time.deltaTime;
                        if (brittleTimer >= brittleTime)
                            Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }

    public void ClearDebuff(Debuffs debuf)
    {
        currentDebuff.Remove(debuf);
        switch (debuf)
        {
            case Debuffs.SMOL_PLOTFORM:
                tm.localScale = defaultScale;
                break;
        }
    }

    public void SetDebuff(Debuffs debuf)
    {
        currentDebuff.Add(debuf);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.collider.CompareTag("Player"))
        {
            playerIsOn = true;
            player = collider.gameObject;
            foreach (var VARIABLE in currentDebuff)
            {
                switch (VARIABLE)
                {
                    case Debuffs.STICKY:
                        var script = player.GetComponent<PlayerManager>();
                        script.jumpForce = script.defaultJumpForce * honeyJumpForceMod;
                        break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.collider.CompareTag("Player"))
        {
            playerIsOn = false;
            var script = player.GetComponent<PlayerManager>();
            script.jumpForce = script.defaultJumpForce;
        }
    }
}
