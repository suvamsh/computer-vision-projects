
using UnityEngine;
using System.Collections;
using System;
public class StreetView : MonoBehaviour
{
	public bool loadOnStart = true;
	public double heading = 0;
	public double lat = 30.284003;
	public double lon = -97.7446368;

	void Start() {
		if(loadOnStart) Refresh();	
	}
	
	public void Refresh() {
		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh ()
	{
		return updateStreetView (lat, lon, heading);
	}

	IEnumerator updateStreetView(double lat, double lon, double heading)
	{
		string key = "AIzaSyAw7oPyfqhT_SaWR2CxN-Hj1CmiA5JyAAk";
		// "https://maps.googleapis.com/maps/api/streetview?size=600x300&location=46.414382,10.013988&heading=151.78";
		var url = "https://maps.googleapis.com/maps/api/streetview";
		var qs = "?";
		qs += WWW.UnEscapeURL (string.Format ("size={0}x{1}", 600, 300));
		qs += "&";
		qs += WWW.UnEscapeURL(string.Format ("location={0},{1}", lat, lon));
		qs += "&";
		qs += "heading=" + heading;
		qs += "&";
		qs += "key=" + key;
		
		Debug.Log (url + qs);
		var headers = new Hashtable();
		headers.Add("Content-Type", "text/x-cross-domain-policy");
		var req = new WWW ( url + qs, null, headers);
		while (!req.isDone)
			yield return null;
		Debug.Log ("Started downloading Image");
		if (req.error == null) {
			Debug.Log ("Finished downloading streetview image successfully");
			
			var tex = new Texture2D (600, 300);
			req.LoadImageIntoTexture (tex);
			GameObject flatGlobe = GameObject.Find ("FlatGlobe");
			flatGlobe.renderer.material.mainTexture = req.texture;

		} else {
			Debug.Log ("Error" + req.error);
		}
	}

	static double deg2rad(double deg)
	{
		return (Math.PI / 180) * deg;
	}
	static double rad2deg(double rad)
	{
		return (180 / Math.PI) * rad;
	}
	static double[] coordinatesFromBearingAndHeading(double lat1, double lon1, double heading, double distance)
	{
		double R = 6378.0;
		double lat2 = Math.Asin( Math.Sin(lat1)*Math.Cos(distance/R) +
		                        Math.Cos(lat1)*Math.Sin(distance/R)*Math.Cos(heading) );
		double lon2 = lon1 + Math.Atan2(Math.Sin(heading)*Math.Sin(distance/R)*Math.Cos(lat1),
		                                Math.Cos(distance/R)-Math.Sin(lat1)*Math.Sin(lat2));
		return new double[]{lat2, lon2};
	}
	public void moveForward(double distance)
	{
		double[] latLon = coordinatesFromBearingAndHeading (deg2rad(lat), deg2rad(lon), heading, distance);
		lat = rad2deg(latLon [0]);
		lon = rad2deg(latLon [1]);
		Debug.Log (String.Format ("{0}, {1}", lat, lon));
		Refresh ();
	}

	public void moveBackward(double distance)
	{
		moveForward (-distance);
	}

	public void turn(double dHeading)
	{
		heading += dHeading;
		heading = heading < 0 ? 360 - heading : heading;
		heading = heading > 360 ? heading % 360 : heading;
		Refresh ();
	}
}
