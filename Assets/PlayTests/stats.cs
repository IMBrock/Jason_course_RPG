﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using UnityEngine;

public class stats : MonoBehaviour
{
    [Test]
    public void can_add()
    {
        Stats stats = new Stats();
        stats.Add(StatType.MoveSpeed, 3f);
        Assert.AreEqual(8f, stats.Get(StatType.MoveSpeed));
        stats.Add(StatType.MoveSpeed, 5f);
        Assert.AreEqual(13f, stats.Get(StatType.MoveSpeed));
    }
    
    [Test]
    public void can_remove()
    {
        Stats stats = new Stats();
        stats.Add(StatType.MoveSpeed, 3f);
        Assert.AreEqual(8f, stats.Get(StatType.MoveSpeed));
        
        stats.Remove(StatType.MoveSpeed, 3f);
        Assert.AreEqual(5f, stats.Get(StatType.MoveSpeed));
    }
}