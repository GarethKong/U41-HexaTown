using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using JetBrains.Annotations;
using Mkey;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Custom
{
    public class ScorePopper : MonoBehaviour
    {
        public int points;
        public List<Hex> hexes;

        public ScorePopper(List<Hex> hexes, int points)
        {
            // find avg position
            float xSum = 0;
            float ySum = 0;
            foreach (var hex in hexes)
            {
                xSum += hex.transform.position.x;
                ySum += hex.transform.position.y;
            }

            //this.node.parent = null;
            this.points = points;
            this.hexes = hexes;
        }

        public void pop()
        {
            GameObject node = new GameObject();

            TextMeshPro lbScore = gameObject.AddComponent(typeof(TextMeshPro)) as TextMeshPro;

            lbScore.text = points > 0 ? "+ " + points + "" : points + "";
            lbScore.fontSize = 40;
            lbScore.font = GameManager.Instance.scoreTextMain.font;
            node.transform.parent = hexes[0].transform.parent;
            node.transform.position = hexes[0].transform.position;
            //TODO TWEEN cc.tween(node)
            //     .to(2, {position: new cc.Vec3(node.x, node.y + HEX_HEIGHT * 2, 0), opacity: 0})
            // .start()
        }
    }
}