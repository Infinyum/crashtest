using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Phantom script :
 * This script is showing "phantoms" of previous position of an object. That way, we can find the whole balistic history of an object
 */
public class phantomBalistic : MonoBehaviour{

	//We need a frame counter (as we don't show phantoms on each frame)
    private int frameNumber;
	
	//////////////////// Parameters of the script ////////////////////

	public Material phantomMat;			//The material we use to represent "phantoms"
    public bool enableScript = true;    //is the script enabled ?
	public bool drawOnCollision = false;//do we draw a phantom when the collision occurs ?
	public int frameFrequency = 15;		//how often do we draw a phantom ? (every 15 frames currently => 0.25s)

	private void drawPhantom(){
		// Get instantiated mesh
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

		//copy the mesh
		Mesh mesh2 = Instantiate(mesh);

		//We create a new game object that we are going to parameterize correctly 
		GameObject obj = new GameObject();

		MeshRenderer mr = obj.AddComponent<MeshRenderer>();
		MeshFilter mf = obj.AddComponent<MeshFilter>();
		mf.mesh = mesh2;

		Material resMat = new Material(phantomMat);
		//resMat.color = new Color(rb.velocity.x, rb.velocity.y, rb.velocity.z, 30);

		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		obj.transform.localScale = transform.lossyScale;

		obj.GetComponent<MeshRenderer>().material = resMat;
		/*If somehow the new object is having collision detection enabled : 
		obj.GetComponent<BoxCollider>().enabled = false;
		obj.GetComponent<Rigidbody>().detectCollisions = false;
		*/
	}

	// Start is called before the first frame update
	void Start(){

		//init the frame counter
        frameNumber = 0;

    }

    // Update is called once per frame
    void Update(){

		//if the script is enabled => we compute things
        if (enableScript){
            if (frameNumber % frameFrequency == 0){

				drawPhantom();
                
            }

            frameNumber++;
        }
        
    }

	//TODO : Add an event to enable the script (and then put default value to false)

    void OnCollisionEnter(Collision collisionInfo){

		if (enableScript) { 
			//The script disable itself when a collision occurs
			enableScript = false;

			if (drawOnCollision){

				//we draw a last phantom
				drawPhantom();
			}
		}
	}
}
