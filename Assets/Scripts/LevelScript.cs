using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{

    public SoundManager.SoundNames LevelMusic;
    public SoundManager.SoundNames[] BackgroundNoise;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager sm = SoundManager.Instance;
        sm.PlaySound(LevelMusic);

        foreach(SoundManager.SoundNames sn in BackgroundNoise)
        {
            sm.PlaySound(sn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
