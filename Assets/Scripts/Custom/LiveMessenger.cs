using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class LiveMessenger : MonoBehaviour
{
    // Start is called before the first frame update

    public Text MessengerText;
    public Image Bg;
    // public Button btnClose;
    void Start()
    {
        MessengerText = GetComponent<Text>();
        Bg = GetComponent<Image>();
    }
    
    public void Show()
    {
        MessengerText.text ="Over" ;
        MessengerText.enabled = true;
    }

    public void Close()
    {
        MessengerText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
