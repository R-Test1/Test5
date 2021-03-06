using System.ComponentModel;
using SkiaSharp;
using FormsRectangle = Xamarin.Forms.Shapes.Rectangle;


namespace Xamarin.Forms.Platform.Tizen.SkiaSharp
{
	public class RectangleRenderer : ShapeRenderer<FormsRectangle, RectView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<FormsRectangle> e)
		{
			if (Control == null)
			{
				SetNativeControl(new RectView());
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == VisualElement.WidthProperty.PropertyName || e.PropertyName == FormsRectangle.RadiusXProperty.PropertyName)
			{
				UpdateRadiusX();
			}
			else if (e.PropertyName == VisualElement.HeightProperty.PropertyName || e.PropertyName == FormsRectangle.RadiusYProperty.PropertyName)
			{
				UpdateRadiusY();
			}
		}

		void UpdateRadiusX()
		{
			if (Element.Width > 0)
				Control.UpdateRadiusX(Element.RadiusX / Element.Width);
		}

		void UpdateRadiusY()
		{
			if (Element.Height > 0)
				Control.UpdateRadiusY(Element.RadiusY / Element.Height);
		}
	}

	public class RectView : ShapeView
	{
		public RectView() : base()
		{
			UpdateShape();
		}

		public float RadiusX { set; get; }

		public float RadiusY { set; get; }

		void UpdateShape()
		{
			var path = new SKPath();
			path.AddRoundRect(new SKRect(0, 0, 1, 1), RadiusX, RadiusY, SKPathDirection.Clockwise);
			UpdateShape(path);
		}

		public void UpdateRadiusX(double radiusX)
		{
			RadiusX = (float)radiusX;
			UpdateShape();
		}

		public void UpdateRadiusY(double radiusY)
		{
			RadiusY = (float)radiusY;
			UpdateShape();
		}
	}
}
