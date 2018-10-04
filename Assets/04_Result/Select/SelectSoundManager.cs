using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSoundManager : MonoBehaviour {

    private AudioSource selectSE;

    private void Start () {
        selectSE = GameObject.Find("SelectSE").GetComponent<AudioSource>();
	}
	
	private void Update () {
		
	}
}
