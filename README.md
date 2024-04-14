# SDR# <-> SDR++ Frequency Converter

A lightweight command line tool to convert frequency lists berween SDR# and SDR++ formats

## Usage

```sh
sdrfc.exe [-option] [drive:][path][inputfilename] [drive:][path][outputfilename]
```

### Options
* `-c` - Copy channels from input file to output file, overwrite the output file (default).
* `-m` - Merge channels from input file to output file, save the result into the output file.

### Warning 
Make sure SDR++/SDR# application are not running when updating frequency files. Both apps load frequency files when started, manage frequency channels in the memory and overwrite files when stopped.

## Features
* Convert frequency channels from one format to another. 
* Auto-detect input and output file formats.
* Create output file if it doesn't exist. If output file does exist and `-m` option is selected
then frequency channels from input file are merged into the output file and saved.

## Installation
Download the latest sdrfc.zip from https://github.com/thewsoftware/sdrfc/releases and unpack it into a designated folder. 
Executables are build with .NET 6.0 framework which is included into Windows 10 by default.



