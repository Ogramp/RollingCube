using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;

	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + new Vector3(0, 3.5f, -3.5f);
	}

}
