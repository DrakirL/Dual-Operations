using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct AnimationFunction
{
    public string nameOfAnim;
    public AnimationClip animation;
    public UnityEvent animEvent;
}
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] AnimationFunction[] allAnimations;
    [SerializeField] private AnimationFunction currentAnimation;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Animation anim;
    // [SerializeField] Animation animation;

    private void Start()
    {
        for(int i = 0; i < allAnimations.Length; i++)
        {
            Debug.Log("ok");
            anim.AddClip(allAnimations[i].animation, allAnimations[i].nameOfAnim);
        }
        //anim.clip = anim.GetClip(allAnimations[1]);
        anim.Play();
        changeAnimation("Walk");
    }
    public void changeAnimation(string newAnimation)
    {
        currentAnimation = findAnimation(newAnimation);
        
        //animation.clip = currentAnimation.animation;
        //animation.Play();
    }
    private void Update()
    {
        /*if (animation.isPlaying)
        {
            return;
        }*/
        #region old stuff
        /* //animation.Play();
         if (Input.GetKeyDown(KeyCode.Q))
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
        #endregion
    }

    private AnimationFunction findAnimation(string nameOfAnim)
    {
        AnimationFunction anim = new AnimationFunction();
        for (int i = 0; i < allAnimations.Length; i++)
        {
            if (allAnimations[i].nameOfAnim == nameOfAnim)
            {
                return allAnimations[i];
            }
        }
        Debug.LogError("no animation found with the name " + nameOfAnim);
        return anim;
    }
}
