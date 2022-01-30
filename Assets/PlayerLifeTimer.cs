using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeTimer : MonoBehaviour
{
    public PlayerManager playerManager;
    // Update is called once per frame
    void Update()
    {
        Text txt = gameObject.GetComponent<Text>();
        txt.text = playerManager.lifeTimer.ToString();
    }
}
