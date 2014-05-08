using UnityEngine;
using System.Collections;

public class changeColour : MonoBehaviour {
	private int num;
	void start(){
		renderer.material.color = Color.green;
		num = 1;
	}

	void OnMouseUp(){
		
			if(num == 1) {
				renderer.material.color = Color.red;
				num = 0;
			}
			else {
				renderer.material.color = Color.green;
				num = 1;
			}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
