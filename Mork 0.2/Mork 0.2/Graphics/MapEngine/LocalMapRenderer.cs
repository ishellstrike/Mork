using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork
{
    public partial class Main
    {
        VertexBuffer vertexbuffer;
        IndexBuffer indexbuffer;
        BasicEffect Effect;

        VertexPositionColor[] _vertices;
        UInt16[] _indices;
        private void LRInit()
        {
            vertexbuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 10000, BufferUsage.WriteOnly);
            indexbuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, 40000, BufferUsage.WriteOnly);
            Effect = new BasicEffect(GraphicsDevice);
        }

        private void LocalMapRenderer(GameTime gt, SpriteBatch sb, GraphicsDevice device)
        {
            device.SetVertexBuffer(vertexbuffer);
            device.Indices = indexbuffer;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
               // device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Length, 0, IndexCount / 3);
            }


            var temp = device.RasterizerState;
            sb.Begin();

            //sb.DrawString(Font1, string.Concat(fps_last, " fps | ", "Vertices: ", _quadTree.Vertices.Vertices.Length, " | Tris: ", _quadTree.IndexCount / 3, " | CULLING: " + _quadTree.Cull), new Vector2(11, 11), Color.Black);
            sb.DrawString(Font1, string.Concat("X " + Camera.Position.X), new Vector2(11, 26), Color.Black);
            sb.DrawString(Font1, string.Concat("Y " + Camera.Position.Y), new Vector2(11, 41), Color.Black);
            sb.DrawString(Font1, string.Concat("Z " + Camera.Position.Z), new Vector2(11, 56), Color.Black);

            sb.DrawString(Font1, string.Concat("Yaw " + Camera.Yaw), new Vector2(151, 26), Color.Black);
            sb.DrawString(Font1, string.Concat("Pitch " + Camera.Pitch), new Vector2(151, 41), Color.Black);
            sb.DrawString(Font1, string.Concat("FM " + sb.GraphicsDevice.RasterizerState.FillMode + " CM " + sb.GraphicsDevice.RasterizerState.CullMode), new Vector2(151, 56), Color.Black);

            //sb.DrawString(Font1, string.Concat(fps_last, " fps | ", "Vertices: ", _quadTree.Vertices.Vertices.Length, " | Tris: ", _quadTree.IndexCount / 3, " | CULLING: " + _quadTree.Cull), new Vector2(10, 10), Color.Wheat);
            sb.DrawString(Font1, string.Concat("X " + Camera.Position.X), new Vector2(10, 25), Color.Wheat);
            sb.DrawString(Font1, string.Concat("Y " + Camera.Position.Y), new Vector2(10, 40), Color.Wheat);
            sb.DrawString(Font1, string.Concat("Z " + Camera.Position.Z), new Vector2(10, 55), Color.Wheat);

            sb.DrawString(Font1, string.Concat("Yaw " + Camera.Yaw), new Vector2(150, 25), Color.Wheat);
            sb.DrawString(Font1, string.Concat("Pitch " + Camera.Pitch), new Vector2(150, 40), Color.Wheat);
            sb.DrawString(Font1, string.Concat("FM " + sb.GraphicsDevice.RasterizerState.FillMode + " CM " + sb.GraphicsDevice.RasterizerState.CullMode), new Vector2(151, 55), Color.Wheat);

            //sb.DrawString(Font1, pickRay.ToString(), new Vector2(50, 300), Color.Red);

            sb.End();

            smap.DrawAllMap(gt, Camera);

            device.RasterizerState = temp;
        }

        private void LRUpdate(GameTime gt)
        {
           // _vertices = new;

            //vertexbuffer.SetData<VertexPositionColor>(_vertices);
            //indexbuffer.SetData<UInt16>(_indices);
        }
    }
}
