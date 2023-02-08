using System.Drawing;
using System.Numerics;

public class GameConfig
{
    static Size BlockSize  = new Size(192, 166);
    static float TouchBlockThreshold = 0.03f;
    static int ReviveTimes = 0;

    public static int sessionNumber = 0;

    private static Vector2[] BlockPosAround = new Vector2[]
    {
        new Vector2(192, 0),
        new Vector2(96, 166.28f),
        new Vector2(-96, 166.28f),
        new Vector2(-192, 0),
        new Vector2(-96, -166.28f),
        new Vector2(96, -166.28f),
    };


   public  static Vector2 DynamicPos  = new Vector2(0, -8);
    
    int[,] IndexAround = new int[6,5]
    {
        { 0, -1, -1, -1, -1 }, //left
        {3, 4, 4, 3, 0 }, // upleft
        { 4, 5, 5, 4, 0 },// upright
        {1, 1, 1, 1, 0}, //right
        {0, -3, -4, -4, -3}, // downright
        {0, -4, -5, -5, -4}// downleft
    };

    public static int[] LineCount = {3, 4, 5, 4, 3};
    public static int  TrihexDeckNum  = 25;
    public static Vector2 DraggingScale = new Vector2(1.5f, 3f); 
    public static Vector2 propeller_hillPos  = new Vector2(0, 0.25f);
    public static Vector2 BoardNodeOffset  = new Vector2(0, 0);

    public static Size captureSize = new Size(667, 376);
   
}