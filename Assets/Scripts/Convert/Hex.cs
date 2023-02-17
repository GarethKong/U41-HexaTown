using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using JetBrains.Annotations;
using Mkey;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Custom
{
    public class Hex : MonoBehaviour
    {
        public Transform eEdge;
        public Transform neEdge;
        public Transform nwEdge;
        public Transform wEdge;
        public Transform swEdge;
        public Transform seEdge;
        public Transform edges;
        public Transform propeller;
        public SpriteRenderer img;
        public int row;
        public int col;
        public Label textType;

        public EHexType hexType;
        public bool hasHill = false;
        public bool counted = false;
        public bool upgraded = false;

        public int getCol()
        {
            return col;
        }

        public void initGrid(float x, float y, int row, int col)
        {
            var transform1 = transform;
            transform1.position = new Vector3(x, y);
            transform1.localScale = new Vector3(1.15f, 1.15f);
            this.row = row;
            this.col = col;
            this.hexType = 0;
            this.hasHill = false;
            this.counted = false;
            this.upgraded = false;
        }

        void embiggen()
        {
            this.eEdge.localScale = new Vector3(0.75f, 0.75f);
            this.neEdge.localScale = new Vector3(0.75f, 0.75f);
            this.nwEdge.localScale = new Vector3(0.75f, 0.75f);
            this.wEdge.localScale = new Vector3(0.75f, 0.75f);
            this.swEdge.localScale = new Vector3(0.75f, 0.75f);
            this.seEdge.localScale = new Vector3(0.75f, 0.75f);
            //this.edges.opacity = 255; 
            this.propeller.localScale = new Vector3(0.75f, 0.75f);
            this.transform.localScale = new Vector3(0.75f, 0.75f);
        }


        public void setType(EHexType hexType)
        {
            //set spFrame by hexType
            this.hexType = hexType;
            Debug.Log("setType: " + hexType);
            img.sprite = SpriteMgr.Instance.hexList[(int)hexType];
            transform.name = img.sprite.name;
            propeller.gameObject.SetActive((int)hexType == 1);
            if ((int)hexType == 1)
            {
                img.sprite = SpriteMgr.Instance.hexList[(int)hexType + (hasHill ? 5 : 0)];
                if (hasHill)
                {
                    //propeller.localScale = new Vector3(0.5f, 0.5f);
                    var position = propeller.transform.position;
                    var pos = new Vector3(position.x + GameConfig.propeller_hillPos.X,
                        position.y + GameConfig.propeller_hillPos.Y);
                    propeller.position = pos;
                }
            }

            if ((int)hexType == 5)
            {
                edges.gameObject.SetActive(false);
            }
        }

        public void setHill(bool hasHill)
        {
            this.hasHill = hasHill;
            if (hasHill)
            {
                img.sprite = SpriteMgr.Instance.hexList[9];
            }
        }

        public void upgrade()
        {
            upgraded = true;
            if ((int)hexType == 2)
            {
                //set spFrame 'tree'
                img.sprite = SpriteMgr.Instance.hexList[(int)hexType + 5];
            }
            else if ((int)hexType == 3)
            {
                //set spFrame 'house'
                img.sprite = SpriteMgr.Instance.hexList[(int)hexType + 5];
            }
            else if ((int)hexType == 5)
            {
                //set spFrame 'port'
                img.sprite = SpriteMgr.Instance.hexList[(int)hexType + 5];
            }

            var effectUpgrade = Instantiate(SpriteMgr.Instance.effectUpgrade, transform.position, Quaternion.identity);
            effectUpgrade.transform.parent = GameManager.Instance.effectNode.transform;
        }

        void setSketchy(bool isSketchy)
        {
            var hexType = this.hexType + (upgraded ? 5 : 0);
            if (isSketchy)
            {
                img.sprite = SpriteMgr.Instance.sketchyList[(int)hexType];
            }
            else
            {
                img.sprite = SpriteMgr.Instance.hexList[(int)hexType];
            }
        }

        void setFrame(int hexType)
        {
            //set spFrame by hexType
            img.sprite = SpriteMgr.Instance.hexList[hexType];
        }


        void Update()
        {
            if (propeller.gameObject.activeSelf)
            {
                var speed = (this.hasHill && this.counted) ? 2.2f : this.counted ? 1f : 0.1f;
                propeller.transform.eulerAngles += new Vector3(0, 0, speed * 100f * Time.deltaTime);
            }
        }

        #region trọng

        public void setColorHex(Color color)
        {
            img.color = color;
        }

        #endregion
    }
}