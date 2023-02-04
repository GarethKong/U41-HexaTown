#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Custom;
using DG.Tweening;
using JetBrains.Annotations;
using Mkey;
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
            if (r.TryGetValue(col, out  value))
            {
                return value;
            }
            else
            {
                return default(T);;
            }
        }
        return default(T);;
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
        Dictionary<int,T?> r;
        if (rows.TryGetValue(row, out  r))
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
            this.data.Remove(this.head);
            this.head++;
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
        return (row - 5)*d_row;
    }

    public static float getX(int row, int col)
    {
        return (col + 0.5f * row - 7.5f)*d_col ;
    }

    public static int getRow(float x, float y)
    {
        return (int)MathF.Floor(0.5f + y/d_row) + 5;
    }

    public static int getCol(float x, float y)
    {
        return (int)MathF.Floor(x/d_col  - 0.5f * (getRow(x, y) - 5.5f)) + 5;
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
}