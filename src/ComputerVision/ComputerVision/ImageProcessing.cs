namespace ComputerVision;
using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

public class ImageProcessing
{
    public static void ProcessAllImages()
    {
        //string nibletFolder = @"../../../docs/niblet";
        //string notNibletFolder = @"../../../docs/not_niblet";
        
        string nibletFolder = "/Users/josephdejong/RiderProjects/SoftwinEnginwinning/docs/niblet";
        string notNibletFolder = "/Users/josephdejong/RiderProjects/SoftwinEnginwinning/docs/not_niblet";

        Console.WriteLine("Processing Niblet images...");
        ProcessFolder(nibletFolder, "Niblet");

        Console.WriteLine("\nProcessing Not-niblet images...");
        ProcessFolder(notNibletFolder, "not_niblet");

        Console.WriteLine("\nDone!");
    }

    public static void ProcessFolder(string folderPath, string label)
    {
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Folder not found: {folderPath}");
            return;
        }

        string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

        string outputFolder = Path.Combine(folderPath, "processed");
        Directory.CreateDirectory(outputFolder);

        foreach (var file in imageFiles)
        {
            if (file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (Image img = Image.Load(file))
                    {
                        Console.WriteLine($"{label}: {Path.GetFileName(file)} - {img.Width}x{img.Height}");

                        // --- RESIZE FIRST ---
                        using (var resizedImg = img.Clone(x => x.Resize(256, 256)))
                        {
                            string baseName = Path.GetFileNameWithoutExtension(file);

                            // Save the plain resized image
                            string outResized = Path.Combine(outputFolder, baseName + "_resized.png");
                            resizedImg.Save(outResized, new PngEncoder());

                            // Grayscale
                            using (var gray = resizedImg.Clone(x => x.Grayscale()))
                            {
                                gray.Save(Path.Combine(outputFolder, baseName + "_gray.png"), new PngEncoder());
                            }

                            // Inverted colors
                            using (var inverted = resizedImg.Clone(x => x.Invert()))
                            {
                                inverted.Save(Path.Combine(outputFolder, baseName + "_inverted.png"), new PngEncoder());
                            }

                            // Brightness adjustment +20%
                            using (var bright = resizedImg.Clone(x => x.Brightness(1.2f)))
                            {
                                bright.Save(Path.Combine(outputFolder, baseName + "_bright.png"), new PngEncoder());
                            }

                            // 30% increased contrast
                            using (var contrast = resizedImg.Clone(x => x.Contrast(1.3f)))
                            {
                                contrast.Save(Path.Combine(outputFolder, baseName + "_contrast.png"), new PngEncoder());
                            }

                            // sharpening
                            using (var sharpened = resizedImg.Clone(x => x.GaussianSharpen()))
                            {
                                sharpened.Save(Path.Combine(outputFolder, baseName + "_sharpen.png"), new PngEncoder());
                            }
                        }
                    }
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {file}: {ex.Message}");
                }
            }
        }
    }
}
