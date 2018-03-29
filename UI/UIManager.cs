﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FishGame.UI
{
    public class UIManager
    {        
        List<UIPanel> PanelList;
        ContentManager _ContentManager;
        public UIManager()
        {
            PanelList = new List<UIPanel>();
        }

        public void LoadContent(ContentManager content)
        {
            _ContentManager = content;
        }

        public void CreatePanel(String name, Vector2 pos, Vector2 size, string texName)
        {
            UIPanel p = new UIPanel();
            p._Name = name;
            p._UIManager = this;
            p.LoadContent(texName);
            p._Position = pos;
            p.SetSize(size);
            PanelList.Add(p);
        }

        public Texture2D GetTexture(string texName)
        {
            return _ContentManager.Load<Texture2D>(@"Art/" + texName);
        }

        public UIPanel GetUIPanel(string name)
        {
            return PanelList.Find(x => x._Name == name);
        }

        public void TogglePanel(string name)
        {
            PanelList.Find(x => x._Name == name).ToggleShow();
        }

        public void AttachButton(UIButton b)
        {
            b.ButtonPressed += HandleButtonClicked;
        }

        private void HandleButtonClicked(UIButton b)
        {
            if(b._Name == "Store")
            {
                TogglePanel("Store");
            }
        }

        public void Update(GameTime gt)
        {
            foreach(UIPanel p in PanelList)
            {
                p.Update(gt);
            }
            //only handling clicks for now
            if (InputHelper.LeftButtonClicked)
            {
                foreach (UIPanel p in PanelList)
                {
                    p.ProcessClick(InputHelper.MouseScreenPos);
                }
            }
        }

        internal SpriteFont GetFont(string v)
        {
            return _ContentManager.Load<SpriteFont>("Fonts/"+v);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(UIPanel p in PanelList)
            {
                p.Draw(sb);
            }
        }
    }
}
