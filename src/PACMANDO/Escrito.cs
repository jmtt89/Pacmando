using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace WindowsGame3
{

    public class Escrito
    {

        SpriteFont Fuente;
        Vector2 posTexto;
        string Mensaje;

        public Escrito(ContentManager Content,string Dir)
        {
            Fuente = Content.Load<SpriteFont>(Dir);
            
        }


        public void Load(string M,Vector2 Pos)
        {
            posTexto = Pos;
            Mensaje = M;
        }


        public void Draw(SpriteBatch batch)
        {
            batch.DrawString(Fuente, Mensaje,posTexto,Color.White);

        }

    }
}