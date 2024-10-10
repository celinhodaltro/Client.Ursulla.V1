using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;
using Client.Common;
using System.Drawing;
using System.Reflection.Metadata;
using System.Reflection;


namespace Client.Common;

/// <summary>
/// This is the main type for your game
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{
    GraphicsDeviceManager Graphics;

    GameDesktop Desktop;
    MouseState LastMouseState;

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);

        Graphics.PreparingDeviceSettings += PrepareDevice;
        Graphics.PreferredBackBufferWidth = 1280;
        Graphics.PreferredBackBufferHeight = 800;
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Setup the window
        IsFixedTimeStep = false;
        Graphics.SynchronizeWithVerticalRetrace = false;
        IsMouseVisible = true;
        Window.AllowUserResizing = true;

        Graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        // Initialize the context
        UIContext.Initialize(Window, Graphics, Content);
        UIContext.Load();

        // Create the game frame
        Desktop = new GameDesktop();
        Desktop.Load();
        Desktop.CreatePanels();

        // Initial layout of all views
        Desktop.LayoutSubviews();
        Desktop.NeedsLayout = true;

        FileInfo file = new FileInfo("./Test.tmv");
        Stream virtualStream = null;

        FileStream fileStream = file.OpenRead();
        if (file.Extension == ".tmv")
            virtualStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress);
        else
            virtualStream = fileStream;

        TibiaMovieStream MovieStream = new TibiaMovieStream(virtualStream, file.Name);
        ClientState State = new ClientState(MovieStream);

        MovieStream.PlaybackSpeed = 50;
        State.ForwardTo(new TimeSpan(0, 30, 0));

        if (State.Viewport.Player == null)
        {
            State.Viewport.Login += delegate (ClientViewport Viewport)
            {
                Desktop.AddClient(State);
            };
        }
        else
        {
            Desktop.AddClient(State);
        }

        State.Update(new GameTime());
    }

    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    protected override void Update(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();

        if (LastMouseState != null)
        {
            if (LastMouseState.LeftButton != mouse.LeftButton)
                Desktop.MouseLeftClick(mouse);
        }

        if (LastMouseState.X != mouse.X || LastMouseState.Y != mouse.Y)
            Desktop.MouseMove(mouse);

        LastMouseState = mouse;

        Desktop.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        try
        {
            Desktop.Draw(null, Window.ClientBounds);
        }
        catch (Exception e)
        {
            throw;
        }

        base.Draw(gameTime);
    }

    protected void PrepareDevice(object sender, PreparingDeviceSettingsEventArgs e)
    {
        e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
    }
}
