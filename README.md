Usage:
```
$ ./Zoner.exe -f /c/Users/akhanin/Desktop/autoruns/autoruns.exe -g -h
   _____
  |___ / ___ _ __   ___ _ __
    / // _ \| '_ \ / _ \ '__|
   / /| (_) | | | | __ / |
  /____\___/|_| | |\___|_| by @twiz718

  Zoner will allow you to set the Zone.Identifier ADS (Alternate Data Stream) of a file.
  `-> The Zone.Identifier values are enumerated here: https://msdn.microsoft.com/en-us/library/ms537175.aspx

  -q                         quiet mode
  -f, --filename=FILENAME    the FILENAME to set the Zone.Identifier value in.
  -z, --zoneid=ZONE_ID       the ZONE_ID value (INTEGER ONLY)
  -u, --url=VALUE            the ReferrerUrl value to set (optional)
  -s, --set                  SET the value of ZONE_ID provided
  -g, --get                  GET the value of ZONE_ID from FILENAME
  -d, --delete               DELETE the Zone.Identifier ADS from FILENAME
  -l, --listzones            List the Zone.Identifier values
  -h, --help                 show usage and exit
```

Getting the Zone.Identifier from a file:
```
$ ./Zoner.exe -f /c/Users/akhanin/Desktop/autoruns/autoruns.exe -g -q
[ZoneTransfer]
ZoneId=0
ReferrerUrl=foo
```

Setting the Zone.Identifier in a file:
```
$ ./Zoner.exe -f /c/Users/akhanin/Desktop/autoruns/autoruns.exe -s -z 4
   _____
  |___ / ___ _ __   ___ _ __
    / // _ \| '_ \ / _ \ '__|
   / /| (_) | | | | __ / |
  /____\___/|_| | |\___|_| by @twiz718

Deleting Zone.Identifier ADS from C:/Users/akhanin/Desktop/autoruns/autoruns.exe
Writing Zone.Identifier value 4 to file C:/Users/akhanin/Desktop/autoruns/autoruns.exe.
```

Deleting the Zone.Identifier from a file:
```
$ ./Zoner.exe -f /c/Users/akhanin/Desktop/autoruns/autoruns.exe -d
   _____
  |___ / ___ _ __   ___ _ __
    / // _ \| '_ \ / _ \ '__|
   / /| (_) | | | | __ / |
  /____\___/|_| | |\___|_| by @twiz718

Deleting Zone.Identifier ADS from C:/Users/akhanin/Desktop/autoruns/autoruns.exe
```

