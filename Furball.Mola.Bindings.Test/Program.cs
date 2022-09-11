using System.Diagnostics;
using System.Numerics;
using SixLabors.ImageSharp;

namespace Furball.Mola.Bindings.Test;

public class Program {
	public static unsafe void Main() {
		RenderBitmap* renderBitmap = Mola.CreateRenderBitmap(128, 128, PixelType.Rgba32);

		Mola.ClearRenderBitmap(renderBitmap);

		double start = (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
		Mola.rasterize_triangle(renderBitmap, new Vertex { Position = new Vector2(10) }, new Vertex { Position = new Vector2(30, 100) }, new Vertex { Position = new Vector2(100, 50) });
		double end = (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;

		Console.WriteLine((end - start) * 1000d);

		Image img = renderBitmap->AsImage();

		img.SaveAsPng("test.png");

		Mola.DeleteRenderBitmap(renderBitmap);
	}
}
