using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Platform_Manager_Script : MonoBehaviour
{
    public GameObject platform;
    public List<GameObject> leftsideSpawns;
    public List<GameObject> rightsideSpawns;
    public float spawnRate;
    private float spawnTime;
    public float platformSpeed;

    [SerializeField] private List<GameObject> spawnedPlatformsRight = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedPlatformsLeft = new List<GameObject>();

    [SerializeField] private Camera cam;

    [SerializeField] private float switchTimer = 0.0f;
    [SerializeField] private float switchTimerTheshold = 30.0f;

    [SerializeField] private bool isFlipped = false;


    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0.0f;
        if (cam == null)
            cam = FindObjectOfType<Camera>();
        SetPlatformPos(isFlipped);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnRate)
        {
            spawnTime = 0.0f;

            spawnedPlatformsLeft.Add(Instantiate(platform, leftsideSpawns[Random.Range(0, 5)].transform.position,
                Quaternion.identity));
            spawnedPlatformsLeft.Last().GetComponent<Rigidbody2D>().velocity = new Vector2(0, -platformSpeed);


            spawnedPlatformsRight.Add(Instantiate(platform, rightsideSpawns[Random.Range(0, 5)].transform.position,
                Quaternion.identity));
            spawnedPlatformsRight.Last().GetComponent<Rigidbody2D>().velocity = new Vector2(0, platformSpeed);
           

        }

        spawnedPlatformsRight.RemoveAll(item => item == null);
        spawnedPlatformsLeft.RemoveAll(item => item == null);

        var debuff = spawnedPlatformsLeft.First().GetComponent<PlatformDebuffs>().currentDebuff;
        spawnedPlatformsLeft.Last().GetComponent<PlatformDebuffs>().currentDebuff = debuff;
        debuff = spawnedPlatformsRight.First().GetComponent<PlatformDebuffs>().currentDebuff;
        spawnedPlatformsRight.Last().GetComponent<PlatformDebuffs>().currentDebuff = debuff;

        switchTimer += Time.deltaTime;
        if (switchTimer > switchTimerTheshold)
        {
            FlipPlatforms();
        }

    }

    public void SetDebuffAll(DebuffManager.Debuffs debuff)
    {
        foreach (var platRight in spawnedPlatformsRight)
        {
            platRight.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
        }

        foreach (var platLeft in spawnedPlatformsLeft)
        {
            platLeft.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
        }
    }

    public void ClearDebuffAll(DebuffManager.Debuffs debuf)
    {
        foreach (var platRight in spawnedPlatformsRight)
        {
            platRight.GetComponent<PlatformDebuffs>().ClearDebuff(debuf);
        }

        foreach (var platLeft in spawnedPlatformsLeft)
        {
            platLeft.GetComponent<PlatformDebuffs>().ClearDebuff(debuf);
        }
    }

    public void SetDebuffPlayer(CrowdPleaser.Player player, DebuffManager.Debuffs debuff)
    {
        if (player == CrowdPleaser.Player.PLAYER_ONE)
        {
            foreach (var platLeft in spawnedPlatformsLeft)
            {
                platLeft.GetComponent<PlatformDebuffs>().SetDebuff(DebuffManager.Debuffs.NONE);
            }
        }
        else
        {
            foreach (var platRight in spawnedPlatformsRight)
            {
                platRight.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
            }
        }
    }

    public void ClearDebuffPlayer(CrowdPleaser.Player player, DebuffManager.Debuffs debuff)
    {
        if (player == CrowdPleaser.Player.PLAYER_ONE)
        {
            foreach (var platLeft in spawnedPlatformsLeft)
            {
                platLeft.GetComponent<PlatformDebuffs>().ClearDebuff(DebuffManager.Debuffs.NONE);
            }
        }
        else
        {
            foreach (var platRight in spawnedPlatformsRight)
            {
                platRight.GetComponent<PlatformDebuffs>().ClearDebuff(debuff);
            }
        }
    }


    private void FlipPlatforms()
    {
        switchTimer = 0.0f;
        SetPlatformPos(!isFlipped);
        platformSpeed *= -1.0f;

        foreach (var platRight in spawnedPlatformsRight)
        {
            if(platRight != null)
                platRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, platformSpeed);
        }       
        foreach (var platLeft in spawnedPlatformsLeft)
        {
            if(platLeft != null)
                platLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -platformSpeed);
        }

    }

    private void SetPlatformPos(bool flip)
    {
        if (flip)
        {
            foreach (var rSpawn in rightsideSpawns)
            {
                Vector3 pos = rSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, (cam.orthographicSize + 0.5f), 0.0f);
                rSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
            foreach (var lSpawn in leftsideSpawns)
            {
                Vector3 pos = lSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, -(cam.orthographicSize + 0.5f), 0.0f);
                lSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
        }
        else
        {
            foreach (var rSpawn in rightsideSpawns)
            {
                Vector3 pos = rSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, -(cam.orthographicSize + 0.5f), 0.0f);
                rSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
            foreach (var lSpawn in leftsideSpawns)
            {
                Vector3 pos = lSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, (cam.orthographicSize + 0.5f), 0.0f);
                lSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
        }

        isFlipped = flip;
    }
}
