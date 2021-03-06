// 
// CMSampelBuffer.cs: Implements the managed CMSampleBuffer
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
#if !COREBUILD
using MonoMac.CoreVideo;
#endif

namespace MonoMac.CoreMedia {

	[Since (4,0)]
	public class CMSampleBuffer : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMSampleBuffer (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMSampleBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMSampleBuffer ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
#if !COREBUILD
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetImageBuffer (IntPtr handle);

		public CVImageBuffer GetImageBuffer ()
		{
			IntPtr ib = CMSampleBufferGetImageBuffer (handle);
			if (ib == IntPtr.Zero)
				return null;

			var ibt = CFType.GetTypeID (ib);
			if (ibt == CVPixelBuffer.CVImageBufferType)
				return new CVPixelBuffer (ib, false);
			return new CVImageBuffer (ib, false);
		}
#endif
	}
}
