// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
    partial class OcclusionQuery
    {
        private int glQueryId;

        private void PlatformConstruct()
        {
            GL.GenQueries(1, out glQueryId);
            GraphicsExtensions.CheckGLError();
        }

        private void PlatformBegin()
        {
            GL.BeginQuery(QueryTarget.SamplesPassed, glQueryId);
            GraphicsExtensions.CheckGLError();
        }

        private void PlatformEnd()
        {
            GL.EndQuery(QueryTarget.SamplesPassed);
            GraphicsExtensions.CheckGLError();
        }

        private bool PlatformGetResult(out int pixelCount)
        {
            int resultReady = 0;
            GL.GetQueryObject(glQueryId, GetQueryObjectParam.QueryResultAvailable, out resultReady);
            GraphicsExtensions.CheckGLError();

            if (resultReady == 0)
            {
                pixelCount = 0;
                return false;
            }

            GL.GetQueryObject(glQueryId, GetQueryObjectParam.QueryResult, out pixelCount);
            GraphicsExtensions.CheckGLError();

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Threading.BlockOnUIThread(() =>
                {
                    GL.DeleteQueries(1, ref glQueryId);
                    GraphicsExtensions.CheckGLError();
                });
            }

            base.Dispose(disposing);
        }
    }
}

