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
        nameStep++;
        myAnimator.Play($"hand{nameStep}Touch",0 ,0.0f);
    }
    
    public void PlayAnimationMove(int nameStep)
    {
        nameStep++;
        myAnimator.Play($"hand{nameStep}Move",0 ,0.0f);
    }
    
    public void PlayAnimationDestroy(int nameStep)
    {
        nameStep++;
        myAnimator.Play($"hand{nameStep}Destroy",0 ,0.0f);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
