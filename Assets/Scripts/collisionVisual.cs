using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        //Set color
        lineRenderer.material.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

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

public struct collisionForce
{
    public ContactPoint contactPoint;
    public Vector3 intensity;
}

public class collisionVisual : MonoBehaviour
{

    public List<collisionForce> impacts;

    private int i = 0;
    public float lineRadius = 0.025f;

    private List<LineDrawer> l;
    private List<LineDrawer> permanentL;
    public int maxNumberLine = 2500;
    public double interlineMinDistance = 0.2;


    // Start is called before the first frame update
    void Start()
    {
        impacts = new List<collisionForce>();
        l = new List<LineDrawer>();

        for (int j = 0; j < maxNumberLine; j++)
        {
            var lineDrawer = new LineDrawer(lineRadius);
            lineDrawer.lineRenderer.SetWidth(lineRadius, lineRadius);
            l.Add(lineDrawer);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ContactPoint previousContact = new ContactPoint();
        bool first = true;
        foreach (collisionForce impact in impacts)
        {

            if (!first)
            {

                Vector3 p = previousContact.point;
                Vector3 p2 = impact.contactPoint.point;
                double dist2 = Mathf.Sqrt((p2.x - p.x) * (p2.x - p.x) + (p2.y - p.y) * (p2.y - p.y) + (p2.z - p.z) * (p2.z - p.z));

                //We don't want to draw overlapping rays
                if (dist2 > interlineMinDistance)
                {

                    Vector3 endPoint = new Vector3(-impact.contactPoint.normal.x * impact.intensity.x, -impact.contactPoint.normal.y * impact.intensity.y, -impact.contactPoint.normal.z * impact.intensity.z);
                    l[i % maxNumberLine].DrawLineInGameView(impact.contactPoint.point, impact.contactPoint.point - endPoint, Color.red);
                    i = (i % maxNumberLine) + 1;
                    previousContact = impact.contactPoint;
                }
                
            }
            else
            {

                Vector3 endPoint = new Vector3(-impact.contactPoint.normal.x * impact.intensity.x, -impact.contactPoint.normal.y * impact.intensity.y, -impact.contactPoint.normal.z * impact.intensity.z);
                l[i % maxNumberLine].DrawLineInGameView(impact.contactPoint.point, impact.contactPoint.point - endPoint, Color.red);


                i = (i % maxNumberLine) + 1;
                previousContact = impact.contactPoint;
                first = false;
            }

            

            //Debug.DrawRay(contact.point, -contact.normal, Color.red);       
        }

    }

    void OnCollisionStay(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            collisionForce c = new collisionForce();
            c.contactPoint = contact;
            c.intensity = collisionInfo.relativeVelocity;
            impacts.Add(c);

        }
    }
}
