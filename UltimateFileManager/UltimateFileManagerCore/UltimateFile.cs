﻿using System;
using System.Collections.Generic;
using System.IO;

namespace UltimateFileManagerCore
{
    public static class UltimateFile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="newNames"></param>
        /// <returns></returns>
        public static List<string> RenameFiles(this IEnumerable<string> files, List<string> newNames)
        {
            if (newNames == null)
            {
                throw new ArgumentNullException(nameof(newNames));
            }
            if (((ICollection<string>)files).Count != newNames.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(newNames), "The given names are less that the total of the files");
            }
            int i = 0;
            List<string> newFiles = new List<string>();
            foreach (string file in files)
            {
                newFiles.Add(Path.Combine(Path.GetDirectoryName(file), newNames[i]));
                i++;
            }
            return newFiles;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="newExtension"></param>
        /// <returns></returns>
        public static List<string> ChangeExtension(this IEnumerable<string> files, string newExtension)
        {
            if (string.IsNullOrEmpty(newExtension))
            {
                throw new ArgumentNullException(nameof(newExtension));
            }
            List<string> newFiles = new List<string>();
            foreach (string file in files)
            {
                newFiles.Add(Path.ChangeExtension(file, newExtension));
            }
            return newFiles;
        }
    }
}