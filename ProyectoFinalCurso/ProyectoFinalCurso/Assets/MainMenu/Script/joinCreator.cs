using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joinCreator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> allBones;

    [ContextMenu("CreateJoints")]
    private void CreateJoints()
    {
        allBones = new List<GameObject>();

        GetBones(transform);

        Rigidbody previousRB = null;

        for(int i = 0; i < allBones.Count; i++)
        {
            allBones[i].AddComponent<CharacterJoint>();

            if(previousRB != null)
            {
                allBones[i].GetComponent<CharacterJoint>().connectedBody = previousRB;

            }
            previousRB = allBones[i].GetComponent<Rigidbody>();
        }
    }
    
    private void GetBones(Transform bone)
    {
        allBones.Add(bone.gameObject);

        foreach(Transform child in bone)
        {
            GetBones(child);
        }
    }
}
