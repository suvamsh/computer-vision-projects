using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13;
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;
	
	void Start() {
		if(loadOnStart) Refresh();	
	}
	
	public void Refresh() {
		if(autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");	
		}
		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh ()
	{
		// "http://api.tiles.mapbox.com/v4/socter.k63nfbkg/0,0,0/512x512.png?access_token=pk.eyJ1Ijoic29jdGVyIiwiYSI6ImtwSkVzamcifQ.3i5RjlCkOGMZtA2of5SF5g";
		var url = "http://api.tiles.mapbox.com/v4/socter.k63nfbkg/";
		var qs = "";
		if (!autoLocateCenter) {
			if (centerLocation.address != "")
				qs += "center=" + WWW.UnEscapeURL (centerLocation.address);
			else {
				qs += "" + WWW.UnEscapeURL (string.Format ("{0},{1}", centerLocation.latitude, centerLocation.longitude));
			}
		
			// qs += "&zoom=" + zoom.ToString ();
			qs += "," + zoom.ToString ();
		}
		qs += "/" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += ".png";
		// qs += "&scale=" + (doubleResolution ? "2" : "1");
		// qs += "&maptype=" + mapType.ToString ().ToLower ();
		qs += "?access_token=pk.eyJ1Ijoic29jdGVyIiwiYSI6ImtwSkVzamcifQ.3i5RjlCkOGMZtA2of5SF5g";
		var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
		// qs += "&sensor=" + (usingSensor ? "true" : "false");
		/*
		foreach (var i in markers) {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		
		foreach (var i in paths) {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL (loc.address);
				else
					qs += "|" + WWW.UnEscapeURL (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}*/

		Debug.Log (url + qs);
		var headers = new Hashtable();
		headers.Add("Content-Type", "text/x-cross-domain-policy");
		var req = new WWW ( url + qs, null, headers);
		while (!req.isDone)
			yield return null;
		Debug.Log ("Started downloading Image");
		if (req.error == null) {
			Debug.Log ("Finished downloading image successfully");

			var tex = new Texture2D (size, size);
			req.LoadImageIntoTexture (tex);
			renderer.material.mainTexture = req.texture;
		} else {
			Debug.Log ("Error" + req.error);
		}
	}
	
	
}

public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}