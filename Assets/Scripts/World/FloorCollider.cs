using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FloorCollider : TimerCollider<ObjClass>
{
    protected override void TickTimer(ObjClass obj, float timePassed)
    {
        obj.DirtyObject(timePassed);
    }

    protected override bool ShouldRemoveTimer(ObjClass obj)
    {
        return (obj.objCleanliness <= 0.0f);
    }

    protected override bool CanAddTimer(ObjClass obj)
    {
        return obj.CanGetDirty;
    }
}
