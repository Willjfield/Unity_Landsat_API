// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class loadImage : MonoBehaviour {
	//declare strings for api calls
	private string m_InGameLog = "";
	private string apiCall;
	//declare long and lat to be inserted into call
	public float longitude;
	public float latitude;
	//declare the final month of the year you want to load until and the current month displayed
	private int finalMonth;
	public int indexCurMonth;
	//declare the final day of the month you want to load until and the current day displayed
	private int finalDay;
	public int indexCurDay;
	//declare the start and end years you want to look at and the current year displayed
	private int startYear;
	private int endYear;
	public int curYear;
	//which image should be displayed? Just the index in the array of textures
	public int imgIndex;
	//Is the day in the beginning(0) or end(1) of the month?
	private int lookupDay;
	//Is the data loaded?
	private bool loaded;
	//Array list of map textures
	private List<Texture2D> mapText = new List<Texture2D>();

	//Get the object that will keep track of the index
	private GameObject indexObject;

	Renderer renderer;
	
	IEnumerator Start() {

		//Init above variables

		//Get the image index from the global variable in the IndexNum layer
		indexObject=GameObject.Find("IndexObject");
		imgIndex = indexObject.GetComponent<IndexNum>().indexVal;
		//imgIndex = 0; //Or use an int if you want to control individual tiles

		string fmt = "00";

		loaded = false;

		curYear = 2015;
		startYear = 2015;
		endYear = 2015;

		finalMonth = 12;
		indexCurMonth = 0;

		finalDay = 31;
		indexCurDay = 0;
		lookupDay = 0;

		//Long and lat for Havana:
		//longitude = -82.37f;
		//latitude = 23.14f;

		renderer = GetComponent<Renderer>();
		//For each year, for each month, and for each day...
		for (int y = startYear; y <= endYear; y++) {
			for (int m=1; m<=finalMonth; m++) {
				for (int d=1; d<finalDay; d+=16) {
					//format the iterator to fit into the api call
					string monthToLoad = m.ToString (fmt);
					string dayToLoad = d.ToString (fmt);
					//set the api call
					apiCall = "https://ad-landsat-api.herokuapp.com/landsat?date_from="+y+"-01-01&date_to="+y+"-" + monthToLoad + "-" + dayToLoad + "&contains="+longitude+","+latitude;
					Debug.Log ("api url: " + apiCall);
					//make any old texture
					GetComponent<Renderer> ().material.mainTexture = new Texture2D (4, 4, TextureFormat.ARGB32, false);
					//make a new api call with the data from the apicall url
					WWW api = new WWW (apiCall);
					yield return api;
					//Parse the JSON returned from the api call
					string JSONText = api.text;
					var N = JSON.Parse (JSONText);
					string imageurl = N ["results"] [0] ["browseURL"];
					//Create a string with the url to the image
					yield return imageurl;
					//If we found a url for an image in the JSON
					if (imageurl != null) {
						Debug.Log ("image url: " + imageurl);
						//Make a new www request for that image
						WWW www = new WWW (imageurl);
						yield return www;
						//make a texture from the image
						Texture2D tempTex = www.texture;
						//If it's dark, make it transparent
						Color[] colors = tempTex.GetPixels (0,0,tempTex.width,tempTex.height);
						yield return colors;
						for(int c=0;c<colors.Length;c++){
							Color tempC = colors[c];
							if((tempC.r + tempC.g + tempC.b) < .1f){
								colors[c].a = 0.0f;
							}
						}
						Texture2D Tex = new Texture2D (tempTex.width,tempTex.height,TextureFormat.ARGB32, false);
						Tex.SetPixels(colors);
						Tex.Apply();
						//Add this texture to the array of textures after it's keyed
						mapText.Add (Tex);
					}
				}
			}
		}
			//set the initial texture to the first one we got (earliest chronologically)
			renderer.material.mainTexture = mapText[0];
			yield return renderer.material.mainTexture;
		//Things are ready to go
		loaded = true;
	}

	void Update(){
		imgIndex = indexObject.GetComponent<IndexNum>().indexVal;
		//Format the date. Needs to take into account variable month lengths.
		if(indexCurMonth>finalMonth){indexCurMonth=finalMonth-1;}
		if(indexCurMonth<0){indexCurMonth=0;}
		if(indexCurDay<0){indexCurDay=0;}
		if(indexCurDay>finalDay){indexCurDay=finalDay-1;}
		//See if the desired date is at the beginning or end of the month
		if (indexCurDay < 16) {
			lookupDay = 0;
		} else {
			lookupDay = 1;
		}
		//...or we can just scrub through all the images in the array. It's much easier.
		if (imgIndex > mapText.Count-1) {imgIndex = mapText.Count-1;}
		if (imgIndex < 0) {imgIndex = 0;}
		//If data is loaded, load the correct texture
		if (loaded) {
			GetComponent<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
			//Use this if you want to input a specific date (needs some kinks worked out to really work)
			//renderer.material.mainTexture = mapText[((curYear-startYear)*12)+(indexCurMonth*2)+lookupDay];
			//Or just scrub
			renderer.material.mainTexture = mapText[imgIndex];
		}
	}

}
