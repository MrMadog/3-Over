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
        Random generator = new Random();

        List<Texture2D> cardTextures;
        List<string> allCards;
        List<Rectangle> suitCardRects;
        List<Rectangle> rankCardRects;
        List<Rectangle> flipCardRects;
        List<int> randomCards;

        SpriteFont testFont, testFont2;

        Texture2D cardSpritesheet;

        Vector2 mousePos;

        bool done = false;
        bool rightCard = false;
        bool flipBool = false;
        bool done1 = false;

        int suitIndex, rankIndex, rank, points = 0, cardIndex, suitScreenIndex, rankScreenIndex;

        string userCard, suit;

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

            userCard = "";

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
            testFont2 = Content.Load<SpriteFont>("TestFont2");



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

            mousePos = new Vector2(mouseState.X, mouseState.Y);

            if (screen == Screen.suitScreen)
            {
                for (int i = 0; i < 4; i++)
                    if (suitCardRects[i].Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        suitIndex = i;
                        done = true;
                    }

                switch (suitIndex)
                {
                    case 0: suit = "clubs"; break;
                    case 1: suit = "diamonds"; break;
                    case 2: suit = "hearts"; break;
                    case 3: suit = "spades"; break;
                }

                if (done)
                    if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                    {
                        done = false;
                        screen = Screen.rankScreen;
                    }
            }

            if (screen == Screen.rankScreen)
            {
                for (int i = 0; i < 13; i++)
                    if (rankCardRects[i].Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        rankIndex = i;
                        rank = rankIndex + 1;
                        userCard = $"{suit} {rank}";
                        done = true;
                    }


                if (done)
                    if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            cardIndex = generator.Next(0, 52);
                            if (randomCards.Contains(cardIndex))
                            {
                                do
                                {
                                    cardIndex = generator.Next(0, 52);
                                } while (randomCards.Contains(cardIndex));
                            }
                            randomCards.Add(cardIndex);
                        }
                        done = false;
                        screen = Screen.flipScreen;
                    }
            }

            if (screen == Screen.flipScreen)
            {
                if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter) && done == false)
                {
                    flipBool = true;
                    for (int i = 0; i < 3; i++)
                    {
                        if (allCards[randomCards[i]].Contains(userCard))
                        {
                            points += 10;
                            rightCard = true;
                        }

                        else if (allCards[randomCards[i]].Contains(rank.ToString()) && rightCard == false)
                        {
                            points += 3;
                        }

                        else if (allCards[randomCards[i]].Contains(suit) && rightCard == false)
                        {
                            points += 1;
                        }
                    }
                    done = true;
                }

                if (done)
                    if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                    {
                        done = false;
                        screen = Screen.pointScreen;
                    }
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
                _spriteBatch.DrawString(testFont2, "Choose a Suit", new Vector2(100, 50), Color.White);

                suitScreenIndex = 0;
                foreach (Rectangle card in suitCardRects)
                {
                    _spriteBatch.Draw(cardTextures[suitScreenIndex], card, Color.White);
                    suitScreenIndex += 13;
                }   

                _spriteBatch.DrawString(testFont, suitIndex.ToString(), new Vector2(200, 400), Color.White);
            }

            if (screen == Screen.rankScreen)
            {
                rankScreenIndex = suitIndex * 13;
                foreach (Rectangle card in rankCardRects)
                {
                    _spriteBatch.Draw(cardTextures[rankScreenIndex], card, Color.White);
                    rankScreenIndex += 1;
                }

                _spriteBatch.DrawString(testFont, rankIndex.ToString(), new Vector2(200, 50), Color.White);
                _spriteBatch.DrawString(testFont, userCard, new Vector2(300, 100), Color.White);
            }

            if (screen == Screen.flipScreen)
            {
                for (int i = 0; i < 3; i++) 
                    _spriteBatch.DrawString(testFont, randomCards[i].ToString(), new Vector2(i * 50 + 50, 50), Color.White);

                if (flipBool == false)
                    for (int i = 200; i < 500; i += 100)
                        _spriteBatch.Draw(cardTextures[54], new Rectangle(i, 200, 96, 150), Color.White);

                if (flipBool)
                {
                    _spriteBatch.Draw(cardTextures[randomCards[0]], new Rectangle(200, 200, 96, 150), Color.White);
                    _spriteBatch.Draw(cardTextures[randomCards[1]], new Rectangle(300, 200, 96, 150), Color.White);
                    _spriteBatch.Draw(cardTextures[randomCards[2]], new Rectangle(400, 200, 96, 150), Color.White);
                }

                _spriteBatch.DrawString(testFont, userCard, new Vector2(300, 100), Color.White);

            }

            if (screen == Screen.pointScreen)
            {
                _spriteBatch.DrawString(testFont, $"Points: {points}", new Vector2(100, 50), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}