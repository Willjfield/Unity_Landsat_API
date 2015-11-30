using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class PlayMovieTextureOnUI : MonoBehaviour 
{
	RawImage rawimage;
	WebCamTexture webcamTexture;

	void Start () 
	{
		webcamTexture = new WebCamTexture();
		//rawimage.texture = webcamTexture;
		//rawimage.material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}
	void Update()
	{
		GetComponent<RawImage>().texture = webcamTexture;
	}
}