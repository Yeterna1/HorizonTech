using System;
using UnityEngine;

public class BackpackUI : MonoBehaviour {

    [SerializeField] private GameObject BackpackObj;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            BackpackObj.SetActive(!BackpackObj.activeSelf);
        }
    }
}
