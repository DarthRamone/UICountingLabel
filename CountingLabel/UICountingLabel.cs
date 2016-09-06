using System;
using UIKit;
using CoreAnimation;
using Foundation;

namespace CountingLabel
{
  public class UICountingLabel : UILabel
  {
    public enum TimingFunction
    {
      EasyIn,
      EasyInOut,
      EasyOut,
      Linear
    }

    const double counterRate = 3.0;

    nfloat startingValue;
    nfloat destinationValue;
    double progress;
    double lastUpdate;
    double totalTime;

    CADisplayLink timer;

    public TimingFunction Function = TimingFunction.EasyIn;

    public void CountFrom(nfloat startValue, nfloat endValue, double duration)
    {
      startingValue = startValue;
      destinationValue = endValue;

      if(timer != null) {
        timer.Invalidate();
        timer = null;
      }

      if(duration == 0) {
        SetTextValue(endValue);
        //Complete();
        return;
      }

      progress = 0;
      totalTime = duration;
      lastUpdate = NSDate.Now.SecondsSinceReferenceDate;

      timer = CADisplayLink.Create(UpdateValue);
      timer.FrameInterval = 2;
      timer.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Default);
      timer.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.UITracking);
    }

    void UpdateValue()
    {
      double now = NSDate.Now.SecondsSinceReferenceDate;
      progress += now - lastUpdate;
      lastUpdate = now;

      if(progress >= totalTime) {
        timer.Invalidate();
        timer = null;
        progress = totalTime;
      }

      SetTextValue(CurrentValue());
    }

    nfloat CurrentValue()
    {
      if(progress >= totalTime) {
        return destinationValue;
      }
      var percent = progress / totalTime;

      double updateVal = 0;
      switch(Function) {
      case TimingFunction.EasyIn:
        updateVal = UpdateEasyIn(percent);
        break;
      case TimingFunction.EasyInOut:
        updateVal = UpdateEasyInOut(percent);
        break;
      case TimingFunction.EasyOut:
        updateVal = UpdateEasyOut(percent);
        break;
      case TimingFunction.Linear:
        updateVal = UpdateLinear(percent);
        break;
      }

      return (nfloat)(startingValue + (updateVal * (destinationValue - startingValue)));
    }

    double UpdateEasyInOut(double t) 
    {
      int sign = 1;
      int r = (int)counterRate;

      if(r % 2 == 0)
        sign = -1;
      
      t *= 2;

      if(t < 1)
        return 0.5f * Math.Pow(t, counterRate);
      
      return sign * 0.5f * (Math.Pow(t-2, counterRate) + sign * 2);
    }

    double UpdateEasyOut(double t) => 1.0 - Math.Pow((1.0 - t), counterRate);

    double UpdateEasyIn(double t) => Math.Pow(t, counterRate);

    double UpdateLinear(double t) => t;

    void SetTextValue(nfloat value)
    {
      Text = Math.Round(value, 0).ToString();
    }
  }
}
