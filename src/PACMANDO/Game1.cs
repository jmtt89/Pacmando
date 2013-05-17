using System;
using System.IO;
using System.Text;

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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D Life;
        Texture2D[] Fruta = new Texture2D[9];

        //Dimenciones
        int Alto = 600;
        int Ancho = 800;

        //mapa
        Mapa Mundo;

        int Vidas;
        int Puntos;
        int Nivel;

        int j = 0;
        bool FrutaContada;

        // Menu
        bool Menu;
        Texture2D MenuN;
        Texture2D Selec;
        Vector2 PosS;

        public Game1()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Alto;
            graphics.PreferredBackBufferWidth = Ancho;

        }

        protected override void Initialize()
        {
            Menu = true;
            FrutaContada = false;
            Vidas = 3;
            Puntos = 0;
            Nivel = 1;
            Life = Content.Load<Texture2D>("Imagenes/pacmando1");

            MenuN = Content.Load<Texture2D>("Imagenes/MenuPacmando");
            Selec = Content.Load<Texture2D>("Imagenes/BalonJuve");
            PosS = new Vector2(740, 240);
            LoadContent();

        }
        
        protected override void LoadContent()
        {
        
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Mundo = new Mapa(this);
            Mundo.LoadWorld(Content, "Imagenes/Board", Nivel,Puntos);
            //base.Initialize();           
        
        }

        protected override void Update(GameTime gameTime)
        {
        
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Menu)
                UpdateMenu();
            else
                UpdateJuego(gameTime);

            base.Update(gameTime);
        }

        private void UpdateJuego(GameTime gameTime)
        {
            if (Mundo.ComeFruta() && !FrutaContada)
            {
                FrutaContada = true;
                int N = ((Nivel - 1) % 9);
                Fruta[j] = Content.Load<Texture2D>("Bonos/" + N);
                j++;

                if (j >= 9)
                {
                    j = j % 9;
                    Vidas++;
                }


            }


            if (Mundo.NivelTerminado())
            {
                Puntos = Mundo.PuntosAcum();
                Nivel++;
                LoadContent();
                FrutaContada = false;
            }
            else
            {
                if (Mundo.ItsAlive())
                {
                    Mundo.Update(gameTime);
                }
                else
                {
                    Vidas = Vidas - 1;

                    if (Vidas >= 0)
                        Mundo.ReEspam(Content);
                    else
                        this.Exit();

                }

            }
        }

        private void UpdateMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                PosS.Y = 350;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                PosS.Y = 240;

            if(PosS.Y > 350)
                PosS.Y = 350;

            if (PosS.Y < 240)
                PosS.Y = 240;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                if(PosS.Y == 240)
                    Menu = false;
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if(Menu)
                DrawMenu(spriteBatch);
            else
                DrawJuego(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawJuego(SpriteBatch spriteBatch)
        {
            
            Mundo.Draw(spriteBatch);

            for (int i = 0; i < (Vidas - 1); i++)
            {
                spriteBatch.Draw(Life, new Vector2(110 + i * 40, 550), Color.White);
            }

            for (int i = 0; i < j; i++)
            {
                spriteBatch.Draw(Fruta[i], new Vector2(120 + i * 30, 570), Color.White);
            }
        }

        private void DrawMenu(SpriteBatch batch)
        {

            batch.Draw(MenuN, new Vector2(0, 0), Color.White);
            batch.Draw(Selec, PosS, Color.White);

        }
    }
}
