// Continuously get the latest webcam shot from outside "Friday's" in Times Square
	// and DXT compress them at runtime
	public var apiCall: String = "https://ad-landsat-api.herokuapp.com/landsat?date_from=2015-05-16&date_to=2015-9-16&cloud_to=20&contains=-82.37,23.15";
	var url = "http://earthexplorer.usgs.gov/browse/landsat_8/2015/016/044/LC80160442015316LGN00.jpg";

	function Start () {
		// Create a texture in DXT1 format
		GetComponent.<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		while(true) {
			var api: WWW = new WWW(apiCall);
			yield api;
			
			var url = api.text;
			// Start a download of the given URL
			var www = new WWW(url);

			// wait until the download is done
			yield www;

			// assign the downloaded image to the main texture of the object
			www.LoadImageIntoTexture(GetComponent.<Renderer>().material.mainTexture);
		}
	}