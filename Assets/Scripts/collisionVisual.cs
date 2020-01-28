using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LineDrawer
{
    private LineRenderer lineRenderer;
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
            //Particles/Additive
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


public class collisionVisual : MonoBehaviour
{

    public List<ContactPoint> impacts;
    //public LineDrawer line;
    //public LineDrawer line2;

    private int i = 0;
    public float lineSize = 0.1f;

    private List<LineDrawer> l;
    private List<LineDrawer> permanentL;
    public int size = 100;
    public double distanceLimit;


    // Start is called before the first frame update
    void Start()
    {
        impacts = new List<ContactPoint>();
        l = new List<LineDrawer>();

        for (int j = 0; j < size; j++)
        {
            l.Add(new LineDrawer(lineSize));
        }

    }

    // Update is called once per frame
    void Update()
    {
        ContactPoint previousContact = new ContactPoint();
        bool first = true;
        foreach (ContactPoint contact in impacts)
        {

            if (!first)
            {

                Vector3 p = previousContact.point;
                Vector3 p2 = contact.point;
                double dist2 = Mathf.Sqrt((p2.x - p.x) * (p2.x - p.x) + (p2.y - p.y) * (p2.y - p.y) + (p2.z - p.z) * (p2.z - p.z));

                //We don't want to draw overlapping rays
                if (dist2 > distanceLimit)
                {
                    l[i % size].DrawLineInGameView(contact.point, contact.point - contact.normal, Color.red);
                    i = (i % size) + 1;
                    previousContact = contact;
                }
                
            }
            else
            {
                l[i % size].DrawLineInGameView(contact.point, contact.point - contact.normal, Color.red);
                i = (i % size) + 1;
                previousContact = contact;
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
            
            impacts.Add(contact);

        }
    }
}
