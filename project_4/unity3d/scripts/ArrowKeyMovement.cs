using UnityEngine;
using System.Collections;


public class ArrowKeyMovement : MonoBehaviour {
	public GoogleMap googleMap;
	public float speed = 0.2f;
	// Use this for initialization
	void Start () {
		//RequireComponent(GoogleMap);
		googleMap = GetComponent<GoogleMap>();
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
			Debug.Log("Increased Zoom");
		}
		if (Input.GetKey(KeyCode.X)) 
		{
			// update texture using googlemaps
			googleMap.zoom--;
			googleMap.Refresh();
			Debug.Log("Decreased Zoom");
		}
	}
}
