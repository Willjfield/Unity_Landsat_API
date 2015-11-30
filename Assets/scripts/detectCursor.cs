using UnityEngine;
using System.Collections;

public class detectCursor : MonoBehaviour {

	void OnTriggerStay(Collider other)
	{
		if (other.transform.localScale.y < 1) {
			other.transform.localScale += new Vector3 (0.0F, 0.05F, 0);
		}
	}
	void OnTriggerExit(Collider other)
	{
	//	if (other.transform.localScale.y > 0) {
			other.transform.localScale = new Vector3 (1.0F, 0.01F, 1.0F);
		//}
	}
	/*
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.localScale.y > 0) {
			other.transform.localScale = new Vector3 (0, 0.01F, 0);
		}
	}
	*/
}
