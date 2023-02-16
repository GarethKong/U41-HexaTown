using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimation : MonoBehaviour
{
    
    private Animator myAnimator;
    private Animation anim;

    public static TutorialAnimation Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
    }
    
    public void PlayAnimationTouch(int nameStep)
    {
        if (nameStep<2)
        {
            GameManager.Instance.HiddenHandTut(false);
            StartCoroutine(EAnimationTouch(nameStep));
        }
    }

    public void PlayAnimationMove(int nameStep, bool isFirst = false)
    {
        if (isFirst)
        {
            GameManager.Instance.HiddenHandTut(false);
        }
        StartCoroutine(EAnimationMove(nameStep));
    }

    private IEnumerator EAnimationMove(int nameStep)
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.HiddenHandTut(true);
        nameStep++;
        myAnimator.Play($"hand{nameStep}Move",0 ,0.0f);
        Debug.Log("BINH Move name step" + nameStep);
    }
    
    private IEnumerator EAnimationTouch(int nameStep)
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.HiddenHandTut(true);
        nameStep++;
        myAnimator.Play($"hand{nameStep}Touch",0 ,0.0f);
        Debug.Log("BINH Touch name step" + nameStep);
    }

    public void PlayAnimationDestroy(int nameStep)
    {
        Debug.Log("BINH Destroy name step" + nameStep);
        nameStep++;
        myAnimator.Play($"hand{nameStep}Destroy", 0, 0.0f);
}
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
