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
            if (this.shape == 'a') {
                this.shape = 'v';
                (this.hexes[0], this.hexes[1]) = (this.hexes[1], this.hexes[0]);
            } else if (this.shape == 'v') {
                this.shape = 'a';
                (this.hexes[1], this.hexes[2]) = (this.hexes[2], this.hexes[1]);
            } else if (this.shape == '\\') {
                this.shape = '/';
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
       }
    }
}