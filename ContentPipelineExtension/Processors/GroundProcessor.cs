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
          //  Debugger.Launch();

            CalculateNormals(input);

            return base.Process(input, context);
        }


        void CalculateNormals(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                MeshHelper.CalculateNormals(mesh, true);                
            }

            foreach (NodeContent child in node.Children)
            {
                CalculateNormals(child);
            }
        }

     /*   private void MakeNormalsCorrect(MeshContent mesh)
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
                CreateNormalChannel(content2.Vertices, vertexNormals, overwriteExistingNormals);
            }
        }*/
    }    
}