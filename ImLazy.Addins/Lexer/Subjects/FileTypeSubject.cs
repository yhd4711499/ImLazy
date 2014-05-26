using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.FileTypeSubject")]
    public class FileTypeSubject : ISubject
    {
        private const string CanonicalNamePrefix = "ImLazy.Addins.Lexer.Subjects";

        public static readonly LexerType FileType = new LexerType(LexerTypes.TypeType, "FileType", CanonicalNamePrefix);

        static readonly LexerType File = new LexerType(null, "AnyFile", CanonicalNamePrefix);
        static readonly LexerType Archieve = new LexerType(null, "Archieve", CanonicalNamePrefix);
        static readonly LexerType Folder = new LexerType(null, "Folder", CanonicalNamePrefix);
        static readonly LexerType Document = new LexerType(File, "Document", CanonicalNamePrefix);
        static readonly LexerType Audio = new LexerType(File, "Audio", CanonicalNamePrefix);
        static readonly LexerType Video = new LexerType(File, "Video", CanonicalNamePrefix);
        static readonly LexerType Picture = new LexerType(File, "Picture", CanonicalNamePrefix);
        static readonly LexerType GuitarProTab = new LexerType(File, "GuitarProTab", CanonicalNamePrefix);

        static readonly LexerType LoveAction = new LexerType(null, "LoveAction", CanonicalNamePrefix);

        public static readonly IEnumerable<LexerType> SupportedTypes = new List<LexerType>
        {
            File, Archieve, Folder, Document, Audio, Video, Picture, GuitarProTab, LoveAction
        };

        private static readonly Regex LoveActionRegex = new Regex(@"((gachi)|(pgd)|(mxgs)|(siro)|(sw))\w*\W?\d");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listInString">多个后缀，用,分隔，不包含. 如"txt,doc,docx"</param>
        /// <returns></returns>
        private static bool InList(string source, string listInString)
        {
            var list = listInString.Split('|');
            return list.Any(_ => _.Equals(source, StringComparison.InvariantCultureIgnoreCase));
        }
        public string Name { get { return "FileTypeSubject"; } }
        public LexerType ElementType { get; private set; }

        public LexerType GetVerbType()
        {
            return FileType;
        }

        public object GetProperty(string filePath)
        {
            LexerType type = null;
            do
            {
                if (Directory.Exists(filePath))
                    type = Folder;

                if (System.IO.File.Exists(filePath))
                    type = File;

                var extension = Path.GetExtension(filePath);
                if (!String.IsNullOrEmpty(extension))
                {
                    var shortExt = extension.Substring(1);
                    if (InList(shortExt, "rar|zip|7z"))
                    {
                        type = Archieve;
                        break;
                    }
                    if (InList(shortExt, "txt|doc|docx|chm|pdf|rtf"))
                    {
                        type = Document;
                        break;
                    }
                    if (InList(shortExt, "mp3|aac|flac|wav|ape|ogg"))
                    {
                        type = Audio;
                        break;
                    }
                    if (InList(shortExt, "avi|mp4|mpg|mkv|wmv|rm|rmvb|flv"))
                    {
                        type = Video;
                        break;
                    }
                    if (InList(shortExt, "jpg|jpeg|bmp|png|gif|tiff|raw"))
                    {
                        type = Picture;
                        break;
                    }
                    if (InList(shortExt, "gp3|gp4|gp5|gtp|gpx"))
                    {
                        type = GuitarProTab;
                        break;
                    }
                }
                var name = Path.GetFileNameWithoutExtension(filePath);
                if (String.IsNullOrEmpty(name)) break;
                name = name.ToLower();
                if (InList(name, "第一会所|第一會所|女優|女优|av|草榴|sex"))
                {
                    type = LoveAction;
                    break;
                }

                if (LoveActionRegex.IsMatch(name))
                {
                    type = LoveAction;
                    break;
                }
            } while (false);
            
            return type;
        }
    }
}
