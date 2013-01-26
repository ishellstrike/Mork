using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Graphics;
using Mork.Graphics.MapEngine;

namespace Mork.Local_Map.Dynamic.Units
{
    public class LocalHeroes
    {
        public List<LocalHero> n = new List<LocalHero>();
        private VertexPositionNormalTextureShade[] verts = new VertexPositionNormalTextureShade[6];
        private BasicEffect be;

        public LocalHeroes(GraphicsDevice gd, Texture2D tex)
        {
            this.be = new BasicEffect(gd);
            be.TextureEnabled = true;
            be.Texture = tex;

            this.verts[0] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(0, 0), 1);
            this.verts[1] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(1, 0), 1);
            this.verts[2] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(0, 1), 1);

            this.verts[3] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(0, 1), 1);
            this.verts[4] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(1, 0), 1);
            this.verts[5] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(1, 1), 1);
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < n.Count; i++)
            {
                var lh = n[i];
                lh.pre_pos = lh.pos;
                lh.MoveUnit(gt);
                if (lh.pos == lh.pre_pos) lh.iddle_time += gt.ElapsedGameTime;
                else lh.iddle_time = TimeSpan.Zero;
            }
        }

        public void Draw(FreeCamera camera, GraphicsDevice gd, int cam_z)
        {
            be.View = camera.View;
            be.Projection = camera.Projection;

            RasterizerState last = gd.RasterizerState;
            RasterizerState news = new RasterizerState();
            news.CullMode = CullMode.None;

            gd.RasterizerState = news;

            foreach (var pass in be.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var lh in n)
                {
                    if (lh.pos.Z >= cam_z -1)
                    {
                        verts[0].Position = new Vector3(lh.pos.X, lh.pos.Y, -lh.pos.Z);
                        verts[1].Position = new Vector3(lh.pos.X + 1, lh.pos.Y, -lh.pos.Z);
                        verts[2].Position = new Vector3(lh.pos.X, lh.pos.Y, -lh.pos.Z - 1);
                        verts[3].Position = new Vector3(lh.pos.X, lh.pos.Y, -lh.pos.Z - 1);
                        verts[4].Position = new Vector3(lh.pos.X + 1, lh.pos.Y, -lh.pos.Z);
                        verts[5].Position = new Vector3(lh.pos.X + 1, lh.pos.Y, -lh.pos.Z - 1);

                        gd.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);
                    }

                }
            }

            gd.RasterizerState = last;
        }
    }
}
