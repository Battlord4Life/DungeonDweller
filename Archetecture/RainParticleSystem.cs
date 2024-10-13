using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;

namespace DungeonDweller.Archetecture
{
    public class RainParticleSystem : ParticleSystem
    {

        Rectangle _source;

        Color[] colors = new Color[]
        {
            Color.SteelBlue,
            Color.DimGray,
            Color.DarkOrange,
            Color.Gray,
            Color.Silver,
            Color.DarkGoldenrod,
        };

        Color color;

        public bool IsRaining { get; set; } = true;

        public RainParticleSystem(Game game, Rectangle source) : base(game, 4000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "particle";
            minNumParticles = 1;
            maxNumParticles = 5;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {


            p.Initialize(where, Vector2.UnitY * 260, Vector2.Zero, color, scale: RandomHelper.NextFloat(0.1f, 0.4f), lifetime: 3);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsRaining) {

                color = colors[RandomHelper.Next(colors.Length)];

                AddParticles(_source);
            }
        }


    }
}