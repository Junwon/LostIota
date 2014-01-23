﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LostIota
{
    public class GameUnit
    {
        protected int health;
        protected int max_health;
        protected SpriteSheetAnimation moveAnimation;
        protected float moveSpeed;

        protected ContentManager content;
        protected FileManager fileManager;
        
        protected Texture2D image;
        protected Vector2 position;

        protected List<List<string>> attributes, contents;

        protected Texture2D health_img;
        protected Rectangle health_rect;

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual Texture2D Image
        {
            get { return image; }
        }

        public virtual int Health
        {
            get { return health; }
        }

        public virtual void LoadContent(ContentManager content, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            health_img = this.content.Load<Texture2D>(@"health");

            health_rect = new Rectangle();
            health_rect.Width = health_img.Width;
            health_rect.Height = health_img.Height;
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input)
        {

        }
        
        public virtual void Update(GameTime gameTime, InputManager input, Collision col)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void DrawHealth(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(health_img, new Vector2(((position.X + image.Width/2)-health_img.Width/2), ((position.Y))), health_rect, Color.White);
            //Console.WriteLine(health);
        }

        public virtual void LostHealth(int h)
        {
            
                if (health > 0)
                {
                    health -= h;
                    health_rect.Width = (int) (health_img.Width * (health / (double)max_health));
                }

          
        }
    }
}