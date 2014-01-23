using System;
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

namespace LostIota
{
    public class Player : GameUnit
    {
        Texture2D player_img;
        Texture2D bullet_img;
        
        enum direction { up, down, left, right };
        direction currentDirection = direction.down;

        double shoot_cd = 0;
        List<Bullet> bulletLoad;

        SoundEffect shoot_sound;
        float sound_volume;

        public Player() :base()
        {
        }

        //Sprite Sheet loading. Page 41
        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            base.LoadContent(content, inputManager);
            fileManager = new FileManager();
            moveAnimation = new SpriteSheetAnimation();
            Vector2 temptFrames = Vector2.Zero;
            

            fileManager.LoadContent("Load/Player.cme", attributes, contents);
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

            bulletLoad = new List<Bullet>();
            bullet_img = this.content.Load<Texture2D>(@"bullet");

            sound_volume = 0.10f;

            moveAnimation.Frames = temptFrames;
            moveAnimation.LoadContent(content, image, "", position);

            shoot_sound = this.content.Load<SoundEffect>(@"Audios/shoot_audio");
            player_img = this.content.Load<Texture2D>(@"player_sprite");

          
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager inputManager, Collision col)
        {
            if (((inputManager.KeyDown(Keys.D) && !inputManager.KeyDown(Keys.W, Keys.A, Keys.S)) ||
                (inputManager.KeyDown(Keys.D) && currentDirection == direction.right))
                && position.X <= (GameConstants.windowWidth - player_img.Width / moveAnimation.Frames.X))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                position.X += GameConstants.moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentDirection = direction.right;
            }
            else if (((inputManager.KeyDown(Keys.A) && !inputManager.KeyDown(Keys.W, Keys.D, Keys.S)) ||
                inputManager.KeyDown(Keys.A) && currentDirection == direction.left)
                && position.X > 0)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                position.X -= GameConstants.moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentDirection = direction.left;
            }
            else if (((inputManager.KeyDown(Keys.W) && !inputManager.KeyDown(Keys.D, Keys.A, Keys.S)) ||
                inputManager.KeyDown(Keys.W) && currentDirection == direction.up)
                && position.Y > 0)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                position.Y -= GameConstants.moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentDirection = direction.up;
            }
            else if (((inputManager.KeyDown(Keys.S) && !inputManager.KeyDown(Keys.W, Keys.A, Keys.D)) ||
                inputManager.KeyDown(Keys.S) && currentDirection == direction.down)
                && position.Y <= (GameConstants.windowHeight - player_img.Height/moveAnimation.Frames.Y))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                position.Y += GameConstants.moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentDirection = direction.down;
            }

            for (int i = 0; i < col.collisionMap.Count; i++)
            {
                for (int j = 0; j < col.collisionMap[i].Count; j++)
                {
                    if (col.collisionMap[i][j] == "x")
                    {
                        if (position.X + moveAnimation.FrameWidth - 15 < j * GameConstants.tileWidth ||
                            position.X + 15 > j * GameConstants.tileWidth + GameConstants.tileWidth ||
                            position.Y + moveAnimation.FrameHeight - 10 < i * GameConstants.tileHeight ||
                            position.Y + 25 > i * GameConstants.tileHeight + GameConstants.tileHeight)
                        {
                            // no collision
                        }
                        else
                        {
                            position = moveAnimation.Position;
                        }
                    }
                }
            }
            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);

            if (inputManager.KeyDown(Keys.P))
            {  
                if (shoot_cd <= 0)
                {
                    Bullet b = new Bullet();
                    b.Position = new Vector2((this.position.X + 12), (this.position.Y + 8)); 

                    if (currentDirection == direction.up)
                        b.Direction = Bullet.direction.up;
                    else if (currentDirection == direction.down)
                        b.Direction = Bullet.direction.down;
                    else if (currentDirection == direction.left)
                        b.Direction = Bullet.direction.left;
                    else if (currentDirection == direction.right)
                        b.Direction = Bullet.direction.right;

                    bulletLoad.Add(b);
                    shoot_sound.Play(sound_volume, 0f, 0f);    
                    shoot_cd = GameConstants.cooldown;
                    LostHealth(10);
                }
            }

            foreach (Bullet b in bulletLoad)
            {
                float x = b.BulletPosX;
                float y = b.BulletPosY;
                switch (b.Direction)
                {
                    case Bullet.direction.right:
                        x += GameConstants.shotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        b.BulletPosX = x;
                        break;
                    case Bullet.direction.left:
                        x -= GameConstants.shotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        b.BulletPosX = x;
                        break;
                    case Bullet.direction.up:
                        y -= GameConstants.shotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        b.BulletPosY = y;
                        break;
                    case Bullet.direction.down:
                        y += GameConstants.shotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        b.BulletPosY = y;
                        break;
                }
                     
                
            }


            try
            {
                foreach (Bullet b in bulletLoad)
                {
                    if (b.BulletPosX > GameConstants.windowWidth || 
                        b.BulletPosX <= 0 ||
                        b.BulletPosY > GameConstants.windowHeight ||
                        b.BulletPosY <= 0)
                    {
                        bulletLoad.Remove(b);
                    }
                }
            }
            catch (InvalidOperationException e)
            {
            }

            if (shoot_cd <= 0)
                shoot_cd = 0;
            else if (shoot_cd > 0) 
                shoot_cd -= GameConstants.cooldown * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public List<Bullet> getBulletLoad()
        {
            return bulletLoad;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            foreach(Bullet b in bulletLoad)
            {
                spriteBatch.Draw(bullet_img, new Vector2(b.BulletPosX, b.BulletPosY), Color.White);
            }

            DrawHealth(spriteBatch);
        }

        private bool CheckCollision()
        {
            return false;
        }
    }

    public class Bullet
    {
        Vector2 position;
        bool shot_moving = false;
        public enum direction { up, down, left, right };
        direction currentDirection = direction.down;

        public Bullet()
        {
        }

        public direction Direction
        {
            get { return currentDirection; }
            set { currentDirection = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float BulletPosX
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float BulletPosY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public bool Shot_Moving
        {
            get { return shot_moving; }
            set { shot_moving = value; }
        }
    }
}
