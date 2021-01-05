﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeCard : Card
{
    public int t1Spice
    {
        get;
        private set;
    }
    public int t2Spice
    {
        get;
        private set;
    }
    public int t3Spice
    {
        get;
        private set;
    }
    public int t4Spice
    {
        get;
        private set;
    }

    public TradeCard(int t1Spice = 0, int t2Spice = 0, int t3Spice = 0, int t4Spice = 0, bool usable = true)
    {
        this.t1Spice = t1Spice;
        this.t2Spice = t2Spice;
        this.t3Spice = t3Spice;
        this.t4Spice = t4Spice;
        this.usable = usable;
    }

    public override bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1)
    {
        if (!usable && spiceInventory.t1SpiceCount > -t1Spice * multiplier
            && spiceInventory.t2SpiceCount > -t2Spice * multiplier
            && spiceInventory.t3SpiceCount > -t3Spice * multiplier
            && spiceInventory.t4SpiceCount > -t4Spice * multiplier) return false;

        usable = false;
        spiceInventory.ModifySpices(t1Spice * multiplier, t2Spice * multiplier, t3Spice * multiplier, t4Spice * multiplier);
        return true;

    }
}
