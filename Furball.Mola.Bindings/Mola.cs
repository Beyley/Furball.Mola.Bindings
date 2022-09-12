using System;
using System.Numerics;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Furball.Mola.Bindings;

public unsafe struct RenderBitmap {
	public PixelType PixelType;
	public uint      Width;
	public uint      Height;
	public Rgba32*   Rgba32Ptr;
	public Argb32*   Argb32Ptr;

	public Image AsImage() {
		long destSize = this.Width * this.Height * sizeof(Rgba32);
		switch (this.PixelType) {
			case PixelType.Rgba32: {
				Rgba32[] arr = new Rgba32[this.Width * this.Height];

				fixed (void* ptr = arr) {
					Buffer.MemoryCopy(this.Rgba32Ptr, ptr, destSize, destSize);
				}

				Image<Rgba32> img = Image.LoadPixelData(arr, (int)this.Width, (int)this.Height);
				return img;
			}
			case PixelType.Argb32: {
				Argb32[] arr = new Argb32[this.Width * this.Height];

				fixed (void* ptr = arr) {
					Buffer.MemoryCopy(this.Argb32Ptr, ptr, destSize, destSize);
				}

				Image<Argb32> img = Image.LoadPixelData(arr, (int)this.Width, (int)this.Height);
				return img;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}

public enum PixelType : byte {
	Rgba32,
	Argb32
}

public unsafe struct Vertex {
	public Vector2       Position;
	public Vector2       TextureCoordinate;
	public RgbaVector    Color;
	public RenderBitmap* TexId;
}

public static class Mola {
	private const string            DLL_NAME           = "Mola";
	private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

	[DllImport(DLL_NAME, CallingConvention = CALLING_CONVENTION, EntryPoint = "create_render_bitmap")]
	public static extern unsafe RenderBitmap* CreateRenderBitmap(uint width, uint height, PixelType pixelType);

	[DllImport(DLL_NAME, CallingConvention = CALLING_CONVENTION, EntryPoint = "delete_render_bitmap")]
	public static extern unsafe void DeleteRenderBitmap(RenderBitmap* bitmap);

	[DllImport(DLL_NAME, CallingConvention = CALLING_CONVENTION, EntryPoint = "clear_render_bitmap")]
	public static extern unsafe void ClearRenderBitmap(RenderBitmap* bimap);

	[DllImport(DLL_NAME, CallingConvention = CALLING_CONVENTION, EntryPoint = "rasterize_triangle")]
	public static extern unsafe void RasterizeTriangle(RenderBitmap* bitmap, Vertex vtx1, Vertex vtx2, Vertex vtx3);

	[DllImport(DLL_NAME, CallingConvention = CALLING_CONVENTION, EntryPoint = "rasterize_line")]
	public static extern unsafe void RasterizeLine(RenderBitmap* bitmap, float x1, float y1, float x2, float y2, Rgba32 col);
	
	[DllImport(DLL_NAME, CallingConvention = CALLING_CONVENTION, EntryPoint = "draw_onto_bitmap")]
	public static extern unsafe void DrawOntoBitmap(RenderBitmap* bitmap, Vertex* vertices, ushort* indices, uint indexCount);
}
