// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using AppKit;
using System.Security;

namespace OpenGL
{
    public partial class GL
	{
        
        static partial void LoadPlatformEntryPoints()
		{
			BoundApi = RenderApi.ES;

            Viewport = iOSViewport;
		}

        private static void iOSViewport (int x, int y, int w, int h)
        {
        }

        private static IGraphicsContext PlatformCreateContext (IWindowInfo info)
        {
            return new GraphicsContext ();
        }

		internal static class EntryPointHelper {
			
			static IntPtr GL = IntPtr.Zero;

			static EntryPointHelper () 
			{
				try {
					GL = Dlfcn.dlopen("/System/Library/Frameworks/OpenGL.framework/OpenGL", 0);
				} catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine (ex.ToString());
				}
			}

			public static IntPtr GetAddress(String function)
			{
				if (GL == IntPtr.Zero)
					return IntPtr.Zero;

				return Dlfcn.dlsym (GL, function);
			}
		}
	}

    public class GraphicsContext : IGraphicsContext
    {
        public GraphicsContext ()
        {
            Context = new NSOpenGLContext (null, null);
        }

        public bool IsCurrent {
            get {
                return NSOpenGLContext.CurrentContext == this.Context;
            }
        }

        public bool IsDisposed {
            get {
                return this.Context == null;
            }
        }

        public int SwapInterval {
            get {
                throw new NotImplementedException ();
            }

            set {
                throw new NotImplementedException ();
            }
        }

        public void Dispose ()
        {
            if (this.Context != null) {
                this.Context.Dispose ();
            }
            this.Context = null;
        }

        public void MakeCurrent (IWindowInfo info)
        {
            this.Context.MakeCurrentContext();
        }

        public void SwapBuffers ()
        {
            //if (!this.Context.PresentRenderBuffer (36161u)) {
            //    throw new InvalidOperationException ("EAGLContext.PresentRenderbuffer failed.");
            //}
        }

        internal NSOpenGLContext Context { get; private set; }
    }
}

