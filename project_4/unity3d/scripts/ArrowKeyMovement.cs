using UnityEngine;
using System.Collections;
using System.IO;

public class ArrowKeyMovement : MonoBehaviour {
	public GoogleMap googleMap;
	public float speed = 0.2f;
	// Use this for initialization
	void Start () {
		//RequireComponent(GoogleMap);
		googleMap = GetComponent<GoogleMap>();
		Color newColor = this.renderer.material.color;
		newColor.a = 255;
		this.renderer.material.color = newColor;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
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
			googleMap.zoom++;
			googleMap.Refresh();
			Vector3 increment = new Vector3(1, 1, 1);
			this.transform.localScale += increment;
			Debug.Log("Increased Zoom");
		}
		if (Input.GetKey(KeyCode.X)) 
		{
			// update texture using googlemaps
			googleMap.zoom--;
			googleMap.Refresh();
			Vector3 increment = new Vector3(1, 1, 1);
			this.transform.localScale -= increment;
			Debug.Log("Decreased Zoom");
		}
		if (Input.GetKey(KeyCode.S))
		{
			Debug.Log("Saved screenshot");
			// Application.CaptureScreenshot("Screenshot.png");
			StartCoroutine(screenshot (Screen.width, Screen.height));
		}
	}
	IEnumerator screenshot(int width, int height)
	{
		yield return new WaitForEndOfFrame();
		Texture2D sshot = new Texture2D(width/2, height);
		sshot.ReadPixels(new Rect(0, 0, width/2, height), 0, 0);
		sshot.Apply();
		GameObject flatGlobe = GameObject.Find ("FlatGlobe");
		flatGlobe.renderer.material.mainTexture = sshot;
		byte[] pngShot = sshot.EncodeToPNG();
		Destroy(sshot);
		File.WriteAllBytes(Application.dataPath + "/../screenshot_" + Random.Range(0, 1024).ToString() + ".png", pngShot);
		
	}
}
