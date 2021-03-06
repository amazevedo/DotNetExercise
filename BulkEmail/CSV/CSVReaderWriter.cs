﻿using System;
using System.IO;

namespace BulkEmail.CSV
{
    /*
     2) Refactor the CSVReaderWriter implementation into clean, elegant, well performing 
        and maintainable code, as you see fit.
        You should not update the BulkEmailProcessor as part of this task.
        Backwards compatibility of the CSVReaderWriter must be maintained, so that the 
        existing BulkEmailProcessor is not broken.
        Other that that, you can make any change you see fit, even to the code structure.
    */

    public class CSVReaderWriter
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode
        {
            Read = 1,
            Write = 2
        };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string[] columns;

            bool result = Read(out columns);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string[] columns)
        {
            string line;

            char separator = '\t';

            line = ReadLine();

            if (line == null)
            {
                columns = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                columns = null;

                return false;
            }
            else
            {
                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
