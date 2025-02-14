using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class PlayerIterator<T>
{
    public PlayerIterator() { }

    public abstract bool hasNext();

    public abstract T getNext();

    public abstract void reset();
}
