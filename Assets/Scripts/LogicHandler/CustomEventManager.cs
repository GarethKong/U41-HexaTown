using System;
using Custom;
using UnityEngine;

namespace Interface
{
    
    public class CustomEventManager : MonoBehaviour
    {
        public static CustomEventManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public Action OnStartMoving;
        public Action OnWinLevel;
        public Action OnFailLevel;
        public Action OnFoodDrop;
        public Action OnNextLevel;
        public Action OnFire;
        public Action OnStopFire;
        public Action OnEatBanana;
        
        public void Fire()
        {
            if (OnFire != null)
            {
                OnFire();
            }
        }
        
        public void EatBanana()
        {
            if (OnEatBanana != null)
            {
                OnEatBanana();
            }
        }
        
        public void StopFire()
        {
            if (OnStopFire != null)
            {
                OnStopFire();
            }
        }

        public void FoodDrop()
        {
            if (OnFoodDrop != null)
            {
                OnFoodDrop();
            }
        }

        public void StartMoving()
        {
            if (OnStartMoving != null)
            {
                OnStartMoving();
            }
        }

        public void WinLevel()
        {
            if (OnWinLevel != null)
            {
                OnWinLevel();
            }
        }
    
        public void FailLevel()
        {
            if (OnFailLevel != null)
            {
                OnFailLevel();
            }
        }
        
        public void NextLevel()
        {
            if (OnNextLevel != null)
            {
                OnNextLevel();
            }
        }
    
    }
}