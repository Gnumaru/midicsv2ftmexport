using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace midicsv2ftmexport {
    class MainClass {
        /// precisa levar em consideração mudança de pattern
        /// vai resetar o numero da row
        /// 
        private static int patternCount;
        static int quarterNoteBPM = 120;
        static long firstNoteTime = int.MaxValue / 2;
        static int currentPattern;
        static int currentRow;

        public static int getFirstNoteIndex(List<string[]> lines) {
            for(int i = 0; i < lines.Count; i++) {
                if(lines[i][1] == "NoteOn") return i;
            }
            return -1;
        }

        public static bool isNoteOn(string[] parts) {
            return parts.Length > 5 && parts[1] == "NoteOn" && parts[5] != "(NoteOff)";
        }

        public static void Main(string[] args) {
            string line = string.Empty;
            List<string[]> lines = new List<string[]>();

            if(args.Length < 1) while((line = Console.ReadLine()) != null) lines.Add(line.Split(','));
            else {
                foreach(var l in File.ReadAllLines(args[0])) {
                    lines.Add(l.Split(','));
                }
            }
            //Console.WriteLine(args[0]);Console.ReadLine();return;
            quarterNoteBPM = int.Parse(lines[2][2].Replace("bpm", string.Empty));
            int firstNoteLineID = getFirstNoteIndex(lines);
            // skip the header and go directly to the notes
            var linesWithNotes = lines.Skip(firstNoteLineID).ToList();
            //Console.WriteLine(lines[0][1]);
            //Console.ReadLine();
            //return;
            var sb = new StringBuilder();
            sb.Append(header);
            var linesID = 0;
            int failsafe = 0;
            //Console.WriteLine(args[0]);Console.ReadLine();
            for(int loopID = 0; linesID < linesWithNotes.Count; loopID++) {
                failsafe++; if(failsafe > 100) break;
                if(loopID == 64) {
                    loopID = 0;
                    patternCount++;
                    sb.Append(patternTitle.Replace("XX", patternCount.ToString("X").PadLeft(2, '0')));
                }

                var parts = linesWithNotes[linesID];
                int lineFTRow = ftRowInt(parts);
                //Console.WriteLine($"loopID:{loopID} linesID:{linesID} lineFTRow:{lineFTRow}");

                if(lineFTRow != loopID) { // fake row
                    sb.AppendLine(emptyRow.Replace("XX", loopID.ToString("X").PadLeft(2, '0')));
                } else { // real row
                    if(isNoteOn(parts)) sb.AppendLine($"ROW {ftRowStr(parts)} : {ftNote(parts)} {ftInstStr(parts)} {ftVolStr(parts)} {ftEffect(parts)} : ... .. . ... : ... .. . ... : ... .. . ... : ... .. . ...");
                    else sb.AppendLine(emptyRow.Replace("XX", loopID.ToString("X").PadLeft(2, '0')));
                    linesID++;
                }
            }
            sb.Append(footer);
            Console.WriteLine(sb);
            Console.ReadLine();
        }

        private static string maxMuteDelayPerRowStr(int rowsCount) => "S" + maxMuteDelayPerRowInt(rowsCount).ToString("X").PadLeft(2, '0');
        private static int maxMuteDelayPerRowInt(int rowsCount) => rowsCount * 6 - 1;



        private static string ftRowStr(string[] e) => ftRowInt(e).ToString("X").PadLeft(2, '0');
        private static int ftRowInt(string[] e) => (int.Parse(e[0]) / quarterNoteBPM);//- (64 * patternCount);


        private static string ftNote(string[] e) {
            var s = e[3];
            if(s.Length == 3) return s;
            return s.Insert(1, "-");
        }

        private static string ftInstStr(string[] e) => ftInstInt(e).ToString("X").PadLeft(2, '0');
        private static int ftInstInt(string[] e) => 0;

        private static string ftVolStr(string[] e) => ftVolInt(e).ToString("X");
        private static int ftVolInt(string[] e) => int.Parse(e[4].Split(':')[1]) / 8;

        private static string ftEffect(string[] e) => maxMuteDelayPerRowStr(1);


        const string header =
@"# FamiTracker text export 0.4.2

# Song information
TITLE           """"
AUTHOR          """"
COPYRIGHT       """"

# Song comment
COMMENT """"

# Global settings
MACHINE         0
FRAMERATE       0
EXPANSION       0
VIBRATO         1
SPLIT           32

# Macros

# DPCM samples

# Instruments
INST2A03   0    -1  -1  -1  -1  -1 ""New instrument""

# Tracks

TRACK  64   6 150 ""New song""
COLUMNS : 1 1 1 1 1

ORDER 00 : 00 00 00 00 00

PATTERN 00
";
        const string footer = "\r\n# End of export\r\n";
        const string emptyRow = "ROW XX : ... .. . ... : ... .. . ... : ... .. . ... : ... .. . ... : ... .. . ...";
        const string patternTitle = "\r\nPATTERN XX\r\n";
    }
}

/*

7680 NoteOn Ch: 1 C6 Vel:80 Len: 1823
9503 NoteOn Ch: 1 C6 Vel:0 (Note Off)
ROW 00 : C-4 00 F ... : ... .. . ... : ... .. . ... : ... .. . ... : ... .. . ...
ROW XX : ... .. . ... : ... .. . ... : ... .. . ... : ... .. . ... : ... .. . ...

\r\n# End of export\r\n
*/
