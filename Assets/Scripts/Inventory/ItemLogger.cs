﻿using System;
using UnityEngine;

public class ItemLogger : ItemComponent
{
    public override void Use()
    {
        Debug.Log("item was used");
    }
}