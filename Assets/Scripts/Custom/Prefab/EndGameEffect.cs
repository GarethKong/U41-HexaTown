using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public class EndGameEffect : MonoBehaviour
{

    public GameObject effectRoot;
    
    public static EndGameEffect Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    
    void Start()
    {
        CustomEventManager.Instance.OnEndGame += OnActiveEffect;
    }

    public void OnActiveEffect()
    {
        effectRoot.SetActive(true);
    }

    public void CloseEffect()
    {
        effectRoot.SetActive(false);
    }
}