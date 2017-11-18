# midicsv2ftmexport
Convert a midi csv file, made with midicsv2ftmexport, to Famitracker's import/export format

Usage:

Put the program in your path. Then call the program passing the midi csv file via standard input with redirection

Examples:
(with input redirection)
midicsv2ftmexport < test.csv

OR (using 'cat' on linux)
cat test.csv | midicsv2ftmexport

OR (using 'type' on windows)
type test.csv | midicsv2ftmexport

The output will be written to the standard output. To write it to a file, use output redirection:

Examples:
(with input and output redirection)
midicsv2ftmexport < test.csv > test.ftm.txt

OR (using cat/type)
cat test.csv | midicsv2ftmexport > test.ftm.txt

To make a midi to famitracker direct conversion, use all three programs in a batch:

mididump test.midi | mididump2csv | midicsv2ftmexport > test.ftm.txt
