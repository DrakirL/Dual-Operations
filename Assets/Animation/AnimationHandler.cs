using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;


[System.Serializable]
public struct AnimationFunction
{
    public AnimationClip animation;
    public UnityEvent animEvent;
}
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] AnimationFunction[] allAnimations;
    private AnimationFunction currentAnimation;
    [SerializeField] Animator anim;
    string comp = "new";

    public void changeAnimation(string newAnimation)
    {
        if (anim != null)
        {
            if (comp != newAnimation || comp == "new")
            {
                comp = newAnimation;
                currentAnimation = findAnim(newAnimation);
                currentAnimation.animEvent.Invoke();
                anim.Play(currentAnimation.animation.name, 0, 0);
            }
        }
    }
    AnimationFunction findAnim(string nameOfAnim)
    {
        AnimationFunction tempFunc = new AnimationFunction();
        for (int i = 0; i < allAnimations.Length; i++)
        {
            if (nameOfAnim == allAnimations[i].animation.name)
            {
                return allAnimations[i];
            }
        }
        Debug.LogError("no animation found with the name " + nameOfAnim);
        return tempFunc;
    }

    //update is only debug tools, delete when done
    private void Update()
    {
        if (false)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                changeAnimation("WALK");
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                changeAnimation("GUN DRAW");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                changeAnimation("STUNNED");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                changeAnimation("IDLE");
            }
        }
    }
}