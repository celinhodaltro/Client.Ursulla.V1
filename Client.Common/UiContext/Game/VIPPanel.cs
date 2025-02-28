﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Common;
public class VIPPanel : UIVirtualFrame
{
    ClientViewport Viewport;

    /// <summary>
    /// Need a reference to be able to unsubscribe to state change events.
    /// </summary>
    protected GameDesktop Desktop;

    public VIPPanel(GameDesktop Desktop)
    {
        Name = "VIP List";

        Bounds.Width = 176;
        Bounds.Height = 200;

        this.Desktop = Desktop;
        Desktop.ActiveViewportChanged += ViewportChanged;
        ViewportChanged(Desktop.ActiveViewport);
    }

    protected void ViewportChanged(ClientViewport NewViewport)
    {
        if (Viewport != null)
            Viewport.VIPStatusChanged -= VIPStatusChanged;

        // If there is no new state just clear the view
        Viewport = NewViewport;

        if (Viewport != null)
            Viewport.VIPStatusChanged += VIPStatusChanged;

        UpdateList();

    }

    private void VIPStatusChanged(ClientViewport Viewport, ClientCreature Creature)
    {
        if (Viewport != this.Viewport)
            return;

        UpdateList();
    }

    private void UpdateList()
    {
        ContentView.RemoveAllSubviews();

        if (Viewport == null)
            return;

        List<ClientCreature> VIPs = new List<ClientCreature>(Viewport.VIPList.Values);
        VIPs.Sort(delegate(ClientCreature A, ClientCreature B)
        {
            if (A.Online && !B.Online)
                return -1;
            else if (B.Online && !A.Online)
                return 1;
            return A.Name.CompareTo(B.Name);
        });

        foreach (ClientCreature VIP in VIPs)
        {
            UILabel Label = new UILabel(VIP.Name);
            Label.Bounds.Width = ClientBounds.Width;
            Label.TextColor = VIP.Online ? Color.LightGreen : Color.Red;
            ContentView.AddSubview(Label);
        }
    }

}
