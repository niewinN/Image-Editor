using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageEditor
{
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap originalImage = null;
        private Button currentFilterButton = null;


        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    WriteableBitmap writeableBitmap = new WriteableBitmap(1, 1);
                    await writeableBitmap.SetSourceAsync(stream);
                    imageControl.Source = writeableBitmap;
                    // Zapisz oryginalny obraz
                    originalImage = writeableBitmap;
                }
            }
        }

       private void ToggleFilter(Button button, Action<WriteableBitmap> applyFilterAction)
{
    // Deselect previously selected button
    if (currentFilterButton != null && currentFilterButton != button)
    {
        currentFilterButton.Tag = false;
        currentFilterButton.Style = (Style)this.Resources["ButtonStyle"];
    }

    // Check if the clicked button is being selected or deselected
    if (button.Tag == null || (bool)button.Tag == false)
    {
        if (originalImage != null)
        {
            // Apply the new filter and update the UI
            WriteableBitmap modifiedImage = new WriteableBitmap(originalImage.PixelWidth, originalImage.PixelHeight);
            applyFilterAction(modifiedImage);

            button.Tag = true;
            button.Style = (Style)this.Resources["ButtonSelectedStyle"];
            currentFilterButton = button; // Update the currently selected button
        }
    }
    else
    {
        // Reset to original image and UI if the same button was clicked again
        imageControl.Source = originalImage;
        button.Tag = false;
        button.Style = (Style)this.Resources["ButtonStyle"];
        currentFilterButton = null; // No filter is currently selected
    }
}



        private void ApplyGrayscaleFilter_Click(object sender, RoutedEventArgs e)
        {
            ToggleFilter(grayscaleButton, modifiedImage =>
            {
                ProcessImage(pixels =>
                {
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        byte b = pixels[i];
                        byte g = pixels[i + 1];
                        byte r = pixels[i + 2];

                        byte gray = (byte)(.299 * r + .587 * g + .114 * b);

                        pixels[i] = gray; // Blue
                        pixels[i + 1] = gray; // Green
                        pixels[i + 2] = gray; // Red
                    }
                }, modifiedImage);
            });
        }


        private void InvertColors_Click(object sender, RoutedEventArgs e)
        {
            ToggleFilter(invertColorsButton, modifiedImage =>
            {
                ProcessImage(pixels =>
                {
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        pixels[i] = (byte)(255 - pixels[i]);       // B
                        pixels[i + 1] = (byte)(255 - pixels[i + 1]); // G
                        pixels[i + 2] = (byte)(255 - pixels[i + 2]); // R
                    }
                }, modifiedImage);
            });
        }

        private void ApplySepiaTone_Click(object sender, RoutedEventArgs e)
        {
            ToggleFilter(applySepiaToneButton, modifiedImage =>
            {
                ProcessImage(pixels =>
                {
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        byte originalRed = pixels[i + 2];
                        byte originalGreen = pixels[i + 1];
                        byte originalBlue = pixels[i];

                        pixels[i + 2] = (byte)Math.Min(.393 * originalRed + .769 * originalGreen + .189 * originalBlue, 255.0); // R
                        pixels[i + 1] = (byte)Math.Min(.349 * originalRed + .686 * originalGreen + .168 * originalBlue, 255.0); // G
                        pixels[i] = (byte)Math.Min(.272 * originalRed + .534 * originalGreen + .131 * originalBlue, 255.0);     // B
                    }
                }, modifiedImage);
            });
        }


        private void ContrastSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (originalImage != null)
            {
                WriteableBitmap modifiedImage = new WriteableBitmap(originalImage.PixelWidth, originalImage.PixelHeight);
                ProcessImage((pixels) =>
                {
                    double contrastLevel = (100.0 + contrastSlider.Value) / 100.0;
                    contrastLevel *= contrastLevel;

                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        for (int j = 0; j < 3; j++) // Apply for each RGB component
                        {
                            double pixel = pixels[i + j] / 255.0;
                            pixel -= 0.5;
                            pixel *= contrastLevel;
                            pixel += 0.5;
                            pixel *= 255;
                            if (pixel < 0) pixel = 0;
                            if (pixel > 255) pixel = 255;
                            pixels[i + j] = (byte)pixel;
                        }
                    }
                }, modifiedImage);
            }
        }

        private void BrightnessSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (originalImage != null)
            {
                WriteableBitmap modifiedImage = new WriteableBitmap(originalImage.PixelWidth, originalImage.PixelHeight);
                ProcessImage((pixels) =>
                {
                    int brightnessChange = (int)brightnessSlider.Value;

                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int pixelValue = pixels[i + j] + brightnessChange;
                            pixelValue = Math.Max(0, Math.Min(255, pixelValue));
                            pixels[i + j] = (byte)pixelValue;
                        }
                    }
                }, modifiedImage);
            }
        }

        private void SaturationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (originalImage != null)
            {
                WriteableBitmap modifiedImage = new WriteableBitmap(originalImage.PixelWidth, originalImage.PixelHeight);
                ProcessImage((pixels) =>
                {
                    float saturationValue = (float)saturationSlider.Value / 50.0f; // Zakres od -1 do 1
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        byte b = pixels[i];
                        byte g = pixels[i + 1];
                        byte r = pixels[i + 2];

                        float gray = 0.299f * r + 0.587f * g + 0.114f * b;
                        pixels[i] = (byte)Math.Clamp(b + (b - gray) * saturationValue, 0, 255); // Blue
                        pixels[i + 1] = (byte)Math.Clamp(g + (g - gray) * saturationValue, 0, 255); // Green
                        pixels[i + 2] = (byte)Math.Clamp(r + (r - gray) * saturationValue, 0, 255); // Red
                    }
                }, modifiedImage);
            }
        }

        private void MirrorImage_Click(object sender, RoutedEventArgs e)
        {
            ToggleFilter(mirrorImageButton, modifiedImage =>
            {
                int width = modifiedImage.PixelWidth;
                byte[] originalPixels = new byte[originalImage.PixelBuffer.Length];
                originalImage.PixelBuffer.AsStream().Read(originalPixels, 0, originalPixels.Length);

                ProcessImage(pixels =>
                {
                    for (int row = 0; row < modifiedImage.PixelHeight; row++)
                    {
                        for (int col = 0; col < width; col++)
                        {
                            int idx = (row * width + col) * 4;
                            int mirrorIdx = (row * width + (width - col - 1)) * 4;
                            for (int i = 0; i < 4; i++)
                            {
                                pixels[idx + i] = originalPixels[mirrorIdx + i];
                            }
                        }
                    }
                }, modifiedImage);
            });
        }

        private void ThresholdFilter_Click(object sender, RoutedEventArgs e)
        {
            ToggleFilter(thresholdFilterButton, modifiedImage =>
            {
                ProcessImage(pixels =>
                {
                    byte threshold = 128; // Można dostosować
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        byte gray = (byte)(0.299 * pixels[i + 2] + 0.587 * pixels[i + 1] + 0.114 * pixels[i]);
                        byte binaryColor = gray >= threshold ? (byte)255 : (byte)0;
                        pixels[i] = binaryColor;
                        pixels[i + 1] = binaryColor;
                        pixels[i + 2] = binaryColor;
                    }
                }, modifiedImage);
            });
        }

        private void Posterize_Click(object sender, RoutedEventArgs e)
        {
            ToggleFilter(posterizeButton, modifiedImage =>
            {
                ProcessImage(pixels =>
                {
                    int levels = 5; // Można dostosować
                    int factor = 255 / (levels - 1);
                    for (int i = 0; i < pixels.Length; i += 4)
                    {
                        pixels[i] = (byte)(factor * ((pixels[i] / factor)));
                        pixels[i + 1] = (byte)(factor * ((pixels[i + 1] / factor)));
                        pixels[i + 2] = (byte)(factor * ((pixels[i + 2] / factor)));
                    }
                }, modifiedImage);
            });
        }

        private async void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            savePicker.FileTypeChoices.Add("JPEG Image", new List<string>() { ".jpg" });
            savePicker.FileTypeChoices.Add("PNG Image", new List<string>() { ".png" });
            savePicker.SuggestedFileName = "ModifiedImage";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null && imageControl.Source is WriteableBitmap writeableBitmap)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = null;
                    switch (file.FileType)
                    {
                        case ".jpg":
                            encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                            break;
                        case ".png":
                            encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                            break;
                    }

                    Stream pixelStream = writeableBitmap.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                                         (uint)writeableBitmap.PixelWidth,
                                         (uint)writeableBitmap.PixelHeight,
                                         96.0,
                                         96.0,
                                         pixels);

                    await encoder.FlushAsync();
                }
            }
        }



        private void ProcessImage(Action<byte[]> pixelProcessor, WriteableBitmap bitmapToProcess)
        {
            using (var stream = originalImage.PixelBuffer.AsStream())
            {
                byte[] pixels = new byte[stream.Length];
                stream.Read(pixels, 0, pixels.Length);
                pixelProcessor(pixels);

                using (var writeStream = bitmapToProcess.PixelBuffer.AsStream())
                {
                    writeStream.Write(pixels, 0, pixels.Length);
                }
            }
            bitmapToProcess.Invalidate();
            imageControl.Source = bitmapToProcess;
        }
    }
}