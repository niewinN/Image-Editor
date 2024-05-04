﻿#pragma checksum "C:\Users\piotr\source\repos\ImageEditor\ImageEditor\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D666617780A0B1BD7D27A74582D51863DE0B7256E37A70C34F415D1662C6D13A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImageEditor
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 25
                {
                    global::Windows.UI.Xaml.Controls.Button element2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element2).Click += this.LoadImage_Click;
                }
                break;
            case 3: // MainPage.xaml line 26
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.TakePhoto_Click;
                }
                break;
            case 4: // MainPage.xaml line 27
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.SaveImage_Click;
                }
                break;
            case 5: // MainPage.xaml line 28
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.RecordVideo_Click;
                }
                break;
            case 6: // MainPage.xaml line 29
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.RecordAudio_Click;
                }
                break;
            case 7: // MainPage.xaml line 30
                {
                    this.previewControl = (global::Windows.UI.Xaml.Controls.CaptureElement)(target);
                }
                break;
            case 8: // MainPage.xaml line 35
                {
                    this.grayscaleButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.grayscaleButton).Click += this.ApplyGrayscaleFilter_Click;
                }
                break;
            case 9: // MainPage.xaml line 36
                {
                    this.invertColorsButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.invertColorsButton).Click += this.InvertColors_Click;
                }
                break;
            case 10: // MainPage.xaml line 37
                {
                    this.applySepiaToneButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.applySepiaToneButton).Click += this.ApplySepiaTone_Click;
                }
                break;
            case 11: // MainPage.xaml line 38
                {
                    this.mirrorImageButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.mirrorImageButton).Click += this.MirrorImage_Click;
                }
                break;
            case 12: // MainPage.xaml line 39
                {
                    this.thresholdFilterButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.thresholdFilterButton).Click += this.ThresholdFilter_Click;
                }
                break;
            case 13: // MainPage.xaml line 40
                {
                    this.posterizeButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.posterizeButton).Click += this.Posterize_Click;
                }
                break;
            case 14: // MainPage.xaml line 41
                {
                    this.contrastSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.contrastSlider).ValueChanged += this.ContrastSlider_ValueChanged;
                }
                break;
            case 15: // MainPage.xaml line 42
                {
                    this.brightnessSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.brightnessSlider).ValueChanged += this.BrightnessSlider_ValueChanged;
                }
                break;
            case 16: // MainPage.xaml line 43
                {
                    this.saturationSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.saturationSlider).ValueChanged += this.SaturationSlider_ValueChanged;
                }
                break;
            case 17: // MainPage.xaml line 33
                {
                    this.imageControl = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

