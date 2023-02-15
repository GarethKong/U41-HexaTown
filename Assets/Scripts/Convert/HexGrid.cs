using System;
using System.Collections;
using System.Collections.Generic;
using Mkey;
using UnityEngine;
using Utilities;
using Random = System.Random;

namespace Custom
{
    public class HexGrid : MonoBehaviour
    {
        static Shape[] a =
        {
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = -1 },
            new() { ro = 1, co = 0 }
        };

        static Shape[] v =
        {
            new() { ro = 0, co = 0 },
            new() { ro = 0, co = 1 },
            new() { ro = 1, co = 0 }
        };

        static Shape[] suotTrai =
        {
            new() { ro = -1, co = 0 },
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = 0 }
        };

        static Shape[] dautru =
        {
            new() { ro = 0, co = -1 },
            new() { ro = 0, co = 0 },
            new() { ro = 0, co = 1 }
        };

        static Shape[] suotPhai =
        {
            new() { ro = -1, co = 1 },
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = -1 }
        };

        static Shape[] c =
        {
            new() { ro = -1, co = 1 },
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = 0 }
        };

        static Shape[] r =
        {
            new() { ro = 0, co = 1 },
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = -1 }
        };

        static Shape[] n =
        {
            new() { ro = 0, co = -1 },
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = 0 }
        };

        static Shape[] d =
        {
            new() { ro = -1, co = 0 },
            new() { ro = 0, co = 0 },
            new() { ro = 1, co = -1 }
        };

        static Shape[] j =
        {
            new() { ro = -1, co = 1 },
            new() { ro = 0, co = 0 },
            new() { ro = 0, co = -1 }
        };

        static Shape[] l =
        {
            new() { ro = -1, co = 0 },
            new() { ro = 0, co = 0 },
            new() { ro = 0, co = 1 }
        };

        public static Dictionary<char, Shape[]> shapes = new()
        {
            { 'a', a },
            { 'v', v },
            { '\\', suotTrai },
            { '-', dautru },
            { '/', suotPhai },
            { 'c', c },
            { 'r', r },
            { 'n', n },
            { 'd', d },
            { 'j', j },
            { 'l', l },
        };

        public static Dictionary<char, List<char>> rotations = new()
        {
            { 'a', new List<char> { 'a', 'v' } },
            { 'v', new List<char> { 'a', 'v' } },
            { '\\', new List<char> { '\\', '-', '/' } },
            { '-', new List<char> { '\\', '-', '/' } },
            { '/', new List<char> { '\\', '-', '/' } },
            { 'c', new List<char> { 'c', 'r', 'n', 'd', 'j', 'l' } },
            { 'r', new List<char> { 'c', 'r', 'n', 'd', 'j', 'l' } },
            { 'n', new List<char> { 'c', 'r', 'n', 'd', 'j', 'l' } },
            { 'd', new List<char> { 'c', 'r', 'n', 'd', 'j', 'l' } },
            { 'j', new List<char> { 'c', 'r', 'n', 'd', 'j', 'l' } },
            { 'l', new List<char> { 'c', 'r', 'n', 'd', 'j', 'l' } },
        };

        Matrix2D<Hex> gridBoard;
        List<Hex> hexes;
        public List<GameObject> triPreviews = new List<GameObject>();
        int score;
        private Queue<ScorePopper> scoreQueue;


        bool enabled;
        int size;

        [SerializeField] public GameObject edgesNode;

        public Action onQueueEmpty;
        public Action<int> onScoreUpdate;

        public void init(int size, int hills, float posX, float posY, Action<int> onScoreUpdate)
        {
            enabled = true;
            this.gridBoard = new Matrix2D<Hex>();
            this.hexes = new List<Hex>();
            this.size = size;
            this.score = 0;
            this.onScoreUpdate = onScoreUpdate;

            this.scoreQueue = new Queue<ScorePopper>();

            this.transform.position = new Vector3(0, 3f);

            for (var r = 0; r < size + size + 1; r++)
            {
                for (var c = 0; c < size + size + 1; c++)
                {
                    if (c + r < size || c + r > size * 3)
                    {
                        continue;
                    }
                    else
                    {
                        var hexNode = Instantiate(GameManager.Instance.HexTilePrefab);
                        hexNode.transform.parent = gameObject.transform;

                        if (hexNode.TryGetComponent(out Hex hex))
                        {
                            var position = transform.position;
                            hex.initGrid(Utils.getX(r, c) + position.x, Utils.getY(r, c) + position.y, r, c);
                            gridBoard.set(r, c, hex);
                            hexes.Add(hex);
                        }
                    }
                }
            }

            gridBoard.get(0, size)?.setType(EHexType.portbw);
            gridBoard.get(0, size)!.transform.eulerAngles = Vector3.forward * 180f;

            gridBoard.get(0, size * 2)?.setType(EHexType.portbw);
            gridBoard.get(0, size * 2)!.transform.eulerAngles = Vector3.forward * -120;

            gridBoard.get(size, 0)?.setType(EHexType.portbw);
            gridBoard.get(size, 0)!.transform.eulerAngles = Vector3.forward * 120;

            gridBoard.get(size, size * 2)?.setType(EHexType.portbw);
            gridBoard.get(size, size * 2)!.transform.eulerAngles = Vector3.forward * -60;

            gridBoard.get(size, size)?.setType(EHexType.center);

            gridBoard.get(size * 2, 0)?.setType(EHexType.portbw);
            gridBoard.get(size * 2, 0)!.transform.eulerAngles = Vector3.forward * 60;

            gridBoard.get(size * 2, size)?.setType(EHexType.portbw);
            gridBoard.get(size * 2, size)!.transform.eulerAngles = Vector3.forward * 0;

            var placed = 0;
            Random rnd = new Random();

            if (!GameManager.Instance.isTutorial)
            {
                while (placed < hills)
                {
                    var r = rnd.Next((size + size + 1));
                    var c = rnd.Next((size + size + 1));
                    var h = this.gridBoard.get(r, c);
                    if (CheckAroundHill(h, r, c))
                    {
                        h.setHill(true);
                        placed += 1;
                    }
                }
            }

            updateEdges();

            triPreviews = new List<GameObject>();
            for (var i = 0; i < 3; i++)
            {
                GameObject whiteNode = new GameObject();
                whiteNode.transform.parent = GameManager.Instance.dynamicPreview.transform;
                SpriteRenderer sr = whiteNode.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.sortingLayerID = SortingLayer.NameToID("Game");
                sr.sortingOrder = 2;
                whiteNode.name = "dynamic" + "i";
                var localScale = whiteNode.transform.localScale;
                var color = sr.color;
                color.a = 1f;
                sr.color = color;

                var edge = Instantiate(this.edgesNode, whiteNode.transform, true);
                edge.transform.parent = whiteNode.transform;

                var redNode = new GameObject();
                redNode.transform.parent = whiteNode.transform;
                redNode.name = "redNode";

                SpriteRenderer redSr = redNode.AddComponent<SpriteRenderer>() as SpriteRenderer;
                redSr.sortingLayerID = SortingLayer.NameToID("Game");
                redSr.sortingOrder = 3;
                redSr.sprite = SpriteMgr.Instance.redSpriteFrame;
                // redNode.opacity = 255;
                redNode.active = false;
                localScale = new Vector3(1.15f, 1.15f);
                whiteNode.transform.localScale = localScale;
                triPreviews.Add(whiteNode);
            }

            InvokeRepeating("nextPopper", 0f, 0.1f);
        }


        bool CheckAroundHill(Hex h, int r, int c)
        {
            bool isCanAdd = false;
            if (h != null)
            {
                if (!h.hasHill && h.hexType == EHexType.empty)
                {
                    if (IsGridboardEmpty(r, c + 1) &&
                        IsGridboardEmpty(r - 1, c + 1) &&
                        IsGridboardEmpty(r - 1, c) &&
                        IsGridboardEmpty(r, c - 1) &&
                        IsGridboardEmpty(r + 1, c - 1) &&
                        IsGridboardEmpty(r + 1, c))
                    {
                        isCanAdd = true;
                    }
                }
            }

            return isCanAdd;
        }

        bool IsGridboardEmpty(int row, int columnn)
        {
            bool isEmpty = true;
            if (gridBoard.has(row, columnn))
            {
                if (gridBoard.get(row, columnn) != null)
                {
                    if (gridBoard.get(row, columnn)!.hasHill)
                    {
                        isEmpty = false;
                    }
                }
            }

            return isEmpty;
        }

        void updateEdges()
        {
            var grid = this.gridBoard;
            for (var r = 0; r < size + size + 1; r++)
            {
                for (var c = 0; c < this.size + this.size + 1; c++)
                {
                    if (grid.has(r, c))
                    {
                        var hex = grid.get(r, c);

                        // East edge //right
                        if (grid.has(r, c + 1) && grid.get(r, c + 1)!.hexType != EHexType.portbw)
                            Utils.setColorAlphaGO(hex.eEdge, 0);
                        else
                            Utils.setColorAlphaGO(hex.eEdge, 1);

                        // South East edge //up-right
                        if (grid.has(r - 1, c + 1) && grid.get(r - 1, c + 1)!.hexType != EHexType.portbw)
                            Utils.setColorAlphaGO(hex.seEdge, 0);
                        else
                            Utils.setColorAlphaGO(hex.seEdge, 1);

                        // South West edge //up-left
                        if (grid.has(r - 1, c) && grid.get(r - 1, c)!.hexType != EHexType.portbw)
                            Utils.setColorAlphaGO(hex.swEdge, 0);
                        else
                            Utils.setColorAlphaGO(hex.swEdge, 1);

                        // West edge //left
                        if (grid.has(r, c - 1) && (grid.get(r, c - 1)!.hexType == hex.hexType ||
                                                   (hex.hexType == EHexType.center &&
                                                    grid.get(r, c - 1)!.hexType == EHexType.street) ||
                                                   (hex.hexType == EHexType.street &&
                                                    grid.get(r, c - 1)!.hexType == EHexType.center)))
                            Utils.setColorAlphaGO(hex.wEdge, 0.4f);
                        else
                            Utils.setColorAlphaGO(hex.wEdge, 1);

                        // North West edge //down-left
                        if (grid.has(r + 1, c - 1) && (grid.get(r + 1, c - 1)!.hexType == hex.hexType ||
                                                       (hex.hexType == EHexType.center &&
                                                        grid.get(r + 1, c - 1)!.hexType == EHexType.street) ||
                                                       (hex.hexType == EHexType.street &&
                                                        grid.get(r + 1, c - 1)!.hexType == EHexType.center)))
                            Utils.setColorAlphaGO(hex.nwEdge, 0.4f);
                        else
                            Utils.setColorAlphaGO(hex.nwEdge, 1);

                        // North East edge //down-right
                        if (grid.has(r + 1, c) && (grid.get(r + 1, c)!.hexType == hex.hexType ||
                                                   (hex.hexType == EHexType.center &&
                                                    grid.get(r + 1, c)!.hexType == EHexType.street) ||
                                                   (hex.hexType == EHexType.street &&
                                                    grid.get(r + 1, c)!.hexType == EHexType.center)))
                            Utils.setColorAlphaGO(hex.neEdge, 0.4f);
                        else
                            Utils.setColorAlphaGO(hex.neEdge, 1);
                    }
                }
            }
        }

        public void sinkBlanks()
        {
            SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.splash, 0, 0.75f, null);
            foreach (var h in this.hexes)
            {
                if (h.hexType == EHexType.empty || (h.hexType == EHexType.portbw && !h.counted))
                {
                    h.gameObject.SetActive(false);
                    h.edges.gameObject.SetActive(false);
                    gridBoard.delete(h.row, h.col);
                }
            }

            updateEdges();
        }

        public bool canPlaceShape(char shape)
        {
            foreach (var hex in hexes)
            {
                if (hex.hexType == EHexType.empty)
                {
                    foreach (var rotation in rotations[shape])
                    {
                        var canPlaceHere = true;
                        foreach (var offsets in shapes[rotation])
                        {
                            var r = hex.row + offsets.ro;
                            var c = hex.col + offsets.co;
                            if (!(gridBoard.has(r, c) && gridBoard.get(r, c).hexType == EHexType.empty))
                            {
                                canPlaceHere = false;
                            }
                        }

                        if (canPlaceHere) return true;
                    }
                }
            }

            return false;
        }

        List<Hex> neighbors(int row, int col)
        {
            List<Hex> hexes = new List<Hex>();
            hexes.Add(this.gridBoard.get(row, col + 1));
            hexes.Add(this.gridBoard.get(row - 1, col + 1));
            hexes.Add(this.gridBoard.get(row - 1, col));
            hexes.Add(this.gridBoard.get(row, col - 1));
            hexes.Add(this.gridBoard.get(row + 1, col - 1));
            hexes.Add(this.gridBoard.get(row + 1, col));
            return hexes;
        }

        public void deactivate()
        {
            this.enabled = false;
            this.triPreviews[0].active = false;
            this.triPreviews[1].active = false;
            this.triPreviews[2].active = false;
            for (var i = 0; i < 3; i++)
            {
                Destroy(this.triPreviews[i]);
                // this.triPreviews[i].removeFromParent();
                // this.triPreviews[i].destroy();
            }
        }

        private void nextPopper()
        {
            if (scoreQueue == null) return;
            if (scoreQueue.size() > 0)
            {
                var p = scoreQueue.deq();
                p.pop(this);

                score += p.points;
                // update this.score

                if (onScoreUpdate != null)
                {
                    onScoreUpdate(score);
                }

                if (p.hexes != null)
                {
                    foreach (var hex in p.hexes)
                    {
                        hex.upgrade();

                        if (p.points > 0)
                        {
                            //play particle fx
                        }
                    }

                    if (p.points > 0)
                    {
                        if (p.hexes[0].hexType == EHexType.street)
                        {
                            // play sound 'pop'
                            SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.pop, 0, 0.5f, null);
                        }

                        if (p.hexes[0].hexType == EHexType.grass)
                        {
                            // play sound 'tree'
                            SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.tree, 0, 0.5f, null);
                        }

                        if (p.hexes[0].hexType == EHexType.windmill)
                        {
                            if (p.hexes[0].hasHill)
                            {
                                // play sound 'windmill-hill'
                                SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.windmillhill, 0, 0.6f, null);
                            }
                            else
                            {
                                // play sound 'windmill'
                                SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.windmill, 0, 0.6f, null);
                            }
                        }

                        if (p.hexes[0].hexType == EHexType.portbw)
                        {
                            // play sound 'port'
                            SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.port, 0, 0.9f, null);
                        }
                    }
                }
            }
            else if (onQueueEmpty != null)
            {
                onQueueEmpty();
                onQueueEmpty = null;
            }
        }

        public void updateTriPreview(float posX, float posY, Trihex trihex, bool isUpdatePos = false)
        {
            if (!enabled) return;
            if (isUpdatePos)
            {
                GameManager.Instance.dynamicPreview.transform.position = new Vector3(posX, posY + 3f);
            }

            posX -= GameConfig.BoardNodeOffset.X;
            posY -= GameConfig.BoardNodeOffset.Y;
            var _row = Utils.getRow(posX, posY);
            var _col = Utils.getCol(posX, posY);
            // console.log("col, row = ", _col, _row, Math.round(x), Math.round(y));
            List<Hex> hexes = new List<Hex>();
            var touching = false;

            var dynamicPos = GameManager.Instance.dynamicPreview.transform.position;
            var _offsets = shapes[trihex.shape];
            for (var i = 0; i < 3; i++)
            {
                var offset = _offsets[i];
                var posX1 = (float)(offset.co + 0.5 * offset.ro) * Utils.d_col;
                var posY1 = offset.ro * Utils.d_row;
                hexes.Add(this.gridBoard.get(_row + offset.ro, _col + offset.co));
                triPreviews[i].transform.position = new Vector3(posX1 + dynamicPos.x, posY1 + dynamicPos.y);
                if (!touching)
                {
                    List<Hex> listNeighbors = this.neighbors(_row + offset.ro, _col + offset.co);
                    foreach (var n in listNeighbors)
                    {
                        if (n && (n.hexType == EHexType.windmill ||
                                  n.hexType == EHexType.grass ||
                                  n.hexType == EHexType.street ||
                                  n.hexType == EHexType.center))
                        {
                            touching = true;
                            break;
                        }
                    }
                }
            }

            if (touching && hexes[0] && hexes[0].hexType == EHexType.empty &&
                hexes[1] && hexes[1].hexType == EHexType.empty &&
                hexes[2] && hexes[2].hexType == EHexType.empty)
            {
                for (var i = 0; i < 3; i++)
                {
                    if (triPreviews[i].TryGetComponent(out SpriteRenderer sp))
                    {
                        sp.sprite = SpriteMgr.Instance.triPreviews[trihex.hexes[i]];
                        triPreviews[i].transform.Find("redNode").gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    if (triPreviews[i].TryGetComponent(out SpriteRenderer sp))
                    {
                        sp.sprite = SpriteMgr.Instance.triPreviews[trihex.hexes[i]];
                        triPreviews[i].transform.Find("redNode").gameObject.SetActive(GameManager.Instance.isMoving);
                    }
                }
            }
        }

        public bool placeTrihex(float posX, float posY, Trihex trihex)
        {
            posX -= 0;
            posY -= 3;
            var r = Utils.getRow(posX, posY);
            var c = Utils.getCol(posX, posY);

            List<Hex> hexes = new List<Hex>();
            var touching = false;
            for (var i = 0; i < 3; i++)
            {
                var offsets = shapes[trihex.shape][i];
                hexes.Add(this.gridBoard.get(r + offsets.ro, c + offsets.co));
    
                if (!touching)
                {
                    List<Hex> neighbors = this.neighbors(r + offsets.ro, c + offsets.co);
                    foreach (var n in neighbors)
                    {
                        if (n && n.hexType is EHexType.windmill or EHexType.grass or EHexType.street or EHexType.center)
                        {
                            touching = true;
                            break;
                        }
                    }
                }
            }

            if (GameManager.Instance.isTutorial)
            {
                if (touching && hexes[0] && hexes[0].hexType == EHexType.empty &&
                    hexes[0].img.color.Equals(Color.yellow) &&
                    hexes[1] && hexes[1].hexType == EHexType.empty &&
                    hexes[1].img.color.Equals(Color.yellow)&&
                    hexes[2] && hexes[2].hexType == EHexType.empty &&
                    hexes[2].img.color.Equals(Color.yellow) && GameManager.Instance.nextTrihex.CanPlaceOrNot(GameManager.Instance.indexStep))
                {
                    // play sound 'place'
                    SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.place, 0, 1f, null);

                    for (var i = 0; i < 3; i++)
                    {
                        hexes[i].setType((EHexType)trihex.hexes[i]);
                    }

                    // calculate scores
                    for (var i = 0; i < 3; i++)
                    {
                        if (hexes[i].hexType == EHexType.windmill)
                        {
                            getPointsFor(hexes[i]);
                        }
                    }

                    for (var i = 0; i < 3; i++)
                    {
                        if (hexes[i].hexType != EHexType.windmill)
                        {
                            getPointsFor(hexes[i]);
                        }
                    }

                    updateEdges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (touching && hexes[0] && hexes[0].hexType == EHexType.empty &&
                    hexes[1] && hexes[1].hexType == EHexType.empty &&
                    hexes[2] && hexes[2].hexType == EHexType.empty)
                {
                    // play sound 'place'
                    SoundMaster.Instance.SoundPlayByEnum(EAudioEffectID.place, 0, 1f, null);

                    for (var i = 0; i < 3; i++)
                    {
                        hexes[i].setType((EHexType)trihex.hexes[i]);
                    }

                    // calculate scores
                    for (var i = 0; i < 3; i++)
                    {
                        if (hexes[i].hexType == EHexType.windmill)
                        {
                            getPointsFor(hexes[i]);
                        }
                    }

                    for (var i = 0; i < 3; i++)
                    {
                        if (hexes[i].hexType != EHexType.windmill)
                        {
                            getPointsFor(hexes[i]);
                        }
                    }

                    updateEdges();
                    return true;
                }
                else
                    {
                        return false;
                    }
                }
            }
        // returns connected hexes INCLUDING itself
        List<Hex> getConnected(Hex hex)
        {
            List<Hex> connectedHexes = new List<Hex>();
            var visited = new HashSet<Hex>();
            var queue = new Queue<Hex>();
            queue.enq(hex);

            while (queue.size() > 0)
            {
                var h = queue.deq();
                if (!visited.Contains(h)) connectedHexes.Add(h);
                visited.Add(h);

                foreach (var n in this.neighbors(h.row, h.col))
                {
                    if (n && (n.hexType == h.hexType ||
                              (h.hexType == EHexType.street && n.hexType == EHexType.portbw)) && !visited.Contains(n))
                    {
                        queue.enq(n);
                    }
                }
            }

            return connectedHexes;
        }
        
        #region  Trọng

        public void initHill()
        {
            var hillTemp = this.gridBoard.get(4, 6);
            hillTemp.setHill(true);
        }

        public void SetYellowHexTut(int r, int c)
        {
            var Temp = this.gridBoard.get(r, c);
            Temp.setColorHex(Color.yellow);
        }
        
        public void RemoveYellowHexTut(int r, int c)
        {
            var Temp = this.gridBoard.get(r, c);
            Temp.setColorHex(Color.white);
        }

        #endregion

        void getPointsFor(Hex hex)
        {
            if (hex.counted) return;

            if (hex.hexType == EHexType.windmill)
            {
                var isolated = true;
                foreach (var h in this.neighbors(hex.row, hex.col))
                {
                    if (h && h.hexType == EHexType.windmill)
                    {
                        isolated = false;
                        if (h.counted)
                        {
                            h.counted = false;
                            scoreQueue.enq(new ScorePopper(new List<Hex> { h }, h.hasHill ? -3 : -1));
                        }
                    }
                }

                if (isolated)
                {
                    scoreQueue.enq(new ScorePopper(new List<Hex> { hex }, hex.hasHill ? 3 : 1));
                    hex.counted = true;
                }
            }
            else if (hex.hexType == EHexType.grass)
            {
                List<Hex> group = this.getConnected(hex);

                var uncountedParks = new List<Hex>();
                foreach (var park in group)
                {
                    if (!park.counted) uncountedParks.Add(park);
                }

                while (uncountedParks.Count >= 3)
                {
                    var newParks = SpliceExtension.Splice(uncountedParks, 0, 3);
                    newParks[0].counted = true;
                    newParks[1].counted = true;
                    newParks[2].counted = true;
                    uncountedParks.RemoveRange(0, 3);
                    scoreQueue.enq(new ScorePopper(newParks, 5));
                }
            }
            else if (hex.hexType == EHexType.street)
            {
                foreach (var h in this.neighbors(hex.row, hex.col))
                {
                    if (h && h.hexType == EHexType.center && !hex.counted)
                    {
                        scoreQueue.enq(new ScorePopper(new List<Hex> { hex }, 1));
                        hex.counted = true;
                    }
                }

                var group = this.getConnected(hex);
                var connectedToCenter = false;
                foreach (var h in group)
                {
                    if (h.hexType == EHexType.street && h.counted) connectedToCenter = true;
                }

                if (connectedToCenter)
                {
                    foreach (var h in group)
                    {
                        if (!h.counted)
                        {
                            scoreQueue.enq(new ScorePopper(new List<Hex> { h }, h.hexType == EHexType.portbw ? 3 : 1));
                            h.counted = true;
                        }
                    }
                }
            }
        }
    }
}