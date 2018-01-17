using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using Trinet.Core.IO.Ntfs;

namespace Zoner
{
  class Program
  {

    static void ShowLogo()
    {
      Console.WriteLine("   _____");
      Console.WriteLine("  |___ / ___ _ __   ___ _ __");
      Console.WriteLine(@"    / // _ \| '_ \ / _ \ '__|");
      Console.WriteLine("   / /| (_) | | | | __ / |");
      Console.WriteLine(@"  /____\___/|_| | |\___|_| by @twiz718");
      Console.WriteLine("");
    }
    static void ShowUsage(OptionSet p)
    {
      Console.WriteLine("  Zoner will allow you to set the Zone.Identifier ADS (Alternate Data Stream) of a file.");
      Console.WriteLine("  `-> The Zone.Identifier values are enumerated here: https://msdn.microsoft.com/en-us/library/ms537175.aspx");
      Console.WriteLine("");
      p.WriteOptionDescriptions(Console.Out);
      Environment.Exit(0);
    }

    static void ListZones()
    {
      Console.WriteLine(" URLZONE_INVALID        = -1");
      Console.WriteLine(" URLZONE_PREDEFINED_MIN =  0");
      Console.WriteLine(" URLZONE_LOCAL_MACHINE  =  0");
      Console.WriteLine(" URLZONE_INTRANET       =  1");
      Console.WriteLine(" URLZONE_TRUSTED        =  2");
      Console.WriteLine(" URLZONE_INTERNET       =  3");
      Console.WriteLine(" URLZONE_UNTRUSTED      =  4");
      Console.WriteLine(" URLZONE_PREDEFINED_MAX =  999");
      Console.WriteLine(" URLZONE_USER_MIN       =  1000");
      Console.WriteLine(" URLZONE_USER_MAX       =  10000");
    }

    static void Main(string[] args)
    {
      bool quietMode = false;
      bool showUsage = false;
      string fileName = "";
      int zoneId = 0;
      bool set = false;
      bool get = false;
      bool delete = false;
      bool listZones = false;
      string referrerUrl = "";

      var p = new OptionSet()
        {
          { "q", "quiet mode", v => quietMode = v != null },
          { "f|filename=", "the {FILENAME} to set the Zone.Identifier value in.", v => fileName = v },
          { "z|zoneid=", "the {ZONE_ID} value (INTEGER ONLY)", (int v) => zoneId = v },
          { "u|url=", "the ReferrerUrl value to set (optional)", v => referrerUrl = v },
          { "s|set", "SET the value of {ZONE_ID} provided", v => set = v != null },
          { "g|get", "GET the value of {ZONE_ID} from {FILENAME}", v => get = v != null },
          { "d|delete", "DELETE the Zone.Identifier ADS from {FILENAME}", v => delete = v != null },
          { "l|listzones", "List the Zone.Identifier values", v => listZones = v != null },
          { "h|help", "show usage and exit", v => showUsage = v != null },
        };

      List<string> extra;
      try
      {
        extra = p.Parse(args);
      }
      catch (OptionException e)
      {
        Console.WriteLine(e.Message);
        Console.WriteLine("Try `Zoner.exe --help' for more information.");
        return;
      }

      if (quietMode != true)
      {
        ShowLogo();
      }

      if (showUsage)
      {
        ShowUsage(p);
      }
      else if (listZones)
      {
        ListZones();
      }

      if (get && fileName.Length > 0)
      {
        getZoneIdFromFileName(fileName, quietMode);
      }
      else if (set && fileName.Length > 0)
      {
        setZoneIdInFileName(fileName, zoneId, referrerUrl, quietMode);
      }
      else if (delete && fileName.Length > 0)
      {
        deleteZoneIdFromFile(fileName, quietMode);
      }
      else
      {
        ShowUsage(p);
      }

    }

    private static void deleteZoneIdFromFile(string fileName, bool quietMode)
    {
      var fileInfo = new FileInfo(fileName);

      if (fileInfo.AlternateDataStreamExists("Zone.Identifier"))
      {
        if (!quietMode)
        {
          Console.WriteLine("Deleting Zone.Identifier ADS from {0}", fileName);
        }
        fileInfo.DeleteAlternateDataStream("Zone.Identifier");
      }
      else
      {
        if (!quietMode)
        {
          Console.WriteLine("Zone.Identifier ADS is not found in {0}", fileName);
        }
      }
    }

    private static void setZoneIdInFileName(string fileName, int zoneId, string referrerUrl, bool quietMode)
    {
      var fileInfo = new FileInfo(fileName);

      if (fileInfo.AlternateDataStreamExists("Zone.Identifier"))
      {
        deleteZoneIdFromFile(fileName, quietMode);
      }

      AlternateDataStreamInfo ads = fileInfo.GetAlternateDataStream("Zone.Identifier", FileMode.OpenOrCreate);
      writeZoneIdentifier(fileName, zoneId, referrerUrl, quietMode, ads);
    }

    private static void writeZoneIdentifier(string fileName, int zoneId, string referrerUrl, bool quietMode, AlternateDataStreamInfo ads)
    {
      using (var stream = ads.OpenWrite())
      using (var writer = new StreamWriter(stream))
      {
        if (!quietMode)
        {
          Console.WriteLine("Writing Zone.Identifier value {0} to file {1}.", zoneId, fileName);
        }
        writer.WriteLine("[ZoneTransfer]");
        writer.WriteLine("ZoneId={0}", zoneId);
        if (referrerUrl.Length > 0)
        {
          if (!quietMode)
          {
            Console.WriteLine("Writing ReferrerUrl value: {0}", referrerUrl);
          }
          writer.WriteLine("ReferrerUrl={0}", referrerUrl);
        }
      }
    }

    private static void getZoneIdFromFileName(string fileName, bool quietMode)
    {
      var fileInfo = new FileInfo(fileName);

      // Read the "Zone.Identifier" stream, if it exists:
      if (fileInfo.AlternateDataStreamExists("Zone.Identifier"))
      {
        AlternateDataStreamInfo s = fileInfo.GetAlternateDataStream("Zone.Identifier", FileMode.Open);

        if (!quietMode)
        {
          Console.WriteLine("Found the Zone.Identifier ADS in {0}. Size: {1} bytes. ", fileName, s.Size);
          Console.WriteLine("--------------------[BEGIN]--------------------");
        }

        using (TextReader reader = s.OpenText())
        {
          Console.WriteLine(reader.ReadToEnd());
        }

        if (!quietMode)
        {
          Console.WriteLine("---------------------[END]---------------------");
        }
      }
      else
      {
        if (!quietMode)
        {
          Console.WriteLine("Zone.Identifier ADS is not found on {0}", fileName);
        }
      }

    }
  }
}
