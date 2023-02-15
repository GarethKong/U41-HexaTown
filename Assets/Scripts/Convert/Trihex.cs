using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using JetBrains.Annotations;
using Mkey;
using UnityEngine;
using UnityEngine.UI;

namespace Custom
{
    public class Trihex 
    {
        public int[] hexes;
        public char shape;

        public Trihex(int color1, int color2, int color3, char shape)
        {
            hexes = new [] { color1, color2, color3};
            this.shape = shape;
        }
        
       public void rotateLeft() {
            if (shape == 'a') {
                shape = 'v';
                (hexes[0], hexes[1]) = (hexes[1], hexes[0]);
            } else if (shape == 'v') {
                shape = 'a';
                (hexes[1], hexes[2]) = (hexes[2], hexes[1]);
            } else if (shape == '\\') {
                shape = '/';
            } else if (this.shape == '/') {
                this.shape = '-';
                (this.hexes[0], this.hexes[2]) = (this.hexes[2], this.hexes[0]);
            } else if (this.shape == '-') {
                this.shape = '\\';
            } else if (this.shape == 'c') {
                this.shape = 'r';
            } else if (this.shape == 'r') {
                this.shape = 'n';
                (this.hexes[0], this.hexes[2]) = (this.hexes[2], this.hexes[0]);
            } else if (this.shape == 'n') {
                this.shape = 'd';
            } else if (this.shape == 'd') {
                this.shape = 'j';
            } else if (this.shape == 'j') {
                this.shape = 'l';
                (this.hexes[0], this.hexes[2]) = (this.hexes[2], this.hexes[0]);
            } else if (this.shape == 'l') {
                this.shape = 'c';
            }
            
            
            //Trong Write check true tut hex with rotate left
            if (GameManager.Instance.Tutorial)
            {
                Debug.Log("steppppppp"+ GameManager.Instance.GetStep());
                Debug.Log(this.shape + "  type: " + this.hexes[0] + " type 1: " + this.hexes[1]);
                switch (GameManager.Instance.GetStep())
                {
                    case 0:
                        if (this.shape == 'a' && this.hexes[0] == 2 && this.hexes[1] == 1)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand1",true);
                            // GameManager.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            TutorialAnimation.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            Debug.Log("hand1 true");
                        }
                        break;
                    case 1:
                        if (this.shape == '-' && this.hexes[0] == 3 && this.hexes[1] == 3 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            TutorialAnimation.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            Debug.Log("hand2 true");
                        }
                        break;
                    case 2:
                        if (this.shape == 'c' && this.hexes[0] == 3 && this.hexes[1] == 3 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            TutorialAnimation.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            Debug.Log("hand4 true");
                        }
                        break;
                    
                    case 3:
                        if (this.shape == 'v' && this.hexes[0] == 2 && this.hexes[1] == 2 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            TutorialAnimation.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            Debug.Log("hand4 true");
                        }
                        break;
                    case 4:
                        if (this.shape == 'n' && this.hexes[0] == 1 && this.hexes[1] == 2 && this.hexes[2] == 2)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            TutorialAnimation.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            Debug.Log("hand5 true");
                        }
                        break;
                    case 5:
                        if (this.shape == 'd' && this.hexes[0] == 2 && this.hexes[1] == 1 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            TutorialAnimation.Instance.PlayAnimationMove(GameManager.Instance.GetStep());
                            Debug.Log("hand6 true");
                        }
                        break;
                }
            }
        }
       
       public void rotateRight() {
           if (this.shape == 'a') {
               this.shape = 'v';
               (this.hexes[1], this.hexes[2]) = (this.hexes[2], this.hexes[1]);
           } else if (this.shape == 'v') {
               this.shape = 'a';
               (this.hexes[0], this.hexes[1]) = (this.hexes[1], this.hexes[0]);
           } else if (this.shape == '\\') {
               this.shape = '-';
           } else if (this.shape == '/') {
               this.shape = '\\';
           } else if (this.shape == '-') {
               this.shape = '/';
               (this.hexes[0], this.hexes[2]) = (this.hexes[2], this.hexes[0]);
           } else if (this.shape == 'c') {
               this.shape = 'l';
           } else if (this.shape == 'r') {
               this.shape = 'c';
           } else if (this.shape == 'n') {
               this.shape = 'r';
               (this.hexes[0], this.hexes[2]) = (this.hexes[2], this.hexes[0]);
           } else if (this.shape == 'd') {
               this.shape = 'n';
           } else if (this.shape == 'j') {
               this.shape = 'd';
           } else if (this.shape == 'l') {
               this.shape = 'j';
               (this.hexes[0], this.hexes[2]) = (this.hexes[2], this.hexes[0]);
           }
           
           //Trong Write check true tut hex with rotate right
           if (GameManager.Instance.Tutorial)
           {
                Debug.Log("steppppppp"+ GameManager.Instance.GetStep());
                Debug.Log(this.shape + "  type: " + this.hexes[0] + " type 1: " + this.hexes[1]);
                switch (GameManager.Instance.GetStep())
                {
                    case 0:
                        if (this.shape == 'a' && this.hexes[0] == 2 && this.hexes[1] == 1)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand1",true);
                            Debug.Log("hand1 true");
                        }
                        break;
                    case 1:
                        if (this.shape == '-' && this.hexes[0] == 3 && this.hexes[1] == 3 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            Debug.Log("hand2 true");
                        }
                        break;
                    case 3:
                        if (this.shape == 'v' && this.hexes[0] == 2 && this.hexes[1] == 2 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            Debug.Log("hand4 true");
                        }
                        break;
                    case 4:
                        if (this.shape == 'n' && this.hexes[0] == 1 && this.hexes[1] == 2 && this.hexes[2] == 2)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            Debug.Log("hand5 true");
                        }
                        break;
                    case 5:
                        if (this.shape == 'd' && this.hexes[0] == 2 && this.hexes[1] == 1 && this.hexes[2] == 3)
                        {
                            // GameManager.Instance.SetBoolAnimation("hand2",true);
                            Debug.Log("hand6 true");
                        }
                        break;
                }
            }
       }
    }
}