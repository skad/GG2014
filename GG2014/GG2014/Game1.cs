using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GG2014
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int idNoteCorde;
        Corde[] cordes;
        List<Enemis> ListObject;
        bool touche_down;
        bool jump_touche_down;
        bool fall_touche_down;
        Texture2D tex_ennemy_leaf;
        Texture2D tex_background;

        Vent vent;
        Note note;
        double EnemiTime;
        double TouchTime;
        double JumpTime;
        double FallTime;

        GenerateurObjet mRandomProvider;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1380;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ListObject = new List<Enemis>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            cordes = new Corde[4];
            int w = GraphicsDevice.Viewport.Bounds.Width;
            int h = GraphicsDevice.Viewport.Bounds.Height;

            cordes[0] = new Corde((int)(w / 3), h / 3, w / 8, h - h / 16);
            cordes[1] = new Corde((int)(w / 2.1), h / 3, w / 3, h - h / 16);
            cordes[2] = new Corde((int)(w - (w / 2.1)), h / 3, w - (w / 3), h - h / 16);
            cordes[3] = new Corde((int)(w - (w / 3)), h / 3, w - (w / 8), h - h / 16);
            
            for (int i = 0; i <= 3; i++)
            {
                cordes[i].genBaseTexture(GraphicsDevice);
            }
 
            EnemiTime = 0;
            TouchTime = 0;
            JumpTime = 0;
            mRandomProvider = new GenerateurObjet();
            Texture2D tex1, tex2, tex3;
            tex1 = Content.Load<Texture2D>("noire-32");
            tex2 = Content.Load<Texture2D>("double-croche-32");
            tex3 = Content.Load<Texture2D>("triple-croche-32");
            tex_ennemy_leaf = Content.Load<Texture2D>("leaf-32");
            tex_background = Content.Load<Texture2D>("background");
            note = new Note(0, 0, tex1, tex2, tex3, 3);
            idNoteCorde = 1;
            touche_down = false;
            jump_touche_down = false;
            fall_touche_down = false;
            vent = new Vent(0, 0, tex1);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            EnemiTime += time;
            TouchTime += time;
            JumpTime += time;
            FallTime += time;

            if (EnemiTime > 2.0f)
            {
                EnemiTime -= 2.0f;
                int cordId = mRandomProvider.getCorde();
                Enemis temp = new Enemis(cordes[cordId].getStart().X, cordes[cordId].getStart().Y, cordes[cordId].getVectorDir());
                temp.setSize(32);
                temp.setTexture(tex_ennemy_leaf);
                ListObject.Add(temp);
            }

            if (TouchTime > 0.05f)
            {
                TouchTime -= 0.05f;
                touche_down = false;
            }

            if (JumpTime > 0.5f)
            {
                JumpTime -= 0.5f;
                jump_touche_down = false;
            }

            if (FallTime > 0.5f)
            {
                FallTime -= 0.5f;
                fall_touche_down = false;
            }

            for (int i = 0; i < ListObject.Count - 1; i++)
            {
                Enemis temp2 = ListObject[i];
                if (temp2.getPos().Y > graphics.PreferredBackBufferWidth)
                {
                    ListObject.Remove(temp2);
                }
                else
                {
                    ListObject[i].setPosition((temp2.getPos().X + (temp2.getDir().X / 1)), (temp2.getPos().Y + (temp2.getDir().Y / 1)));
                    ListObject[i].setSize(temp2.getSize());
                }

                if (temp2.getPos().X >= note.getPos().X - 20 && temp2.getPos().X <= note.getPos().X + 20 && temp2.getPos().Y > note.getPos().Y)
                {
                    ListObject.Remove(temp2);
                    checkForDeath();
                }
            }

            // Keyboard functions
            KeyboardState kState = Keyboard.GetState();

            // GTFO
            if (kState.IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                System.Environment.Exit(0);
            }

            if (kState.IsKeyDown(Keys.Left) && !touche_down)
            {
                note.decreaseAngle();
                touche_down = true;
            }

            if (kState.IsKeyDown(Keys.Right) && !touche_down)
            {
                note.increaseAngle();
                touche_down = true;
            }

            // Keyboard functions
            Keys[] currentPressedKeys = kState.GetPressedKeys();
            // CHEAT
            if (kState.IsKeyDown(Keys.RightControl) && kState.IsKeyDown(Keys.OemOpenBrackets))
            {
                note.cheetah();
            }

            double angle = note.getAngle();
            if (kState.IsKeyDown(Keys.Space) && !jump_touche_down)
            {
                
                if (angle <= MathHelper.PiOver4)
                {
                    idNoteCorde--;
                }
                else if (angle >= 3 * MathHelper.PiOver4)
                {
                    idNoteCorde++;
                }
                jump_touche_down = true;
            }

            // Check angle 
            if (note.getAngle() > MathHelper.Pi && !fall_touche_down)
            {
                if (checkForDeath())
                {
                    // Readjust angle
                    note.resetAngle();
                }
                fall_touche_down = true;
            }

            if (idNoteCorde < 0 || idNoteCorde > 3)
            {
                System.Console.WriteLine("GAME OVER (" + idNoteCorde + ")");
                System.Environment.Exit(0);
            }

            note.setPosition(cordes[idNoteCorde].getEnd().X, cordes[idNoteCorde].getEnd().Y);

            base.Update(gameTime);
        }

        private bool checkForDeath()
        {
            if (note.getLivesLeft() > 1)
            {
                note.kill();
                return true;
            }
            else
            {
                // Game over
                System.Console.WriteLine("You got screwed");
            }
            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // Background
            Rectangle backgroundRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height);
            spriteBatch.Draw(tex_background, backgroundRectangle, Color.White);

            for (int i = 0; i <= 3; i++)
            {
                cordes[i].Draw(spriteBatch);
            }

            for (int i = 0; i < ListObject.Count - 1; i++)
            {
                ListObject[i].Draw(spriteBatch);
            }

            note.Draw(spriteBatch);
            // vent.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
