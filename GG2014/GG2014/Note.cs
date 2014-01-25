﻿using System;
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
    class Note : Object
    {
        private int _vie;
        private Texture2D[] textures;
        private Rectangle source;
        
        public Note(int x,int y,Texture2D tex1,Texture2D tex2,Texture2D tex3,int nbVie=3): base(x,y)
        {
            this._vie = nbVie;
            this.textures = new Texture2D[nbVie];
            textures[0]=tex1;
            textures[1]=tex2;
            textures[2]=tex3;
            this.setSize(32);
        }

        public int vie 
        {
            get{return _vie;}
            set { _vie = value; }
        }


        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb, textures[_vie - 1]);
        }
    }
}
