using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Townsman2d
{
    class GuiObject
    {
        public Rectangle rect;
        public bool lpressed;
        public bool rpressed;
        public bool enable;
        public bool lclick;
        public bool rclick;
        public bool darktransparency;
        public bool lighttransparency;
        public GameState drawstate;
        public string text;
        public bool undercursor;

        public delegate void UpdateFunction(ref GuiObject me);
        public delegate void DrawFunction(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me);
        public DrawFunction drawFunction;
        public UpdateFunction updateFunction;


        static public void Nothing(bool l, bool r) { ;}
        public GuiObject() { ;}
        public GuiObject(Rectangle rec, bool dtr, bool ltr, GameState st, UpdateFunction f,DrawFunction f2, string text = "") 
        {
            rect = rec;
            lpressed = false;
            rpressed = false;
            enable = true;
            lclick = false;
            rclick = false;
            darktransparency = dtr;
            lighttransparency = ltr;
            drawstate = st;
            this.text = text;
            updateFunction = f;
            drawFunction = f2;
        }

        public void Update(MouseState state)
        {
            lclick = false;
            rclick = false;
            if (rect.Contains(new Point(state.X, state.Y)))
            {
                if (state.LeftButton == ButtonState.Pressed)
                    if (!lpressed) { lclick = true; lpressed = true; }
                if (lpressed && state.LeftButton == ButtonState.Released)
                    lpressed = false;

                if (state.RightButton == ButtonState.Pressed)
                    if (!rpressed) { rclick = true; rpressed = true; }
                if (rpressed && state.RightButton == ButtonState.Released)
                    rpressed = false;
                undercursor = true;
            }
            else undercursor = false;
            //Function(lclick, rclick);
        }
    }
}