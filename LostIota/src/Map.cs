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
    class Map
    {
        FileManager fileManager;
        ContentManager content;

        List<List<string>> attributes, contents;

        Texture2D map_img;

        Song background_music;
        bool songstart = false;
        float sound_volume = 0.35f;

        List<Texture2D> tiles;
        List<List<string>> tileMap;
        List<string> row;

        int tileWidth = 32;
        int tileHeight = 32;

        public void LoadContent(ContentManager content, string map_level)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            fileManager = new FileManager();

            fileManager.LoadContent(map_level, attributes, contents);

            tiles = new List<Texture2D>();
            tiles.Add(content.Load<Texture2D>(@"Tiles/GrassTile"));
            tiles.Add(content.Load<Texture2D>(@"Tiles/BrickWall"));
            tiles.Add(content.Load<Texture2D>(@"Tiles/Rock"));
            tileMap = new List<List<string>>();
            row = new List<string>();

            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < contents[i].Count; j++)
                {
                    row.Add(contents[i][j]);
                }
                tileMap.Add(row);
                row = new List<string>();
            }

            background_music = this.content.Load<Song>(@"Audios/audio01");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = sound_volume;
        }

        public void UnloadContent()
        {
            content.Unload();
            fileManager = null;
            background_music = null;
        }

        public void Update()
        {
            if (!songstart)
            {
                MediaPlayer.Play(background_music);
                songstart = true;
            }
        }

        public void Update(string map_level)
        {
            fileManager.LoadContent(map_level, attributes, contents);

            tileMap = new List<List<string>>();
            row = new List<string>();

            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < contents[i].Count; j++)
                {
                    row.Add(contents[i][j]);
                }
                tileMap.Add(row);
                row = new List<string>();
            }

            if (!songstart)
            {
                MediaPlayer.Play(background_music);
                songstart = true;
            }
            
        }

        public void stopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }

        public void pauseBackgroundMusic()
        {
            MediaPlayer.Pause();
        }

        public void resumeBackgroundMusic()
        {
            MediaPlayer.Resume();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(map_img, new Vector2(0, 0), new Rectangle(0, 0, map_img.Width, map_img.Height), Color.White, 0, new Vector2(map_img.Width/2, map_img.Height/2), 0.0f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(map_img, new Vector2(0, 0), Color.White);
            for (int y = 0; y < tileMap.Count; y++)
            {
                for (int x = 0; x < tileMap[y].Count; x++)
                {
                    string temp = (tileMap[y][x]);
                    spriteBatch.Draw(
                        tiles[int.Parse(temp)],
                        new Vector2(x * tileWidth, y * tileHeight),
                        Color.White);
                }
            }

        }
    }
}
