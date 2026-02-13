using UnityEngine;

// You must derive the base class form monobehavior to attach it to objects in the scene
public class ObjClass : MonoBehaviour
{
    public enum ObjType { GRABBABLE, PICKUP };
    public enum Spread { NOSPREAD, PEANUTBUTTER, JELLY}

    public bool inHand;
    public bool inMouth;
    public ObjType m_type;

    //Object manager or subclass should pass in the object type
    public ObjClass(ObjType t) 
    {
        inHand = false;
        m_type = t;
    }

    //Callable by the player
    //TODO: Is it more fun to be able to hotswap hand and mouth? Currently set up so that hotswap is a thing
    //TODO: Can these be called by the xr manager?
    public bool PutInHand() {
        if (!inHand && m_type == ObjType.GRABBABLE)
        {
            inHand = true;
            if (inMouth) inMouth = false;
            return true;
        }
        return false;
    }
    public void DropFromHand() { 
        if(inHand) inHand = false;  
    }
    public bool PutInMouth() { 
        if(!inMouth)
        {
            inMouth = true;
            if (inHand) inHand = false;
            return true;
        }
        return false;
    }
    public void DropFromMouth() { }
    
}
