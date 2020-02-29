using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class to wrap the LineRenderer Object (makes it easier to use)
 * 
 */
public struct LineDrawer
{
    public LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer(float lineSize = 0.2f)
    {
        GameObject lineObj = new GameObject("LineObj");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        //lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    private void init(float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color startColor, Color endColor, float startWidth, float endWidth)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        //Set color
        lineRenderer.material.color = startColor;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        //Set width
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}

/**
 * Struct to group what we need to draw a line : a contact point and an intensity
 */ 
public struct collisionForce
{
    public ContactPoint contactPoint;
    public Vector3 intensity;
}

/**
 * Script to visually draw lines on collision points 
 */
public class collisionVisual : MonoBehaviour{

	
    private int i = 0;					// internal counter of lines
	private List<LineDrawer> lines;     //list of available lines
	public List<collisionForce> impacts;//List of impact points (where we draw lines)

	//////////////////// Parameters of the script ////////////////////

	public float lineRadiusCoeff = 0.01f;       //"radius" of the line (how big is the line)
	public int maxNumberLine = 2500;            //How much lines can we have simultaneously ?
	public double interlineMinDistance = 0.2;   //How far MUST be two lines to exist ?
	public bool enableScript = true;			//is the script enabled ?


	// Start is called before the first frame update
	void Start(){

		//We initialize our lists
        impacts = new List<collisionForce>();
        lines = new List<LineDrawer>();

		//We fill our lineDrawer list (=> ready to draw lines)
        for (int j = 0; j < maxNumberLine; j++){

            var lineDrawer = new LineDrawer(lineRadiusCoeff);
            lineDrawer.lineRenderer.startWidth = lineRadiusCoeff;
            lineDrawer.lineRenderer.endWidth = lineRadiusCoeff;
            lines.Add(lineDrawer);
        }

    }

    // Update is called once per frame
    void Update(){

		//We do something only if the script is enabled
		if (enableScript){

			ContactPoint previousContact = new ContactPoint();
			bool first = true;
			foreach (collisionForce impact in impacts){

				if(impact.intensity.magnitude <= 0.1)
				{
					return;
				}

				//We compute the actual size of the line
				float sizeRay = lineRadiusCoeff / 2.0f * impact.intensity.magnitude * 1.5f;

				if (!first){

					Vector3 p = previousContact.point;
					Vector3 p2 = impact.contactPoint.point;
					double dist2 = Mathf.Sqrt((p2.x - p.x) * (p2.x - p.x) + (p2.y - p.y) * (p2.y - p.y) + (p2.z - p.z) * (p2.z - p.z));

					//We don't want to draw overlapping rays
					if (dist2 > interlineMinDistance)
					{

						Vector3 endPoint = new Vector3(-impact.contactPoint.normal.x * impact.intensity.x, -impact.contactPoint.normal.y * impact.intensity.y, -impact.contactPoint.normal.z * impact.intensity.z);

						//We setup the draw (color mostly)
						var v = impact.contactPoint.point - endPoint;
						v = v.normalized;
						Color vColorStart = new Color(v.x / v.magnitude, v.y / v.magnitude, v.z / v.magnitude);
						Color vColorEnd = vColorStart;//new Color(endPoint.x / endPoint.magnitude, endPoint.y / endPoint.magnitude, endPoint.z / endPoint.magnitude);

						//We take one of the lineDrawer and use it to draw our line
						lines[i % maxNumberLine].DrawLineInGameView(impact.contactPoint.point, impact.contactPoint.point - endPoint, vColorStart, vColorEnd, sizeRay, sizeRay);
						i = (i % maxNumberLine) + 1;	//increment counter
						previousContact = impact.contactPoint;
					}
				}
				// For the first contact point, we don't check the previous line as there is no previous line...
				else
				{

					Vector3 endPoint = new Vector3(-impact.contactPoint.normal.x * impact.intensity.x, -impact.contactPoint.normal.y * impact.intensity.y, -impact.contactPoint.normal.z * impact.intensity.z);

					var v = impact.contactPoint.point;
					Color vColorStart = new Color(v.x / v.magnitude, v.y / v.magnitude, v.z / v.magnitude);
					Color vColorEnd = new Color(endPoint.x / endPoint.magnitude, endPoint.y / endPoint.magnitude, endPoint.z / endPoint.magnitude);

					lines[i % maxNumberLine].DrawLineInGameView(impact.contactPoint.point, impact.contactPoint.point - endPoint, vColorStart, vColorEnd, sizeRay, sizeRay);
					i = (i % maxNumberLine) + 1;
					previousContact = impact.contactPoint;
					first = false;
				}



				//Debug.DrawRay(contact.point, -contact.normal, Color.red);       
			}
		}
    }

	void addRay(Collision collisionInfo)
	{
		/* One point collision ray */

		//We compute the collision only if the script is enabled
		//if (enableScript){
		//	ContactPoint contact = collisionInfo.contacts[0];
		//	collisionForce c = new collisionForce();
		//	c.contactPoint = contact;
		//	c.intensity = collisionInfo.relativeVelocity;
		//	impacts.Add(c);
		//}



		//Multiple ray for same contact
		if (enableScript)
		{
			foreach (ContactPoint contact in collisionInfo.contacts)
			{
				collisionForce c = new collisionForce();
				c.contactPoint = contact;
				c.intensity = collisionInfo.relativeVelocity;
				impacts.Add(c);

			}
		}

	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		addRay(collisionInfo);
	}

	void OnCollisionStay(Collision collisionInfo)
	{
		addRay(collisionInfo);
	}	
}
