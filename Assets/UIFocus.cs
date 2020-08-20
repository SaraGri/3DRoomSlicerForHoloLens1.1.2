using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class UIFocus : MonoBehaviour, IFocusable {
    public GameObject ui;
    private UIController uiController;
    // Use this for initialization
    void Start () {
        uiController = ui.GetComponent<UIController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnFocusEnter()
    {
        uiController.isAnyUIObjectInFocus = true;
        //throw new System.NotImplementedException();
    }

    public void OnFocusExit()
    {
        uiController.isAnyUIObjectInFocus = false;
        //throw new System.NotImplementedException();
    }
}
