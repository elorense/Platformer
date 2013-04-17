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
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Platformer
{
    public class PlayerData
    {
        public String spriteName;
        public String spriteFlipped;
        public String spriteWalking;
        public String spriteJumping;
        public String jumpSound;
        public String landSound;
        public int health;
        public int jumpDistance;
        public float topSpeed;
        public float jumpSpeed;
        public float speed;
        public Vector2 center;
        public Vector2 startingPoint;
        
       
    }

    public class WorldData
    {
        public String stageName;
        public String floorName;
        public String floorTop;
        public String platformName;
        public String treasureName;
        public int floorCount, platFormCount;
        public float stageStart, stageEnd;
        public float stageTop, stageBottom;
        public Vector2 treasureLocation;
        public Vector2[] floorLocation;
        public Vector2[] platformLocations;
        public int[] movingPlatList;
        public int moveDistance;
        
    }

    public class MenuData
    {
        public String mainSong;
        public String gameOverSong;
    }

}

