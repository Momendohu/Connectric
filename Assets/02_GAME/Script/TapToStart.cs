using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour {

    private GameObject gameManager;

    private int counter = 0;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        this.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.3f);

    }
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.3f + 0.7f * Mathf.Sin(((float)counter / 180) * 3.14f) );
        counter++;
        if(counter > 180)
        {
            counter = 0;
        }

        //if(gameManager.GetComponent<GameManager>().)

	}
}
