using OpenTK.Mathematics;
using Opxel.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opxel.Voxels
{
    internal class BlockProperty
    {
        public string Name { init; get; } = "Unnamed Block";
        public BlockTags Tags;
        public Vector2i[] UVXPositive { init; get; } = GetUVsFromUVStart(Vector2i.Zero);
        public Vector2i[] UVXNegative { init; get; } = GetUVsFromUVStart(Vector2i.Zero);
        public Vector2i[] UVYPositive { init; get; } = GetUVsFromUVStart(Vector2i.Zero);
        public Vector2i[] UVYNegative { init; get; } = GetUVsFromUVStart(Vector2i.Zero);
        public Vector2i[] UVZPositive { init; get; } = GetUVsFromUVStart(Vector2i.Zero);
        public Vector2i[] UVZNegative { init; get; } = GetUVsFromUVStart(Vector2i.Zero);

        public BlockProperty(string Name, Vector2i topFaceUVTextureStart, Vector2i sideFaceUVTextureStart, Vector2i bottomFaceUVTextureStart, BlockTags tags = BlockTags.None)
        {
            //Top
            UVYPositive = GetUVsFromUVStart(topFaceUVTextureStart);

            //Side
            UVXPositive = GetUVsFromUVStart(sideFaceUVTextureStart);
            UVXNegative = GetUVsFromUVStart(sideFaceUVTextureStart);
            UVZPositive = GetUVsFromUVStart(sideFaceUVTextureStart);
            UVZNegative = GetUVsFromUVStart(sideFaceUVTextureStart);

            //buttom
            UVYNegative = GetUVsFromUVStart(bottomFaceUVTextureStart);

            this.Tags = tags;
        }

        public Vector2i[] GetUVs(FaceDirection faceDirection)
        {
            switch(faceDirection)
            {
                case FaceDirection.XPositive:
                    return UVXPositive;
                case FaceDirection.XNegative:
                    return UVXNegative;
                case FaceDirection.YPositive:
                    return UVYPositive;
                case FaceDirection.YNegative:
                    return UVYNegative;
                case FaceDirection.ZPositive:
                    return UVZPositive;
                case FaceDirection.ZNegative:
                    return UVZNegative;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException(nameof(faceDirection), (int)faceDirection, typeof(FaceDirection));
            }
        }

        public static Vector2i[] GetUVsFromUVStart(Vector2i uvStart)
        {
            return new Vector2i[]
            {
                uvStart + new Vector2i(0, 1),
                uvStart + new Vector2i(1, 1),
                uvStart + new Vector2i(1, 0),
                uvStart
            };
        }
    }
}
