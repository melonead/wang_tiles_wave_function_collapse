using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using wfc;


namespace wfc;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D tileSet;

    private Wfc  wfcAlg;

    
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        wfcAlg = new Wfc();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        tileSet = new Texture2D(_spriteBatch.GraphicsDevice, 2048, 128);
        tileSet = Content.Load<Texture2D>("tileset");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        Rectangle sourceRect = new Rectangle(128, 0, 128, 128);
        Rectangle destinationRect = new Rectangle(0, 0, 128, 128);

        _spriteBatch.Begin();
        _spriteBatch.Draw(tileSet, destinationRect, sourceRect, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
