using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MarbleGravity : MonoBehaviour {

	public GameObject Planet;
	public float gravityConstant;
	public Vector3 gravityForce;
	public float distanceBetween;
	public Vector3 normalizedDirection;
	public float massTimesConstant;
	public Material selfMaterial;
	public Vector3 awayFromPlanet;
	public Vector3 marbleToPlanet;
	public Vector3 randomizedAwayForce;
	public Vector3 inverseNormx;
	public Vector3 inverseNormy;
	public Vector3 inverseNormz;
	public float shootForce;
	public float waitTime;
	public bool avoidX;
	public bool avoidY;
	public bool avoidZ;
	public bool touchingPlanet;
	public bool isSleeping;
	public Rigidbody selfBody;
	private float max;
	private int collCount;

	void Start () {
		collCount = 0;
		Planet = GameObject.Find ("planet");
        if (Planet == null) {
            Debug.LogError("Planet not found! Please ensure there is a GameObject named 'planet'.");
        }
		selfMaterial = gameObject.GetComponent<MeshRenderer>().material;
		selfBody = gameObject.GetComponent<Rigidbody> ();
		max = selfBody.sleepThreshold;
		selfBody.sleepThreshold = 0.0025f;
	}

	void OnCollisionEnter(Collision other)
	{
		isSleeping = false;
		if(other.transform.name == "planet")
		{
			collCount = 1;
			selfBody.drag = 1f;
			selfBody.angularDrag = 1f;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if(other.transform.name == "planet")
		{
			collCount = 0;
			selfBody.drag = 0.2f;
			selfBody.angularDrag = 0.1f;
		}
	}

	void addForceToMarble()
	{
		if (Planet != null)
		    gameObject.GetComponent<Rigidbody>().AddForce(gravityForce);
	}

	void FixedUpdate () {
        if (Planet == null) return;

		inverseNormx = new Vector3 (-1 / normalizedDirection.x, 0f, 0f);
		inverseNormy = new Vector3 (0f, -1 / normalizedDirection.y, 0f);
		inverseNormz = new Vector3 (0f, 0f, -1 / normalizedDirection.z);
		Vector3 inverseNormXYZ = new Vector3 (-1 / normalizedDirection.x, -1 / normalizedDirection.y, -1/normalizedDirection.z);

        marbleToPlanet = Planet.transform.position - gameObject.transform.position;
		distanceBetween = marbleToPlanet.magnitude;
		Vector3 calcDirection = marbleToPlanet / distanceBetween;
		normalizedDirection = calcDirection;

        // Safety check for distance
        if (distanceBetween < 0.1f) distanceBetween = 0.1f;

        // Apply input influence to gravity?
        // Maybe the user wants to "tilt" gravity.
        // But MarbleManager already applies force.
        // Let's keep gravity as strict planet attraction.

        if (Planet.GetComponent<Rigidbody>() != null) {
		    massTimesConstant = gravityConstant*Planet.GetComponent<Rigidbody>().mass*selfBody.mass;
		    gravityForce = normalizedDirection*(massTimesConstant)/(distanceBetween*distanceBetween);
        } else {
            // Fallback if planet has no RB
             gravityForce = normalizedDirection * 9.8f * selfBody.mass;
        }

		float average = (Mathf.Abs(selfBody.velocity.x) + Mathf.Abs(selfBody.velocity.y) + Mathf.Abs(selfBody.velocity.z)) / 3;

		if(collCount == 0)
		{
			if(isSleeping!= false) {
				isSleeping = false;
			}
		}

		if((average <= max) && collCount == 1){
			if(waitTime <= 0f){
				if (!isSleeping) {
					isSleeping = true;
					selfMaterial.color = Color.green;
					selfBody.Sleep ();
				}
			}
			else{
				waitTime -= Time.deltaTime;
			}
				
		} else if (!isSleeping){
			waitTime = 0.1f;
			if (selfMaterial.color != Color.red)
			{
				selfMaterial.color = Color.red;
			}
			addForceToMarble ();
		}
	}
}
