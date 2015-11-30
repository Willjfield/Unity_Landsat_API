using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class WebCameraScript : MonoBehaviour {
	public GUITexture myCameraTexture;
	private WebCamTexture webCameraTexture;
	void Start () {
		// Checks how many and which cameras are available on the device
		for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
		{
			// We want the back camera
			if (!WebCamTexture.devices[cameraIndex].isFrontFacing)
			{
				webCameraTexture = new WebCamTexture(cameraIndex, Screen.width, Screen.height);
				// Here we flip the GuiTexture by applying a localScale transformation
				// works only in Landscape mode
				myCameraTexture.transform.localScale = new Vector3(-1,-1,1);
			}
		}    
		// Here we tell that the texture of coming from the camera should be applied
		// to our GUITexture. As we have flipped it before the camera preview will have the
		// correct orientation
		myCameraTexture.texture = webCameraTexture;
		// Starts the camera
		webCameraTexture.Play();
	}
	public void ShowCamera()
	{
		myCameraTexture.GetComponent<GUITexture>().enabled = true;
		webCameraTexture.Play();
	}
	public void HideCamera()
	{
		myCameraTexture.GetComponent<GUITexture>().enabled = false;
		webCameraTexture.Stop();
	}
	void OnGUI()
	{
		// A button to demonstrate how to turn the camera on and off, in case you need it
		if(GUI.Button(new Rect(0,0,100,100),"ON/OFF"))
		{
			if(webCameraTexture.isPlaying)
				this.HideCamera();
			else
				this.ShowCamera();
		}
	}
}