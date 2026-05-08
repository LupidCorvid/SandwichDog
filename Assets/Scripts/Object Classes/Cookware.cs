using UnityEngine;

public class Cookware : ObjClass
{
    [SerializeField] protected CookwareCollider collider;
    public CookwareCollider Collider => collider;

    public Cookware(ObjType inObjType, string name) : base(ObjType.PICKUP, name)
    {
        
    }
}
