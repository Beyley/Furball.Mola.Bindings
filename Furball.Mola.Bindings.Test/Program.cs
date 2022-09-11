using SixLabors.ImageSharp;

namespace Furball.Mola.Bindings.Test; 

public class Program {
	public static unsafe void Main() {
		RenderBitmap* renderBitmap = Mola.CreateRenderBitmap(128, 128, PixelType.Rgba32);
		
		Mola.ClearRenderBitmap(renderBitmap);

		Image img = renderBitmap->AsImage();
		
		img.SaveAsPng("test.png");
							
		Mola.DeleteRenderBitmap(renderBitmap);
	}
}