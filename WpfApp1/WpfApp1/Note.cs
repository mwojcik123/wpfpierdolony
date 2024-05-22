using System;

namespace WpfApp1
{
    public class Note
    {
        public int ID { get; set; }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                //UpdateModificationDate();
            }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                //UpdateModificationDate();
            }
        }

        private string category;
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                //UpdateModificationDate();
            }
        }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public void UpdateModificationDate()
        {
            ModificationDate = DateTime.UtcNow;
        }
        public void CreateDataDate()
        {
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
        public string FormattedCreationDate
        {
            get { return CreationDate.ToString("dd/MM/yyyy HH:mm"); }
        }

        public string FormattedModificationDate
        {
            get { return ModificationDate.ToString("dd/MM/yyyy HH:mm"); }
        }
    }
}