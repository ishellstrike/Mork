using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Bad_Database;
using Mork.Graphics;
using Mork.Graphics.MapEngine;

namespace Mork.Local_Map.Dynamic.Units {
    public class LocalUnits {
        private readonly BasicEffect be;
        private readonly VertexPositionNormalTextureShade[] verts = new VertexPositionNormalTextureShade[6];
        public List<LocalUnit> N = new List<LocalUnit>();

        public LocalUnits(GraphicsDevice gd, Texture2D tex) {
            be = new BasicEffect(gd);
            be.TextureEnabled = true;
            be.Texture = tex;

            verts[0] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(0, 0), 1);
            verts[1] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(1, 0), 1);
            verts[2] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(0, 1), 1);

            verts[3] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(0, 1), 1);
            verts[4] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(1, 0), 1);
            verts[5] = new VertexPositionNormalTextureShade(new Vector3(), Vector3.One, new Vector2(1, 1), 1);
        }

        public int Count {
            get { return N.Count; }
        }

        public void Update(GameTime gt) {
            for (int i = 0; i < N.Count; i++) {
                LocalUnit lh = N[i];
                lh.PrePos = lh.Pos;
                lh.MoveUnit(gt);
                if (lh.Pos == lh.PrePos) {
                    lh.IddleTime += gt.ElapsedGameTime;
                }
                else {
                    lh.IddleTime = TimeSpan.Zero;
                }
            }
        }

        public void Draw(FreeCamera camera, GraphicsDevice gd, int cam_z) {
            be.View = camera.View;
            be.Projection = camera.Projection;

            float tsper = 1/(Commons.TextureAtlas.X/(Commons.TextureAtlasTexSize));
            float tsperh = 1/(Commons.TextureAtlas.Y/(Commons.TextureAtlasTexSize));

            RasterizerState last = gd.RasterizerState;

            gd.RasterizerState = new RasterizerState {CullMode = CullMode.None};

            foreach (EffectPass pass in be.CurrentTechnique.Passes) {
                pass.Apply();

                foreach (LocalUnit lh in N) {
                    if (lh.Pos.Z >= cam_z - 1) {
                        float umovx = (Main.dbcreatures.Data[lh.ID].MetatexN%(Commons.TextureAtlasWCount*2))*tsper;
                        float umovy = (Main.dbcreatures.Data[lh.ID].MetatexN/(Commons.TextureAtlasWCount*2))*tsperh;

                        verts[0].TextureCoordinate = new Vector2(0 + umovx, 0 + umovy);
                        verts[1].TextureCoordinate = new Vector2(tsper + umovx, 0 + umovy);
                        verts[2].TextureCoordinate = new Vector2(0 + umovx, tsperh + umovy);
                        verts[3].TextureCoordinate = new Vector2(0 + umovx, tsperh + umovy);
                        verts[4].TextureCoordinate = new Vector2(tsper + umovx, 0 + umovy);
                        verts[5].TextureCoordinate = new Vector2(tsper + umovx, tsperh + umovy);

                        verts[0].Position = new Vector3(lh.Pos.X, lh.Pos.Y, -lh.Pos.Z);
                        verts[1].Position = new Vector3(lh.Pos.X + 1, lh.Pos.Y, -lh.Pos.Z);
                        verts[2].Position = new Vector3(lh.Pos.X, lh.Pos.Y, -lh.Pos.Z - 1);
                        verts[3].Position = new Vector3(lh.Pos.X, lh.Pos.Y, -lh.Pos.Z - 1);
                        verts[4].Position = new Vector3(lh.Pos.X + 1, lh.Pos.Y, -lh.Pos.Z);
                        verts[5].Position = new Vector3(lh.Pos.X + 1, lh.Pos.Y, -lh.Pos.Z - 1);

                        gd.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);
                    }
                }
            }

            gd.RasterizerState = last;
        }
    }
}