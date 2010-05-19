using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = System.String;
using TOutput = System.String;
using System.Diagnostics;

namespace Gdd.Game.Engine.Content.Pipeline.Processors
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Gdd.Game.Engine.Content.Pipeline.Processors.GroundProcessor")]
    public class GroundProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            Debugger.Launch();

            
            CalculateNormals(input);            

            return base.Process(input, context);
        }


        void CalculateNormals(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                MakeNormalsCorrect(mesh);
            }

            foreach (NodeContent child in node.Children)
            {
                CalculateNormals(child);
            }
        }

        private void MakeNormalsCorrect(MeshContent mesh)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            Vector3[] vertexNormals = new Vector3[mesh.Positions.Count];
            foreach (GeometryContent content in mesh.Geometry)
            {
                AccumulateTriangleNormals(content.Indices, content.Vertices, vertexNormals);
            }
            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i] = SafeNormalize(vertexNormals[i]);

                vertexNormals[i].Y = Math.Abs(vertexNormals[i].Y);
            }
            foreach (GeometryContent content2 in mesh.Geometry)
            {
                CreateNormalChannel(content2.Vertices, vertexNormals, true);
            }
        }

        private static void AccumulateTriangleNormals(IndexCollection indices, VertexContent vertices, Vector3[] vertexNormals)
        {
            IndirectPositionCollection positions = vertices.Positions;
            VertexChannel<int> positionIndices = vertices.PositionIndices;
            for (int i = 0; i < indices.Count; i += 3)
            {
                Vector3 vector4 = positions[indices[i]];
                Vector3 vector = positions[indices[i + 1]];
                Vector3 vector3 = positions[indices[i + 2]];

                Vector3 vector2 = Vector3.Cross(vector3 - vector, vector - vector4);
                vector2.Normalize();
                for (int j = 0; j < 3; j++)
                {
                    vector2.Y = Math.Abs(vector2.Y);
                    vertexNormals[positionIndices[indices[i + j]]] = vector2;
                }
            }
        }
        private static Vector3 SafeNormalize(Vector3 value)
        {
            float num = value.Length();
            if (num == 0f)
            {
                Debugger.Break();
                return Vector3.Zero;
            }
            return (Vector3)(value / num);
        }

        private static void CreateNormalChannel(VertexContent vertices, Vector3[] vertexNormals, bool overwriteExistingNormals)
        {
            VertexChannelCollection channels = vertices.Channels;
            if (overwriteExistingNormals)
            {
                channels.Remove(VertexChannelNames.Normal());
            }
            else if (channels.Contains(VertexChannelNames.Normal()))
            {
                return;
            }
            VertexChannel<Vector3> channel = channels.Add<Vector3>(VertexChannelNames.Normal(), null);
            VertexChannel<int> positionIndices = vertices.PositionIndices;
            for (int i = 0; i < channel.Count; i++)
            {
                channel[i] = vertexNormals[positionIndices[i]];
            }
        }
    }    
}