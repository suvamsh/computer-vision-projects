
#pragma strict

var thisMesh : Mesh;
var uvs : Vector2[];
var speed = 0.25;
#if !UNITY_IPHONE && !UNITY_ANDROID && !UNITY_WP8 && !UNITY_BLACKBERRY && !UNITY_TIZEN

function Start () 
{
    thisMesh = GetComponent(MeshFilter).mesh;
    uvs = thisMesh.uv;
}

function Update()
{
	for (var i : int = 0; i < uvs.length; i++) 
	{
		uvs[i].x = (uvs[i].x + speed);
	}
	
	thisMesh.uv = uvs;
}

#endif
