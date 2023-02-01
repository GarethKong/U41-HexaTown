using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue: MonoBehaviour
{

    public GameObject popUpBox;

    public TMP_Text popUpText;


    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
    }

    public void DisablePopUp()
    {
        popUpBox.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        popUpBox = GetComponent<GameObject>();
        popUpText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
