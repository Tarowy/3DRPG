using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranstionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        Enter,A,B,C //该传送门是入口还是其他种类的传送门
    }

    public DestinationTag destinationTag;
}
