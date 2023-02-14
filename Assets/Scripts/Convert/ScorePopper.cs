using System;
using System.Collections;
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
    public class ScorePopper
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

        public void pop(MonoBehaviour monoBehaviour)
        {
            GameObject node = new GameObject();
            int layer = LayerMask.NameToLayer("UI");
            node.layer = layer;
            TextMeshProUGUI lbScore = node.AddComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            lbScore.text = points > 0 ? "+ " + points + "" : points + "";
            lbScore.fontSize = 60;
            lbScore.fontSize = 70;
            lbScore.alignment = TextAlignmentOptions.Center;
            lbScore.color = new Color(0f, 0f, 0f, 1f);
            lbScore.font = FontManager.instance.GetFont("mvboli SDF");
            lbScore.horizontalAlignment = HorizontalAlignmentOptions.Center;
            node.transform.SetParent(GameManager.Instance.scoreNode.transform);
            // var positionNew = Utils.WorldToCanvasPosition(hexes[0].transform,
            //     GameManager.Instance.canvas.transform as RectTransform, Camera.main);
            // node.transform.position = positionNew;
            //node.transform.position = Camera.main.WorldToScreenPoint(hexes[0].transform.position);
          

            Vector2 adjustedPosition =
                GameManager.Instance.uiCamera.WorldToScreenPoint(new Vector3(hexes[0].transform.position.x,
                    hexes[0].transform.position.y, 1));

            RectTransform rect = GameManager.Instance.canvas.GetComponent<RectTransform>();
            adjustedPosition.x *= rect.sizeDelta.x / (float)Camera.main.pixelWidth;
            adjustedPosition.y *= rect.sizeDelta.y / (float)Camera.main.pixelHeight;
            var alchorPos = adjustedPosition - rect.sizeDelta / 2f;
            Vector3 newPos = new Vector3(alchorPos.x, alchorPos.y, 3);
            node.transform.localScale = Vector3.one;
            node.GetComponent<RectTransform>().localPosition = newPos;
            node.transform.DOLocalMove(new Vector3(newPos.x, newPos.y + 50, 0), 2)
                .SetEase(Ease.InSine).OnComplete(() =>
                {
                });
            monoBehaviour.StartCoroutine(Utils.fadeInAndOut(node, false, 2f));
        }
    }
}