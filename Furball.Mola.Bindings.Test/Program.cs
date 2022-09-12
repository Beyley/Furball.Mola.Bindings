using System.Diagnostics;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Furball.Mola.Bindings.Test;

public class Program {
	public static unsafe void Main() {
		#region Create texture

		RenderBitmap* texture = Mola.CreateRenderBitmap(360, 360, PixelType.Rgba32);

		Image<Rgba32> texImg = Image.Load<Rgba32>("2.png");

		Span<Rgba32> span = new(texture->Rgba32Ptr, (int)(texture->Width * texture->Height));
		texImg.CopyPixelDataTo(span);

		#endregion
		
		RenderBitmap* renderBitmap = Mola.CreateRenderBitmap(1024, 1024, PixelType.Rgba32);

		Mola.ClearRenderBitmap(renderBitmap);

		double  start    = (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
		Vertex* vertices = stackalloc Vertex[] {
			new Vertex { Position = new Vector2(10, 10), Color     = new RgbaVector(1, 0, 1), TexId = texture, TextureCoordinate = new Vector2(0) },
			new Vertex { Position = new Vector2(1000, 10), Color   = new RgbaVector(1, 1, 1), TexId = texture, TextureCoordinate = new Vector2(1, 0) },
			new Vertex { Position = new Vector2(1000, 1000), Color = new RgbaVector(1, 1, 0), TexId = texture, TextureCoordinate = new Vector2(1) },
			new Vertex { Position = new Vector2(10, 1000), Color   = new RgbaVector(0, 1, 1), TexId = texture, TextureCoordinate = new Vector2(0, 1) }
		};
		ushort* indices = stackalloc ushort[] {
			0, 1, 2, 0, 2, 3
		};
		Mola.DrawOntoBitmap(renderBitmap, vertices, indices, 6);
		// for (int i = 0; i < 1000; i++)
		// Mola.RasterizeTriangle(
		// 	renderBitmap,
		// 	new Vertex { Position = new Vector2(10, 10), Color  = new RgbaVector(1, 0, 0), TexId = texture, TextureCoordinate = new Vector2(0, 0)},
		// 	new Vertex { Position = new Vector2(30, 100), Color = new RgbaVector(0, 1, 0), TexId = texture, TextureCoordinate = new Vector2(0, 1)},
		// 	new Vertex { Position = new Vector2(100, 50), Color = new RgbaVector(0, 0, 1), TexId = texture, TextureCoordinate = new Vector2(1, 1)}
		// );
		double end = (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;

		Console.WriteLine((end - start) * 1000d);

		Image img = renderBitmap->AsImage();

		img.SaveAsPng("test.png");

		Mola.DeleteRenderBitmap(renderBitmap);
		Mola.DeleteRenderBitmap(texture);
	}
}
