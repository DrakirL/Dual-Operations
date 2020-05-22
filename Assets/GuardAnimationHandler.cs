using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public struct GuardAnimation
{
    public string nameOfAnim;
    public AnimationClip animation;

    //public GameObject bags;
    //public GameObject body;
    //public GameObject eyes;
    //public GameObject gun;
}*/

public class GuardAnimationHandler : MonoBehaviour
{
   /* [SerializeField] GuardAnimation[] allAnimations;
    [SerializeField] private GuardAnimation currentAnimation;
    [SerializeField] Animation animation;
    private void Start()
    {
        changeAnimation("Walk");
    }
    public void changeAnimation(string newAnimation)
    {
        currentAnimation = findAnimation(newAnimation);
        animation.clip = currentAnimation.animation;
        animation.Play();
    }*/
    private void Update()
    {
       /* if (animation.isPlaying)
        {
            return;
        }
        //animation.Play();
        if(Input.GetKeyDown(KeyCode.Q))
        {
            changeAnimation("Walk");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            changeAnimation("Idle");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            changeAnimation("Draw");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            changeAnimation("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            changeAnimation("Stunned");
        }*/
    }

    /*private GuardAnimation findAnimation(string nameOfAnim)
    {
        GuardAnimation anim = new GuardAnimation();
        for(int i = 0; i < allAnimations.Length; i++)
        {
            if (allAnimations[i].nameOfAnim == nameOfAnim)
            {
                return allAnimations[i];
            }
        }
        Debug.LogError("no animation found with the name " + nameOfAnim);
        return anim;
    }*/
}
