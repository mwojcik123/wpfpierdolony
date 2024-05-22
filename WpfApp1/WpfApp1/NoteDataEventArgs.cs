using System;

namespace WpfApp1
{
    public class NoteDataEventArgs : EventArgs
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public string OldTitle { get; set; }

        public NoteDataEventArgs(int id, string title, string category, string content)
        {
            ID = id;
            Title = title;
            Category = category;
            Content = content;

        }

        public Note ToNote()
        {
            return new Note
            {
                ID = ID,
                Title = Title,
                Category = Category,
                Content = Content
            };
        }
    }
}