using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalArea : MonoBehaviour
{
    private Player subject;
    [SerializeField] private Rigidbody linkedObject;

    private void Start()
    {
        subject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        subject = other.gameObject.GetComponent<Player>();

        if (subject == null) return;
        subject.transform.position = linkedObject.transform.position;
        Debug.Log("Teleported player to level");
        subject.Rotate(180f);
    }

    private void OnTriggerExit(Collider other)
    {
        subject = null;
    }
    

    public Player GetPlayer()
    {
        if (subject != null) return subject;
        Debug.Log("No player in the area");
        return null;
    }
}
