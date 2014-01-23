using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LostIota
{
    class Enemy : GameUnit
    {
        Texture2D enemy_img;
        bool hit;

        public bool Hit
        {
            get { return hit; }
            set { hit = value; }
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            hit = false;
            fileManager = new FileManager();
            moveAnimation = new SpriteSheetAnimation();
            Vector2 temptFrames = Vector2.Zero;

            fileManager.LoadContent("Load/Enemy.cme", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Health":
                            health = int.Parse(contents[i][j]);
                            max_health = health;
                            break;
                        case "Frames":
                            string[] frames = contents[i][j].Split(' ');
                            temptFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "Image":
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                        case "Position":
                            frames = contents[i][j].Split(' ');
                            position = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                    }
                }
            }

            moveAnimation.Frames = temptFrames;
            moveAnimation.LoadContent(content, image, "", position);
            moveAnimation.Scale = 1f;
            enemy_img = this.content.Load<Texture2D>(@"enemy");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            moveAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            DrawHealth(spriteBatch);
        }
    }
}
