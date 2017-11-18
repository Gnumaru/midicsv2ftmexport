# midicsv2ftmexport
Convert a midi csv file, made with midicsv2ftmexport, to Famitracker's import/export format

# Disclaimer
The midi protocol and Famitrackers persistent representation (files) of famicom executable music format are two very different things. That said, they do share something, because they are, at theyr very core, the representation of sucession of events in time. What are those events, what they represent, may differ greatly in midi and Famitracker, but in essence they are the succession of events in time, like any temporal media.

The purpose of midicsv2ftmexport is to make easier for musicians already accustomed and skilled in writing sheet music in software like Musescore, Sibelius, Finale etc to 'port' their 'tunes' to Famitracker. It is not to 'convert' the midi to Famitracker the best way possible, but just to convert 'tunes' that will be worked on solely on Famitracker later.

I made this software for my personal use because I'm used to writing sheet music with sheet editing software and when I started trying to use Famitracker, it seemed to slow for me, an untrained guy, to write a simple melody that I could write very quickly in Musescore.

# Pre Requisites:
* Your midi track must have __ONLY ONE__ channel (one staff of sheet music).
* Your midi track must be __MONOFONIC__ (no more than one note playing at the same time)
* Your midi track must use __NORMAL INSTRUMENTS__ (no percussion).
* Your midi track must have some note __AT THE VERY START__ of the track. You can delete it later in Famitracker.

# Usage:

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
