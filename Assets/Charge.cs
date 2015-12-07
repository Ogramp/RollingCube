using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour {

    public float rotateSpeed;	
	
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
	}
}
