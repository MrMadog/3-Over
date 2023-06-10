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
        List<int> rightSuitIndex;
        List<int> rightRankIndex;
        List<int> rightCardIndex;

        SpriteFont testFont, testFont2, testFont3;

        Texture2D cardSpritesheet;

        Vector2 mousePos;

        bool done = false;
        bool rightCard = false;
        bool flipBool = false;
        bool rightSuit = false;
        bool rightRank = false;

        int suitIndex, rankIndex, points, cardIndex, suitScreenIndex, rankScreenIndex;

        float seconds, startTime;

        string userCard, suit, rank, payout;

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
            rightCardIndex = new List<int>();
            rightRankIndex = new List<int>();
            rightSuitIndex = new List<int>();

            screen = Screen.suitScreen;

            base.Initialize();

            var cards = File.ReadAllLines("all_cards.txt");

            foreach (var s in cards)
                allCards.Add(s);

            userCard = "";

            for (int i = 267; i < 813; i+= 150)
                suitCardRects.Add(new Rectangle(i, 300, 96, 150));

            for (int i = 267; i < 813; i += 150)
                rankCardRects.Add(new Rectangle(i, 200, 96, 150)); // row 1 (4)
            for (int i = 192; i < 888; i += 150)
                rankCardRects.Add(new Rectangle(i, 300, 96, 150)); // row 2 (5)
            for (int i = 267; i < 813; i += 150)
                rankCardRects.Add(new Rectangle(i, 400, 96, 150)); // row 3 (4)

            for (int i = 342; i < 738; i += 150)
                flipCardRects.Add(new Rectangle(i, 300, 96, 150));

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cardSpritesheet = Content.Load<Texture2D>("card_deck");
            testFont = Content.Load<SpriteFont>("testFont");
            testFont2 = Content.Load<SpriteFont>("TestFont2");
            testFont3 = Content.Load<SpriteFont>("testFont3");



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

            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;

            if (screen == Screen.suitScreen)
            {
                for (int i = 0; i < 4; i++)
                    if (suitCardRects[i].Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
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
                        rank = $" {rankIndex + 1}";
                        userCard = $"{suit}{rank}";
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
                            rightCardIndex.Add(i);
                            rightCard = true;
                        }
                        else if (allCards[randomCards[i]].Contains(rank.ToString()) && rightCard == false)
                        {
                            points += 3;
                            rightRankIndex.Add(i);
                            rightRank = true;
                        }
                        else if (allCards[randomCards[i]].Contains(suit) && rightCard == false)
                        {
                            points += 1;
                            rightSuitIndex.Add(i);
                            rightSuit = true;
                        }
                    }
                    done = true;
                }

                if (done)
                    if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                    {
                        startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                        done = false;
                        screen = Screen.pointScreen;
                    }
            }

            if (screen == Screen.pointScreen)
            {
                if (seconds >= 2)
                    done = true;
                else 
                    done = false;

                switch (points)
                {
                    case 1: payout = "0.5x"; break;
                    case 0: payout = "0x"; break;
                    case 2: case 3: payout = "1x"; break;
                    case 4: payout = "1.5x"; break;
                    case 10: case 11: payout = "2x"; break;
                    case 5: case 6: payout = "5x"; break;
                    case 12: case 13: payout = "10x"; break;
                    case 7: case 14: payout = "50x"; break;
                    case 9: case 16: payout = "500x"; break;
                }

                if (done)
                    if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        suit = "";
                        rank = "";
                        userCard = "";
                        done = false;
                        rightCard = false;
                        rightSuit = false;
                        rightRank = false;
                        flipBool = false;
                        randomCards.Clear();
                        rightRankIndex.Clear();
                        rightSuitIndex.Clear();
                        rightCardIndex.Clear();
                        points = 0;
                        screen = Screen.suitScreen;
                    }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

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
                
                if (done)
                {
                    _spriteBatch.DrawString(testFont, suit, new Vector2(100, 100), Color.White);
                    _spriteBatch.DrawString(testFont2, "Press SPACE to Continue", new Vector2(260, 600), Color.White);
                }
            }

            if (screen == Screen.rankScreen)
            {
                _spriteBatch.DrawString(testFont2, "Choose a Rank", new Vector2(100, 50), Color.White);

                rankScreenIndex = suitIndex * 13;
                foreach (Rectangle card in rankCardRects)
                {
                    _spriteBatch.Draw(cardTextures[rankScreenIndex], card, Color.White);
                    rankScreenIndex += 1;
                }

                _spriteBatch.DrawString(testFont, userCard, new Vector2(100, 100), Color.White);

                if (done)
                    _spriteBatch.DrawString(testFont2, "Press SPACE to Continue", new Vector2(260, 600), Color.White);
                else
                    _spriteBatch.DrawString(testFont, suit, new Vector2(100, 100), Color.White);
            }

            if (screen == Screen.flipScreen)
            {
                _spriteBatch.DrawString(testFont2, "Click ENTER to Flip Cards", new Vector2(100, 50), Color.White);

                if (flipBool == false)
                    for (int i = 0; i < 3; i++)
                        _spriteBatch.Draw(cardTextures[54], flipCardRects[i], Color.White);

                if (flipBool)
                    for (int i = 0; i < 3; i++)
                        _spriteBatch.Draw(cardTextures[randomCards[i]], flipCardRects[i], Color.White);

                _spriteBatch.DrawString(testFont, $"Your Card: {userCard}", new Vector2(100, 100), Color.White);

                if (flipBool)
                    _spriteBatch.DrawString(testFont2, "Press SPACE to Continue", new Vector2(260, 600), Color.White);

                if (rightSuit)
                    foreach (int rightCard in rightSuitIndex)
                        _spriteBatch.DrawString(testFont3, "+1 pts", new Vector2(flipCardRects[rightCard].X, 250), Color.Black);

                if (rightRank)
                    foreach (int rightCard in rightRankIndex)
                        _spriteBatch.DrawString(testFont3, "+3 pts", new Vector2(flipCardRects[rightCard].X, 250), Color.Black);

                if (rightCard)
                    foreach (int rightCard in rightCardIndex)
                        _spriteBatch.DrawString(testFont3, "+10 pts", new Vector2(flipCardRects[rightCard].X, 250), Color.Black);
            }

            if (screen == Screen.pointScreen)
            {
                if (points >= 2 || points == 0)
                    _spriteBatch.DrawString(testFont2, $"You Got: {points} Points!", new Vector2(325, 200), Color.White);
                else
                    _spriteBatch.DrawString(testFont2, $"You Got: {points} Point!", new Vector2(325, 200), Color.White);

                _spriteBatch.DrawString(testFont, $"You Get {payout} Your Bet.", new Vector2(375, 350), Color.White);

                if (done)
                    _spriteBatch.DrawString(testFont2, "Press ENTER to Continue", new Vector2(260, 600), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}