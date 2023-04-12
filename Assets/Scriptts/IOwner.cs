using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOwner
{
    public PlayerData GetData();
    public void AddBase(Base baseArg);
    public void RemoveBase(Base baseArg);
}
