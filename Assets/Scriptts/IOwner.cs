using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseOwner
{
    public PlayerData GetData();
    public void AddBase(Base baseArg);
    public void RemoveBase(Base baseArg);
}
