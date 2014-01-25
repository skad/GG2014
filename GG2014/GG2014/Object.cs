﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GG2014
{
    class Object
    {
        Vector2 mPos;
        double mSize;
        Texture2D mTexture;
        Rectangle mSource;

        public Object(float x, float y)
        {
            mSize = 20;
            mPos.X = x;
            mPos.Y = y;
            mSource = new Rectangle(0, 0, 32, 32);
        }

        public void genBaseTexture(GraphicsDevice graphic)
        {
            //definition d'une texture toute pourie
            mTexture = new Texture2D(graphic, 1, 1);
            mTexture.SetData(new Color[] { Color.White });
        }

        public Texture2D getBaseTexture()
        {
            return mTexture;
        }
        public Vector2 getPos()
        {
            return mPos;
        }

        public void setPosition(int x, int y)
        {
            this.mPos.X = x - 16;
            this.mPos.Y = y - 16;
        }

        public double getSize()
        {
            return this.mSize;
        }

        public void setSize(double size)
        {
           this.mSize = size;
        }

        public void Draw(SpriteBatch sb,Texture2D texture = null)
        {
            if (texture == null)
            {
                texture = mTexture;
            }
            Rectangle destination = new Rectangle((int)this.getPos().X, (int)this.getPos().Y, (int)this.mSize, (int)this.mSize);
            sb.Draw(texture, destination, mSource, Color.White);
        }

    }
}
