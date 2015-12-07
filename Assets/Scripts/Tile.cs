using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public int posX;
    public int posY;

    public bool passable = true;

	// Use this for initialization
	void Start () {        
        if (!passable)
            GetComponent<Renderer>().material.color = Color.red;
    }

}
