using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusic : MonoBehaviour {

    private GameObject gameManager;
    private AudioSource soundSE;
    private bool touch_seFlag = false;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        soundSE = GameObject.Find("TitleSE").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if(!touch_seFlag/* && gameManager.GetComponent<GameManager>().IsTitleAppeared*/)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                soundSE.PlayOneShot(soundSE.clip);
                touch_seFlag = true;
            }
        }
        
    }
}
