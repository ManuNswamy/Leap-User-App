using Foundation;
using System;
using UIKit;

namespace LeapUser
{
    public partial class SplashViewController : UIViewController
    {
        public SplashViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			UIGraphics.BeginImageContext(View.Frame.Size);
			UIImage.FromBundle("splash.jpg").Draw(View.Bounds);
			var bgImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			var uiImageView = new UIImageView(View.Frame);
			uiImageView.Image = bgImage;
			View.AddSubview(uiImageView);
			View.SendSubviewToBack(uiImageView);
		}
    }
}