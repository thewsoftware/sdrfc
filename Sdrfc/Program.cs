using FrequencyManagerCommon;
using FrequencyManagerCommon.Helpers;
using FrequencyManagerCommon.Models;

namespace SdrSharpToSdrPlusPlus;

class Program
{
    static void Main(string[] args)
    {
        const string SdrppDefaultFileName = "frequency_manager_config.json";
        const string SdrsharpDefaultFileName = "frequencies.xml";
        const string CopyOption = "-c";
        const string MergeOption = "-m";

        if (args.Length == 0)
        {
            Help();
        }
        else
        {
            var option = isOption(args[0]) ? args[0].Trim().ToLower() : CopyOption;

            if (!CopyOption.Equals(option) && !MergeOption.Equals(option))
            {
                Help();
            }

            var inputFileName = !isOption(args[0]) ? args[0] : args[1];
            
            var outputFileName = !isOption(args[0])
                ? (args.Length > 1 ? args[1] : null)
                : (args.Length > 2 ? args[2] : null);

            using (var sdrpp = new SdrPlusPlusHelper())
            using (var sdrsharp = new SdrSharpHelper())
            {
                var inputFileExt = Path.GetExtension(inputFileName);
                
                if (".json".Equals(inputFileExt.ToLower()))
                {
                    // convert from sdr++ format to sdr#

                    if (!File.Exists(inputFileName))
                    {
                        ErrorAndExit($"File does not exists; {inputFileName}");
                    }

                    var inputChannels = sdrpp.LoadFromFile(inputFileName);

                    if (inputChannels == null || !inputChannels.Any())
                    {
                        ErrorAndExit($"Invalid file format; {inputFileName} is not SDR++ frequency file.");
                    }

                    outputFileName = outputFileName?.ToLower() ?? SdrsharpDefaultFileName;

                    var outputChannels = File.Exists(outputFileName) 
                        ? sdrsharp.LoadFromFile(outputFileName)
                        : new List<Channel>();

                    try
                    {
                        using (var frequencyManager = new FrequencyManager(outputChannels))
                        {
                            if (File.Exists(outputFileName) && MergeOption.Equals(option))
                            {
                                WriteLine($"Merge {inputFileName.ToLower()} ({inputChannels.Count} channels) into {outputFileName.ToLower()} ({outputChannels.Count} channels).");
                                WriteLine($"Matching by [group name]+[frequency].");
                            }
                            else
                            {
                                WriteLine($"Copy {inputFileName.ToLower()} ({inputChannels.Count} channels) into {outputFileName.ToLower()}.");
                            }

                            (long updated, long added) = CopyOption.Equals(option)
                                ? frequencyManager.Copy(inputChannels)
                                : frequencyManager.Merge(inputChannels);
                            
                            WriteLine($"{updated} channels updated, {added} channels added.");

                            sdrsharp.SaveToFile(frequencyManager.Channels, outputFileName);
                        }

                        Exit();
                    }
                    catch (Exception ex )
                    {
                        ErrorAndExit(ex.Message);
                    };
                }
                else if (".xml".Equals(inputFileExt.ToLower()))
                {
                    // convert from sdr# format to sdr++

                    if (!File.Exists(inputFileName))
                    {
                        ErrorAndExit($"File does not exists; {inputFileName}");
                    }

                    var inputChannels = sdrsharp.LoadFromFile(inputFileName);

                    if (inputChannels == null || !inputChannels.Any())
                    {
                        ErrorAndExit($"Invalid file format; {inputFileName} is not SDR# frequency file.");
                    }

                    outputFileName = outputFileName?.ToLower() ?? SdrppDefaultFileName;

                    var outputChannels = File.Exists(outputFileName)
                        ? sdrpp.LoadFromFile(outputFileName)
                        : new List<Channel>();

                    try
                    { 
                        using (var frequencyManager = new FrequencyManager(outputChannels))
                        {
                            if (File.Exists(outputFileName) && MergeOption.Equals(option))
                            {
                                WriteLine($"Merge {inputFileName.ToLower()} ({inputChannels.Count} channels) into {outputFileName.ToLower()} ({outputChannels.Count} channels).");
                                WriteLine($"Matching by [group name]+[frequency].");
                            }
                            else
                            {
                                WriteLine($"Copy {inputFileName.ToLower()} ({inputChannels.Count} channels) into {outputFileName.ToLower()}.");
                            }

                            (long updated, long added) = CopyOption.Equals(option)
                                ? frequencyManager.Copy(inputChannels)
                                : frequencyManager.Merge(inputChannels);

                            WriteLine($"{updated} channels updated, {added} channels added.");

                            sdrpp.SaveToFile(frequencyManager.Channels, outputFileName);
                        }

                        Exit();
                    }
                    catch (Exception ex)
                    {
                        ErrorAndExit(ex.Message);
                    };
                }
                else
                {
                    Help();
                }
            }
        }
    }

    static private bool isOption(string text)
    {
        return text != null && text.Length == 2 && text.StartsWith("-");
    }

    static private void Help()
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine($" SDR#/SRD++ frequency file converter https://github.com/thewsoftware ");
        Console.WriteLine("==============================================================================");
        Console.WriteLine("");
        Console.WriteLine("Usage: sdrfc [-option] [drive:][path][inputfilename] [drive:][path][outputfilename]");
        Console.WriteLine("");
        Console.WriteLine("Options:");
        Console.WriteLine("-c\tCopy channels from input file to output file, overwrite the output file (default).");
        Console.WriteLine("-m\tMerge channels from input file to output file, save the result into the output file.");
        Console.WriteLine("");
        Console.WriteLine("Convert frequency channels from one format to another. Auto-detect input and output file formats.");
        Console.WriteLine("Create output file if it doesn't exist. If output file does exist and '-m' option is selected");
        Console.WriteLine("then frequency channels from input file are merged into the output file and saved.");
        Console.WriteLine("");
        Exit();
    }

    static private void Exit()
    {
        Environment.Exit(0);
    }

    static private void ErrorAndExit(string message)
    {
        WriteLine($"Error: {message}");
        Environment.Exit(-1);
    }

    static private void WriteLine(string text)
    {
        Console.WriteLine($"[SDRFC] {text}");
    }
}