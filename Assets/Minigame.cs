using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public Piece[,] pieces;
    private void OnEnable()
    {
        pieces = new Piece[6, 6];

        for (int i = 0; i < 36; i++)
        {
            pieces[(int)transform.GetChild(i).transform.localPosition.x, (int)transform.GetChild(i).transform.localPosition.y] = transform.GetChild(i).GetComponent<Piece>();
            /*for (int j = 0; j < 6; j++)
            {
                Debug.Log(transform.GetChild(counter));
                pieces[i, j] = transform.GetChild(counter++).GetComponent<Piece>();
                Debug.Log(pieces[i, j].transform.localPosition);
            }*/
        }
    }
}
