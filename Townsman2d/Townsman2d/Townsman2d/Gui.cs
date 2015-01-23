using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Townsman2d
{
    enum RightPanelStates
    { 
        Nothing,
        Building,
        Resourse,
        Bog,
        Grass,
        Construct,
        Inventory,
        Upkeep,
        Build,
        Help,
        Terraform,
        Upgrade,
        Degrade,
        Productive,
        Market,
        Market2,
        Church,
        AllUpkeep,
        Captured,
        Quest,
        QuestGoal,
        MissionComplate,
        MissionInComplate,
        Event
    }

    enum RightPanelCommand
    {
        Upkeep,
        Attack,
        Build,
        Productive,
        Terraform,
        Upgrade,
        Degrade,
        Help,
        Up,
        Down,
        Ok,
        No,
        Market,
        Market2,
        Church,
        Inventory
    }

    class ColorMessage
    {
        public string text;
        public Color color;
        public double time;

        public ColorMessage(string txt,Color c,double t)
        {
            text = txt;
            color = c;
            time = t;
        }

        public bool Update(double dt)
        {
            time -= dt;
            if (time < 1) 
                color.A = (byte)(time*255);
            return dt <= 0;
        }
    }

    class Gui
    {
        public GuiObject[] elements;
        public List<ColorMessage> messages;
        public List<string> names;
        public string selectedName;
        public int selectedNameId;
        public RightPanelStates rightPanelState;
        public List<RightPanelCommand> rightPanelCommands;
        public int buildterraid;
        public int resourceid;
        public int resourceadd;
        public string eventmessage;
        
        public Gui() 
        { 
            elements = new GuiObject[44];
            
            names = new List<string>();
            messages = new List<ColorMessage>();
            selectedName = ""; 
            selectedNameId = -1;
            rightPanelCommands = new List<RightPanelCommand>();
            rightPanelState = RightPanelStates.Nothing;
            buildterraid=0;
            resourceadd = 0;
        }

        public void Update(MouseState mstate,GameState state,GameTime gameTime)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].drawstate == state&&elements[i].enable)
                {
                    elements[i].Update(mstate);
                    elements[i].updateFunction(ref elements[i]);
                }
            }

            for (int i = messages.Count - 1; i >= 0; i--)
            {
                messages[i].Update(gameTime.ElapsedGameTime.TotalSeconds);
                if (messages[i].time<=0) messages.RemoveAt(i);
            }
        }

        public void Draw(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, GameState state,SpriteBatch sb,SpriteFont fnt)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if ((elements[i].drawstate == GameState.Any || elements[i].drawstate == state)&&elements[i].enable)
                    elements[i].drawFunction(line, darkbackground, lightbackground, ref elements[i]);
            }

            for (int i = 0; i < messages.Count; i++)
            {
                sb.DrawString(fnt, messages[i].text, new Vector2(0, 100 + 20 * i), messages[i].color);
            }
        }
    }
}
