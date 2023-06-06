using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;

namespace _3_Over
{
    public class Game1 : Game
    {
        List<Texture2D> cardTextures;
        List<string> allCards;
        List<Rectangle> suitCardRects;
        List<Rectangle> rankCardRects;
        List<Rectangle> flipCardRects;
        List<int> randomCards;

        SpriteFont testFont;
        Texture2D cardSpritesheet;

        enum Screen
        {
            suitScreen, rankScreen, flipScreen, pointScreen
        }

        Screen screen;

        KeyboardState keyboardState, prevKeyboardState;
        MouseState mouseState, prevMouseState;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            cardTextures = new List<Texture2D>();
            allCards = new List<string>();
            suitCardRects = new List<Rectangle>();
            rankCardRects = new List<Rectangle>();
            flipCardRects = new List<Rectangle>();
            randomCards = new List<int>();

            screen = Screen.suitScreen;

            base.Initialize();

            var cards = File.ReadAllLines("all_cards.txt");

            foreach (var s in cards)
                allCards.Add(s);

            //suit screen card rectangles vvv
            for (int i = 267; i < 813; i+= 150)
                suitCardRects.Add(new Rectangle(i, 200, 96, 150));

            //rank screen card rectangles vvv
            for (int i = 267; i < 813; i += 150)
                rankCardRects.Add(new Rectangle(i, 200, 96, 150)); // row 1 (4)
            for (int i = 192; i < 888; i += 150)
                rankCardRects.Add(new Rectangle(i, 300, 96, 150)); // row 2 (5)
            for (int i = 267; i < 813; i += 150)
                rankCardRects.Add(new Rectangle(i, 400, 96, 150)); // row 3 (4)

            //flip screen card rectangles vvv


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cardSpritesheet = Content.Load<Texture2D>("card_deck");
            testFont = Content.Load<SpriteFont>("testFont");



            Texture2D cropTexture;
            Rectangle sourceRect;

            int width = cardSpritesheet.Width / 13;
            int height = cardSpritesheet.Height / 5;

            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 13; x++)
                {
                    sourceRect = new Rectangle(x * width, y * height, width, height);
                    cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    cardSpritesheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);

                    if (cardTextures.Count < 55)
                        cardTextures.Add(cropTexture);
                }

        }

        protected override void Update(GameTime gameTime)
        {
            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();



            if (screen == Screen.suitScreen)
            {

            }

            if (screen == Screen.rankScreen)
            {

            }

            if (screen == Screen.flipScreen)
            {

            }

            if (screen == Screen.pointScreen)
            {

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (screen == Screen.suitScreen)
            {
                foreach (Rectangle card in suitCardRects)
                    _spriteBatch.Draw(cardTextures[0], card, Color.White);
            }

            if (screen == Screen.rankScreen)
            {

            }

            if (screen == Screen.flipScreen)
            {

            }

            if (screen == Screen.pointScreen)
            {

            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}