using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Townsman2d
{
    public class Particle
    {
        public Vector2 position;
        public Vector2 offcet;
        public float lifetime;
        public float maxlifetime;

        public Particle(int t, Vector2 p, float l, bool up)
        {
            position = p;
            lifetime = l;
            maxlifetime = l;
            offcet = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            offcet.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
            lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public class ParticleManager
    {
        public List<Particle> particles;

        public ParticleManager()
        {
            particles = new List<Particle>();
        }

        public void AddNewParticle(Particle p)
        {
            particles.Add(p);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);
                if (particles[i].lifetime <= 0) particles.Remove(particles[i]);
                if (particles.Count == 0) break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D tex, Color lightColor)
        {
            foreach (Particle p in particles)
            {
                Vector2 pos = new Vector2(p.position.X * 68 + p.position.Y * 68, p.position.X * 34 - p.position.Y * 34);
                spriteBatch.Draw(tex, pos - camera.position + camera.offcet, new Rectangle(0, (int)(48 * p.lifetime / p.maxlifetime), 48, 48), lightColor);
            }
        }
    }
}