using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork.Graphics
{
    [Serializable]
    public struct VertexPositionNormalTextureShade : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public float Shade;
        public VertexPositionNormalTextureShade(Vector3 position, Vector3 normal, Vector2 textureCoordinate, float shade)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
            Shade = shade;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1));

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

        public static int SizeInBytes { get { return sizeof(float) * 9; } }
    }
}
