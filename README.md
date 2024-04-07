# SDR# <-> SDR++ Frequency Converter

A lightweight command line tool to convert frequency lists berween SDR# and SDR++ formats

##Usage

sdrfc.exe [-option] [drive:][path][inputfilename] [drive:][path][outputfilename]

###Options
-c	Copy channels from input file to output file, overwrite the output file (default).
-m	Merge channels from input file to output file, save the result into the output file.

##Features
* Convert frequency channels from one format to another. 
* Auto-detect input and output file formats.
* Create output file if it doesn't exist. If output file does exist and '-m' option is selected
then frequency channels from input file are merged into the output file and saved.


