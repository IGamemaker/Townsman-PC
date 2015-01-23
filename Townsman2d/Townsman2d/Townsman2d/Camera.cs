using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Townsman2d
{
    class Camera
    {
        public Vector2 position;
        public Vector2 offcet;
        public Vector2 mouse;
        public MouseState mousestate;
        public Vector2 window;
        public Vector2 mapposition;
        public Vector2 underposition;
        public Vector2 mapviewport;
        public Vector2 mapsize;
        public Color light;

        public Camera(Vector2 wnd, Vector2 mpsz)
        {
            position = Vector2.Zero;
            window = wnd;
            underposition=mapposition = new Vector2(-1, -1);
            mapsize = mpsz;
            offcet = (wnd - new Vector2((mpsz.X + mpsz.Y) * 68, (mpsz.X - mpsz.Y) * 34) + new Vector2(68, 34)) / 2;
            if (offcet.X < 0) offcet.X = 0;
            if (offcet.Y < 0) offcet.Y = 0;

            mapviewport = (new Vector2((mpsz.X + mpsz.Y) * 68, (mpsz.X + mpsz.Y) * 34) - wnd) / 2;
            mapviewport.X += 150;
            if (mapviewport.X < 0) mapviewport.X = 0;
            if (mapviewport.Y < 0) mapviewport.Y = 0;

            light = Color.White;
        }

        public void Update(MouseState ms)
        {
            mousestate = ms;
            mouse = new Vector2(ms.X, ms.Y);

            if (mouse.X < 2 && position.X > -mapviewport.X) position.X -= 5;
            if (mouse.X > window.X - 2 && position.X < mapviewport.X*3) position.X += 5;
            if (mouse.Y < 2 && position.Y > -mapviewport.Y-140) position.Y -= 5;
            if (mouse.Y > window.Y - 2 && position.Y < mapviewport.Y+16) position.Y += 5;

            Vector2 c = mouse + position - offcet + new Vector2(34, 17);

            int x = (int)c.X;
            int y = (int)c.Y;

            underposition.X = (x / 68 + y / 34) / 2;
            underposition.Y = x / 68 - underposition.X;
        }

        public float GetZ(float y, float offct = 0)
        {
            float z = 1-((y / 2 + offct) / window.Y + 0.25f);
            return z;
        }
    }
}
