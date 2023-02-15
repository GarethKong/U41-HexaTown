#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Custom;
using DG.Tweening;
using JetBrains.Annotations;
using Mkey;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shape
{
    public int ro { get; set; }
    public int co { get; set; }
}

public class Matrix2D<T>
{
    public Dictionary<int, Dictionary<int, T>> rows;
    
    public Matrix2D()
    {
        rows = new Dictionary<int, Dictionary<int, T?>>();
    }

    public void set(int row, int col, T? obj)
    {
        Dictionary<int, T> r;
        if (!rows.TryGetValue(row, out r))
        {
            r = new Dictionary<int, T?>();
            rows[row] = r;
        }

        r[col] = obj;
    }

    public T? get(int row, int col)
    {
        Dictionary<int, T?> r;
        if (rows.TryGetValue(row, out r))
        {
            T? value;
            if (r.TryGetValue(col, out value))
            {
                return value;
            }
            else
            {
                return default(T);
                ;
            }
        }

        return default(T);
        ;
    }

    public void delete(int row, int col)
    {
        var r = this.rows[row];
        if (r != null)
        {
            r.Remove(col);
        }
    }

    public bool has(int row, int col)
    {
        Dictionary<int, T?> r;
        if (rows.TryGetValue(row, out r))
        {
            return r.ContainsKey(col);
        }

        return false;
    }
}


public class Queue<T>
{
    [ItemCanBeNull] Dictionary<int, T> data;
    int head;
    int tail;

    public Queue()
    {
        this.data = new Dictionary<int, T?>();
        this.head = 0;
        this.tail = 0;
    }

    public void enq(T? item)
    {
        data[this.tail] = item;
        tail++;
    }

    public T deq()
    {
        if (data.ContainsKey(head))
        {
            var item = this.data[this.head];
            data.Remove(this.head);
            head++;
            return item;
        }

        return default;
    }

    public int size()
    {
        return this.tail - this.head;
    }
}


public class Utils
{
    public static float HEX_HEIGHT = 1.15f;
    public static float HEX_WIDTH = 1; //can 3 chia 2
    public static float d_row = 1 * .8660254f; //52.5
    public static float d_col = HEX_WIDTH; // 60.622


    public static void setColorAlphaGO(Transform transform, float alpha)
    {
        if (transform.TryGetComponent(out SpriteRenderer sp))
        {
            var colorTemp = sp.color;
            colorTemp.a = alpha;
            sp.color = colorTemp;
        }
    }


    public static void setColorAlpha(SpriteRenderer sp, float alpha)
    {
        var colorTemp = sp.color;
        colorTemp.a = alpha;
        sp.color = colorTemp;
    }

    public static void setColorAlphaImage(Image sp, float alpha)
    {
        var colorTemp = sp.color;
        colorTemp.a = alpha;
        sp.color = colorTemp;
    }

    public static float getY(int row, int col)
    {
        return (row - 5) * d_row;
    }

    public static float getX(int row, int col)
    {
        return (col + 0.5f * row - 7.5f) * d_col;
    }

    public static int getRow(float x, float y)
    {
        return (int)MathF.Floor(0.5f + y / d_row) + 5;
    }

    public static int getCol(float x, float y)
    {
        return (int)MathF.Floor(x / d_col - 0.5f * (getRow(x, y) - 5.5f)) + 5;
    }

    public static char pick(char[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static void Shuffle<T>(IList<T> list)
    {
        var rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
    
    static public Vector2 WorldToCanvasPosition(Transform worldObj, RectTransform canvas, Camera cam = null) {

        if (!cam) {
            cam = Camera.main;
        }

        Vector2 ViewportPosition = cam.WorldToViewportPoint(worldObj.position);
        Vector2 WorldObject_CanvasPosition = new Vector2(
            ((ViewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f)));

        return WorldObject_CanvasPosition;
    }

   public static IEnumerator fadeInAndOut(GameObject objectToFade, bool fadeIn, float duration)
    {
        yield return new WaitForSeconds(0.5f);
        
        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = 1;
            b = 0;
        }

        int mode = 0;
        Color currentColor = Color.clear;

        TextMeshProUGUI textMeshPro = objectToFade.GetComponent<TextMeshProUGUI>();
        SpriteRenderer tempSPRenderer = objectToFade.GetComponent<SpriteRenderer>();
        Image tempImage = objectToFade.GetComponent<Image>();
        RawImage tempRawImage = objectToFade.GetComponent<RawImage>();
        MeshRenderer tempRenderer = objectToFade.GetComponent<MeshRenderer>();
        Text tempText = objectToFade.GetComponent<Text>();

  
        
        if (tempSPRenderer != null)
        {
            currentColor = tempSPRenderer.color;
            mode = 0;
        }
        //Check if Image
        else if (tempImage != null)
        {
            currentColor = tempImage.color;
            mode = 1;
        }
        //Check if RawImage
        else if (tempRawImage != null)
        {
            currentColor = tempRawImage.color;
            mode = 2;
        }
        //Check if Text 
        else if (tempText != null)
        {
            currentColor = tempText.color;
            mode = 3;
        }else if (textMeshPro != null)
        {
            currentColor = textMeshPro.color;
            mode = 5;
        }
        //Check if 3D Object
        else if (tempRenderer != null)
        {
            currentColor = tempRenderer.material.color;
            mode = 4;

            //ENABLE FADE Mode on the material if not done already
            tempRenderer.material.SetFloat("_Mode", 2);
            tempRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tempRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            tempRenderer.material.SetInt("_ZWrite", 0);
            tempRenderer.material.DisableKeyword("_ALPHATEST_ON");
            tempRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            tempRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            tempRenderer.material.renderQueue = 3000;
        }
        else
        {
            yield break;
        }

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            switch (mode)
            {
                case 0:
                    tempSPRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 1:
                    tempImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 2:
                    tempRawImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 3:
                    tempText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 4:
                    tempRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
                case 5:
                    textMeshPro.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                    break;
            }

            yield return null;
        }
    }
}