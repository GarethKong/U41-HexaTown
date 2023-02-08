using UnityEngine;
using System.Collections;
using TMPro;

public class FontManager : MonoBehaviour {
    public  TMP_FontAsset[] fonts;
   
    public static FontManager instance;
    public FontManager get {
        get {
            return instance;
        }
    }
 
    public void Awake() {
        instance = this;
    }
 
    public  TMP_FontAsset GetFont(string fontName) {
        foreach (TMP_FontAsset font in fonts)
            if (font.name == fontName)
                return font;
        return null;
    }
    
}