using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 親のボタンスクリプト
public class ButtonScript : MonoBehaviour {

    public ButtonScript button;
	
    public void OnClick()
    {
        if(button == null)
        {
            throw new System.Exception("Button instance is null");
        }

        button.OnClick(this.gameObject.name);

        Debug.Log("押された");
    }

    protected virtual void OnClick(string name)
    {
        //
        Debug.Log("base button");
    }
}
