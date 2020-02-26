using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We need a mesh collider to provide mesh deformation
[RequireComponent(typeof(MeshCollider))]
/**
 * deformationCollision script:
 * Script that deforms locally a mesh after a collision
 */
public class deformationCollision : MonoBehaviour{

	//local area aroud the collision point were deformation occurs
	public float minDist = 1.0f;

	public bool enableScript = true;	//is the script enabled

	//small clamp function
	private static T Clamp<T>(T aValue, T aMin, T aMax) where T : IComparable<T>{

		var _Result = aValue;
		if (aValue.CompareTo(aMax) > 0)
			_Result = aMax;
		else if (aValue.CompareTo(aMin) < 0)
			_Result = aMin;
		return _Result;
	}


	// Start is called before the first frame update
	void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }


    void OnCollisionEnter(Collision collisionInfo){

        // Get instantiated mesh
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        MeshCollider mc = GetComponent<MeshCollider>();

        // get all the vertices
        Vector3[] vertices = mesh.vertices;

		//for each contact point
        foreach (var c in collisionInfo.contacts){

            var impact = mc.ClosestPoint(c.point);

			//we look for the closest vertices
			int p = 0;
			while (p < vertices.Length){

				//We get (into world coordinates) the current vertice
				Vector3 v = transform.TransformPoint(vertices[p]);

				//We compute the distance from the impact point to the vertex
				float dist = (impact - v).sqrMagnitude;


				//Collision deforms a zone around the collision (not only one point)
				//We check if we are in the "deformation" zone
				if (dist <= minDist){

					//We clamp the distance (we don't want division by 0 or very small distances)
					dist = Clamp<float>(dist, 0.01f, minDist);

					Vector3 offset = -c.normal;	//We compute the vertex offset
					offset.Scale(collisionInfo.relativeVelocity/1000.0f);	//we must scale the offset
					vertices[p] += offset /dist*100;
				}
				p++;
			}

			

		}
         
		//we refresh the vertices and the normals
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }



}

