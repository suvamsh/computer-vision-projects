using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ArrowKeyMovement : MonoBehaviour {
	public bool gmap = false;
	public bool sview = false;
	private GoogleMap googleMap;
	private StreetView streetView;
	public float speed = 0.2f;
	// Use this for initialization
	void Start () {
		//RequireComponent(GoogleMap);
		streetView = GetComponent<StreetView> ();
		googleMap = GetComponent<GoogleMap>();
		Color newColor = this.renderer.material.color;
		newColor.a = 255;
		this.renderer.material.color = newColor;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.anyKey) {
			if (gmap) {
				screenshot ();
			}
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 position = this.transform.position;
			Vector3 rotation = this.transform.rotation.eulerAngles;
			rotation.z += 20*speed;
			position.x -= speed;
			this.transform.position = position;
			this.transform.rotation = Quaternion.Euler(rotation);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 position = this.transform.position;
			Vector3 rotation = this.transform.rotation.eulerAngles;
			rotation.z -= 20*speed;
			position.x += speed;
			this.transform.position = position;
			this.transform.rotation = Quaternion.Euler(rotation);

		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 position = this.transform.position;
			Vector3 rotation = this.transform.rotation.eulerAngles;
			rotation.x += 20*speed;
			position.z += speed;
			this.transform.position = position;
			this.transform.rotation = Quaternion.Euler(rotation);

		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 position = this.transform.position;
			Vector3 rotation = this.transform.rotation.eulerAngles;
			rotation.x -= 20*speed;
			position.z -= speed;
			this.transform.position = position;
			this.transform.rotation = Quaternion.Euler(rotation);

		}
		if (Input.GetKey(KeyCode.U))
		{
			Vector3 position = this.transform.position;
			position.y += speed;
			this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.D))
		{
			Vector3 position = this.transform.position;
			position.y -= speed;
			this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.Z)) 
		{
			// update texture using googlemaps
			Zoom(0.1f);
			if (googleMap.zoom > 10) {
				sview = true;
				gmap = false;
				moveForward();
			}
		}
		if (Input.GetKey(KeyCode.X)) 
		{
			// update texture using googlemaps
			googleMap.zoom--;
			if (googleMap.zoom <= 10) {
				sview = false;
				gmap = true;
				googleMap.Refresh();
				Vector3 increment = new Vector3(1, 1, 1);
				this.transform.localScale -= increment;
			}
			Debug.Log("Decreased Zoom");
		}
		if (Input.GetKey(KeyCode.S))
		{
			Debug.Log("Saved screenshot");
			// Application.CaptureScreenshot("Screenshot.png");
			screenshot ();
		}
		if (Input.GetKey (KeyCode.F)) {
			moveForward();
		}
		if (Input.GetKey (KeyCode.B)) {
			moveBackward();
		}
		if (Input.GetKey (KeyCode.L)) {
			turnLeft();
		}
		if (Input.GetKey (KeyCode.R)) {
			turnRight();
		}
	}
	IEnumerator _screenshot(int width, int height)
	{
		yield return new WaitForEndOfFrame();
		Texture2D sshot = new Texture2D(width/2, height);
		sshot.ReadPixels(new Rect(0, 0, width/2, height), 0, 0);
		sshot.Apply();
		GameObject flatGlobe = GameObject.Find ("FlatGlobe");
		flatGlobe.renderer.material.mainTexture = sshot;
		Color c = flatGlobe.renderer.material.color;
		c.a = 1.0f;
		flatGlobe.renderer.material.color = c;
		byte[] pngShot = sshot.EncodeToPNG();
		//Destroy(sshot);
		//File.WriteAllBytes(Application.dataPath + "/../screenshot_" + Random.Range(0, 1024).ToString() + ".png", pngShot);
		
	}
	public void screenshot()
	{
		StartCoroutine (_screenshot(Screen.width, Screen.height));
	}


	public double alpha = 1;
	double prev = 0.0;

	public void Zoom(float scale)
	{
			double smooth = (scale * alpha) + ((1 - alpha) * prev);
			prev = smooth;
			Debug.Log (smooth);
			int z = (int)Math.Floor (smooth * 10.0);

			googleMap.zoom = z;

			//googleMap.Refresh();
			if (z > 5) {
				gmap = false;
				sview = true;
				moveForward();
			} else {
				gmap = true;
				sview = false;
				screenshot ();
			}

			Vector3 increment = new Vector3 (100 * scale, 100 * scale, 100 * scale);
			this.transform.localScale = increment;
		//Debug.Log("Increased Zoom");

	}

	public void moveForward()
	{
		Debug.Log ("Moving forward");
		Debug.Log (streetView);
		streetView.moveForward (0.01);
		streetView.Refresh ();
	}

	public void moveBackward()
	{
		Debug.Log ("Moving forward");
		Debug.Log (streetView);
		streetView.moveBackward (0.01);
		streetView.Refresh ();
	}

	public void turnLeft()
	{
		streetView.turn (15);
	}
	public void turnRight()
	{
		streetView.turn (-15);
	}
}
