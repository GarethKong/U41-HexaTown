
/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBlock : MonoBehaviour, IPointerClickHandler{

    private static TouchBlock instance;

    public void OnPointerClick(PointerEventData data) {
        // T$$anonymous$$s will only execute if the objects collider was the first $$anonymous$$t by the click's raycast
        Debug.Log("BLOCK TOUCH BINH");
    }
}
