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
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;


namespace Platformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class World : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Player playerOne { get; set; }
        public int worldWidth { get; set; }
        public int worldHeight { get; set; }
        public Camera camera { get; set; }
        public MenuScreen menuScreen;
        private WorldData[] worldData;
        private Texture2D treasureTexture;
        private int[] movingPlatList;
        private WorldObject treasureObj;
        
        public List<WorldObject> worldObjectList;
        public float worldLeftBound, worldRightBound, worldTopBound, worldBottomBound;

        public World(Game game)
            : base(game)
        {
            playerOne = new Player(game);
            playerOne.mWorld = this;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            worldData = content.Load<WorldData[]>("WorldFile");
            movingPlatList = worldData[0].movingPlatList;
            worldObjectList = new List<WorldObject>();
            worldLeftBound = worldData[0].stageStart;
            worldRightBound = worldData[0].stageEnd;
            worldBottomBound = worldData[0].stageBottom;
            worldTopBound = worldData[0].stageTop;
            
            LoadWorld(content, "Content/WorldData.xml");
            //worldObjectList.Add(treasureObj);
            playerOne.camera = camera;
            playerOne.menuScreen = menuScreen;
            playerOne.LoadContent(content);
        }

        private void LoadWorld(ContentManager content, String wordFile)
        {
            using (XmlReader reader = XmlReader.Create(new StreamReader(wordFile)))
            {
                XDocument xml = XDocument.Load(reader);
                XElement root = xml.Root;
                foreach (XElement elem in root.Elements())
                {
                    WorldObject obj = new WorldObject();
                    foreach (XElement innerElem in elem.Elements())
                    {
                        XMLParse.AddValueToClassInstance(innerElem, obj);
                    }
                    
                    obj.setTexture(content.Load<Texture2D>(obj.TextureName));
                    if (obj.CanFall) obj.color = Color.Black;
                    if (obj.DoesWalk)
                    {
                        obj.setWalkTexture(content.Load<Texture2D>(obj.WalkingTextureName));
                        obj.walkAnimList = new List<Texture2D>();
                        obj.walkAnimList.Add(obj.Texture);
                        obj.walkAnimList.Add(obj.WalkingTexture);
                    }   
                    worldObjectList.Add(obj);
                }
            }
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            playerOne.Update(gameTime);
            int i = 0;
            foreach (WorldObject obj in worldObjectList)
            {
                obj.Update(gameTime, i);
                i++;
            }

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (WorldObject obj in worldObjectList)
            {
                if (obj.DoesHurt || obj.CanFall) obj.color = Color.Red;
                camera.Draw(obj);
            }
            playerOne.Draw(sb);

        }
    }
}
