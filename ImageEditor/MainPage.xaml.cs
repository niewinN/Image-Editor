using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageEditor
{
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap originalImage = null;

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
            if (button.Tag == null || (bool)button.Tag == false)
            {
                if (originalImage != null)
                {
                    WriteableBitmap modifiedImage = new WriteableBitmap(originalImage.PixelWidth, originalImage.PixelHeight);
                    applyFilterAction(modifiedImage);

                    button.Tag = true;
                    button.Style = (Style)this.Resources["ButtonSelectedStyle"];
                }
            }
            else
            {
                imageControl.Source = originalImage;
                button.Tag = false;
                button.Style = (Style)this.Resources["ButtonStyle"];
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
