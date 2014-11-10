using UnityEngine;

public class Webcam : MonoBehaviour
{
	
	public MeshRenderer[] UseWebcamTexture;
	private WebCamTexture webcamTexture;
	
	void Start()
	{
		webcamTexture = new WebCamTexture();
		foreach(MeshRenderer r in UseWebcamTexture)
		{
			r.material.mainTexture = webcamTexture;
		}
		renderer.material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}
	
	void OnGUI()
	{
		if (webcamTexture.isPlaying)
		{
			if (GUILayout.Button("Pause"))
			{
				webcamTexture.Pause();
			}
			if (GUILayout.Button("Stop"))
			{
				webcamTexture.Stop();
			}
		}
		else
		{
			if (GUILayout.Button("Play"))
			{
				webcamTexture.Play();
			}
		}
	}
}
