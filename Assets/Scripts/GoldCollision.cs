using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCollision : MonoBehaviour {

    [HideInInspector]
    public GameMode gameMode;
	void Start ()
    {
        gameMode = GameObject.FindObjectOfType<GameMode>();	
	}
	
	
	void Update () {
		
	}

    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            gameMode.goldNumber++;
            Destroy(gameObject);
        }
    }
}
