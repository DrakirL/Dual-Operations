using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGuardMaterial : MonoBehaviour
{
    public Material m_AngryMaterial;
    public GameObject m_Guard;

    private MeshRenderer[] children;

    private void ChangeMaterial(Material m_Material)
    {
        children = GetComponentInChildren<MeshRenderer[]>();
        foreach (MeshRenderer rend in children)
        {
           var mats = new Material[rend.materials.Length];
            for (int i = 0; i < rend.materials.Length; i++)
            {
                mats[i] = m_Material;
            }
            rend.materials = mats;
            m_Guard.GetComponentInChildren<MeshRenderer>().material = m_AngryMaterial;
        }
        
       
    }

    private void Update()
    {
        ChangeMaterial(m_AngryMaterial);
    }

}
