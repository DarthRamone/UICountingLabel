using System;
using UIKit;

namespace CountingLabel
{
  public class LabelViewController : UIViewController
  {
    UICountingLabel _label;
    UIButton _button;

    public LabelViewController()
    {
      _label = new UICountingLabel();
      _label.Text = "0";
      _label.TextColor = UIColor.Black;

      _button = new UIButton();
      _button.SetTitle("count", UIControlState.Normal);
      _button.SetTitleColor(UIColor.Blue, UIControlState.Normal);
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      View.BackgroundColor = UIColor.White;

      View.AddSubview(_label);
      View.AddSubview(_button);

      _label.Frame = new CoreGraphics.CGRect(30, 30, 100, 40);

      _label.Function = UICountingLabel.TimingFunction.EasyInOut;

      _button.Frame = new CoreGraphics.CGRect(30, 80, 100, 40);

      _button.TouchUpInside += (sender, e) => {
        _label.CountFrom(0, 300, 4);
      };
    }
  }
}

