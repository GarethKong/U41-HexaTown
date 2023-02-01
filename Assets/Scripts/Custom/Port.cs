using DG.Tweening;
using Mkey;
using UnityEngine;

namespace Custom
{
    public class Port : MonoBehaviour
    {
        public SpriteRenderer sp;
        private Vector2Int gridPosition;
        private LevelGrid _levelGrid;
        public bool isCanPass = false;

        public void Setup(int i, int j, LevelGrid levelGrid)
        {
            sp.sortingOrder = 49 - j;
            gridPosition = new Vector2Int(i, j);
            _levelGrid = levelGrid;
        }
        
        public void OpenDoor()
        {
            SoundMaster.Instance.SoundPlayByEnum(ESoundType.PortOpen, 0, null);
            sp.sprite =  GameAssets.Instance.portOpenSprite;
            isCanPass = true;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f), 0.1f).SetEase(Ease.InSine))
            .Append(transform.DOScale(new Vector3(0.8f, 0.8f), 0.2f))
            .Append(transform.DOScale(new Vector3(1f, 1f), 0.2f));
        }
        
        public void CloseDoor()
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(new Vector3(0.8f, 0.8f), 0.3f).SetEase(Ease.InSine));
            sp.sprite =  GameAssets.Instance.portCloseSprite;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }
    }
}