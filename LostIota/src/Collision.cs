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
    public class Collision
    {
        FileManager fileManager;
        ContentManager content;

        List<List<string>> attributes, contents;

        public List<List<string>> collisionMap;
        List<string> row;

        int tileWidth = 32;
        int tileHeight = 32;

        public void LoadContent(ContentManager content, string file)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            fileManager = new FileManager();

            fileManager.LoadContent(file, attributes, contents);

            collisionMap = new List<List<string>>();
            row = new List<string>();

            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < contents[i].Count; j++)
                {
                    row.Add(contents[i][j]);
                }
                collisionMap.Add(row);
                row = new List<string>();
            }
        }

        public void UnloadContent()
        {
            content.Unload();
            fileManager = null;

        }

        public void Update()
        {

        }

        public void Draw()
        {
        }
    }
}
