using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ImLazy.Addins.Annotations;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.FileTypeSubject")]
    public class FileTypeSubject : ISubject
    {
        private const string CanonicalNamePrefix = "ImLazy.Addins.Lexer.Subjects";

        public static readonly LexerType FileType = new LexerType(LexerTypes.TypeType, "FileType", CanonicalNamePrefix);

        private static readonly LexerType File = new LexerType(null, "AnyFile", CanonicalNamePrefix);
        private static readonly LexerType Archieve = new LexerType(null, "Archieve", CanonicalNamePrefix);
        private static readonly LexerType Folder = new LexerType(null, "Folder", CanonicalNamePrefix);
        private static readonly LexerType Document = new LexerType(File, "Document", CanonicalNamePrefix);
        private static readonly LexerType Audio = new LexerType(File, "Audio", CanonicalNamePrefix);
        private static readonly LexerType Video = new LexerType(File, "Video", CanonicalNamePrefix);
        private static readonly LexerType Picture = new LexerType(File, "Picture", CanonicalNamePrefix);
        private static readonly LexerType GuitarProTab = new LexerType(File, "GuitarProTab", CanonicalNamePrefix);
        private static readonly LexerType LoveAction = new LexerType(null, "LoveAction", CanonicalNamePrefix);

        public static readonly IEnumerable<LexerType> SupportedTypes = new List<LexerType>
        {
            File, Archieve, Folder, Document, Audio, Video, Picture, GuitarProTab, LoveAction
        };

        private static readonly Regex LoveActionRegex = new Regex(@"((gachi)|(pgd)|(mxgs)|(siro)|(sw)|(s-cute)|(yrz)|(sero)|(abp))\w*\W?\d", RegexOptions.IgnoreCase);

        private static readonly Dictionary<Func<string, string, bool>, LexerType> Conditions = new Dictionary<Func<string, string, bool>, LexerType>
        {
            { (name, ext) => Split("第一会所|第一會所|女優|女优|av|草榴|sex|逼|奸|漂亮").Any(name.Contains), LoveAction },
            { (name, ext) => name != null && LoveActionRegex.IsMatch(name), LoveAction },
            { (name, ext) => Matches(ext, "rar|zip|7z"), Archieve },
            { (name, ext) => Matches(ext, "txt|doc|docx|chm|pdf|rtf"), Document },
            { (name, ext) => Matches(ext, "mp3|aac|flac|wav|ape|ogg"), Audio },
            { (name, ext) => Matches(ext, "avi|mp4|mpg|mkv|wmv|rm|rmvb|flv"), Video },
            { (name, ext) => Matches(ext, "jpg|jpeg|bmp|png|gif|tiff|raw"), Picture },
            { (name, ext) => Matches(ext, "gp3|gp4|gp5|gtp|gpx"), GuitarProTab },
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listInString">多个后缀，用|分隔，不包含. 如"txt,doc,docx"</param>
        /// <returns></returns>
        private static bool Matches(string source, string listInString)
        {
            if (String.IsNullOrEmpty(source)) return false;
            while(source[0] == '.')
                source = source.Remove(0, 1);
            var list = listInString.Split('|');
            return list.Any(_ => _.Equals(source, StringComparison.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<string> Split(string source)
        {
            return source.Split('|');
        } 
        public string Name { get { return "FileTypeSubject"; } }
        public LexerType ElementType { get; [UsedImplicitly] private set; }

        public LexerType GetVerbType()
        {
            return FileType;
        }

        public object GetProperty(string filePath)
        {
            LexerType type = null;
            if (Directory.Exists(filePath))
                type = Folder;
            if (System.IO.File.Exists(filePath))
                type = File;

            var extension = Path.GetExtension(filePath);
            var shortName = Path.GetFileNameWithoutExtension(filePath);

            foreach (var condition in Conditions.Where(condition => condition.Key(shortName, extension)))
            {
                type = condition.Value;
                break;
            }
            
            return type;
        }
    }
}
