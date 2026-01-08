using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MarbleManager : MonoBehaviour {

	public List<Transform> Marbles;
    public float globalForceMultiplier = 10.0f;

	// Use this for initialization
	void Start () {
        // Find all marbles if list is empty
        if (Marbles == null || Marbles.Count == 0) {
            Marbles = new List<Transform>();
            var marbles = FindObjectsOfType<MarbleGravity>();
            foreach (var m in marbles) {
                Marbles.Add(m.transform);
            }
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (AdvancedInputManager.Instance != null) {
            Vector3 movement = AdvancedInputManager.Instance.GetCompositeMovement();

            if (movement.sqrMagnitude > 0.001f) {
                Vector3 force = movement * globalForceMultiplier;

                foreach (var marble in Marbles) {
                    if (marble != null) {
                        Rigidbody rb = marble.GetComponent<Rigidbody>();
                        if (rb != null) {
                            rb.AddForce(force);
                        }
                    }
                }
            }
        }
	}
}
