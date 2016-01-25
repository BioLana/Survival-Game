using UnityEngine;
using System.Collections;

public class FootStepManager : MonoBehaviour {
	public GroundType groundType;
	float StepSpeed = 0.3f;
	public float WalkSpeed;
	public float RunSpeed;
	public float CrouchSpeed;
	public Animator anim;
	float StepTime;
	public Variables Grass = new Variables();
	public Variables Rock = new Variables();
	public Variables Sand = new Variables();
	public Variables Snow = new Variables();
	public Variables Metal = new Variables();
	public Variables Wood = new Variables();
	Texture tex;
	public enum GroundType
	{
		Grass,
		Rock,
		Metal,
		Snow,
		Sand,
		Wood,
		Nothing
	}
	void Update(){
		RaycastHit hit;
		if (transform.GetComponent<CharacterControlls> ().grounded == true) {
			if (Physics.Raycast (transform.position, -Vector3.up , out hit)) {
				if (hit.transform.gameObject.GetComponent<MeshRenderer> ()) {
					tex = hit.transform.gameObject.GetComponent<MeshRenderer> ().material.mainTexture;
				}
				if(hit.transform.tag == "Terrain")
				{
					tex = getTerrainTextureAt(transform.position);
				}
				if (tex != null){
				if (tex.name.Contains("Grass")) {
					groundType = GroundType.Grass;
				}
				else if (tex.name.Contains("Rock")) {
					groundType = GroundType.Rock;
				}
				else if (tex.name.Contains("Metal")) {
					groundType = GroundType.Metal;
				}
				else if (tex.name.Contains("Snow")) {
					groundType = GroundType.Snow;
				}
				else if (tex.name.Contains("Sand")) {
					groundType = GroundType.Sand;
				}
				else if (tex.name.Contains("Wood")) {
					groundType = GroundType.Wood;
				}

				}
			}
		}
		if (tex == null) {
			groundType = GroundType.Nothing;
		}
		if (StepTime > 0){
			StepTime -= Time.deltaTime;
		}
		if (StepTime <= 0)
		{
			if (anim.GetFloat("Speed") > 0.1f && transform.GetComponent<CharacterControlls> ().grounded == true) 
			{
				FootStep();
				StepTime = StepSpeed;
			}
		}
		if (GetComponent<CharacterControlls> ().running == true) {
			StepSpeed = RunSpeed;
		} else {
			StepSpeed = WalkSpeed;
		}
		if (GetComponent<CharacterControlls> ().crouching == true) {
			StepSpeed = CrouchSpeed;
		}
	}
	void FootStep()
	{
		if (groundType ==GroundType.Grass) {
			GetComponent<AudioSource>().PlayOneShot(Grass.sounds[Random.Range(0,Grass.sounds.Length)]);
		}
		if (groundType ==GroundType.Rock) {
			GetComponent<AudioSource>().PlayOneShot(Rock.sounds[Random.Range(0,Rock.sounds.Length)]);
		}
		if (groundType ==GroundType.Metal) {
			GetComponent<AudioSource>().PlayOneShot(Metal.sounds[Random.Range(0,Metal.sounds.Length)]);
		}
		if (groundType ==GroundType.Snow) {
			GetComponent<AudioSource>().PlayOneShot(Snow.sounds[Random.Range(0,Snow.sounds.Length)]);
		}
		if (groundType ==GroundType.Sand) {
			GetComponent<AudioSource>().PlayOneShot(Sand.sounds[Random.Range(0,Sand.sounds.Length)]);
		}
		if (groundType ==GroundType.Wood) {
			GetComponent<AudioSource>().PlayOneShot(Wood.sounds[Random.Range(0,Wood.sounds.Length)]);
		}
	}
	public Texture getTerrainTextureAt( Vector3 position )
	{
		Texture retval    =    new Texture();
		Vector3 TS; // terrain size
		Vector2 AS; // control texture size
		
		TS = Terrain.activeTerrain.terrainData.size;
		AS.x = Terrain.activeTerrain.terrainData.alphamapWidth;
		AS.y = Terrain.activeTerrain.terrainData.alphamapHeight;

		// Lookup texture we are standing on:
		int AX = (int)( ( position.x/TS.x )*AS.x + 0.5f );
		int AY = (int)( ( position.z/TS.z )*AS.y + 0.5f );
		float[,,] TerrCntrl = Terrain.activeTerrain.terrainData.GetAlphamaps(AX, AY,1 ,1);
		
		TerrainData TD = Terrain.activeTerrain.terrainData;
		
		for( int i = 0; i < TD.splatPrototypes.Length; i++ )
		{
			if( TerrCntrl[0,0,i] > .5f )
			{
				retval    =    TD.splatPrototypes[i].texture;
			}
			
		}
		return retval;
	}
}
[System.Serializable]
public class Variables{
	public AudioClip[] sounds;
}