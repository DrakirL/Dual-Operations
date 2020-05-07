using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackerMission : MonoBehaviour
{
    [Tooltip("all gameobject displaying tutorials for the hacker")]
    [SerializeField] GameObject[] missionStuff;
    [Tooltip("set a object containing unitys text component and it will display the current side you are on, leave this empty if this is not wanted")]
    [SerializeField] Text pageof;
    [Tooltip("with this bool if you goes over the limit you start at the otherside, otherwise the hacker function wont do anyhing if it will lead to out of bounds")]
    [SerializeField] bool allowLoop;
    bool havePageOfText;

    int currentMission = 0;
     int AllMission;
    // Start is called before the first frame update
    void OnEnable()
    {
        havePageOfText = pageof != null;   

        AllMission = missionStuff.Length-1;
        for(int i = 0; i < missionStuff.Length;  i++)
        {
            missionStuff[i].SetActive(false);
        }
        makeTheShift();
    }

    public void goLeftFunc()
    {
        missionStuff[currentMission].SetActive(false);
        currentMission--;
        if (currentMission < 0)
        {
            if (allowLoop)
            {
                currentMission = AllMission;
            }
            else
            {
                currentMission = 0;
            }
        }
        makeTheShift();
    }
   public void goRightFunc()
    {
        missionStuff[currentMission].SetActive(false);
        currentMission++;

        if (currentMission >= missionStuff.Length)
        {
            if (allowLoop)
            {
                currentMission = currentMission % missionStuff.Length;
            }
            else
            {
                currentMission = AllMission;
            }
        }
        makeTheShift();
    }
    void makeTheShift()
    {
        missionStuff[currentMission].SetActive(true);
        if (havePageOfText)
        {
            pageof.text = (currentMission + 1) + "/" + missionStuff.Length;
        }
    }
}
