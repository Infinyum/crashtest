using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// add the namespace into each files of filters and use them here
//using CollisionVisual;

public class ScriptChanger : MonoBehaviour
{

    //private CollisionVisual.collisionVisual ascript;
    /*public DeformationCollision.deformationCollision bscript;
    public PhantomBalistic.phantomBalistic cscript;*/

    public void SetCollisionVisual()
    {
        //ascript = GetComponent<CollisionVisual.collisionVisual>();
        /*bscript = GetComponent<DeformationCollision.deformationCollision>();
        cscript = GetComponent<PhantomBalistic.phantomBalistic>();*/
        //ascript.enabled = true;
        /*bscript.enabled = false;
        cscript.enabled = false;*/
    }

    public void SetDeformationCollision()
    {
        //ascript = GetComponent<CollisionVisual.collisionVisual>();
        /*bscript = GetComponent<DeformationCollision.deformationCollision>();
        cscript = GetComponent<PhantomBalistic.phantomBalistic>();*/
        //ascript.enabled = false;
        /*bscript.enabled = true;
        cscript.enabled = false;*/
    }

    public void SetPhantomBalistic()
    {
        //ascript = GetComponent<CollisionVisual.collisionVisual>();
        /*bscript = GetComponent<DeformationCollision.deformationCollision>();
        cscript = GetComponent<PhantomBalistic.phantomBalistic>();*/
        //ascript.enabled = false;
        /*bscript.enabled = false;
        cscript.enabled = true;*/
    }

    public void SetNone()
    {
        //ascript = GetComponent<CollisionVisual.collisionVisual>();
        /*bscript = GetComponent<DeformationCollision.deformationCollision>();
        cscript = GetComponent<PhantomBalistic.phantomBalistic>();*/
        //ascript.enabled = false;
        /*bscript.enabled = false;
        cscript.enabled = false;*/
    }


}
