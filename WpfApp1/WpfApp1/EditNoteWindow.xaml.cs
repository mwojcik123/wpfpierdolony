using Dapper;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace WpfApp1
{
    public partial class EditNoteWindow : Window
    {
        private string databaseFile = "NotesDatabase.sqlite";
        private string connectionString;
        private Note note;

        public event EventHandler<NoteDataEventArgs> NoteDataUpdated;

        public EditNoteWindow(Note note)
        {
            InitializeComponent();
            InitializeDatabase();
            this.note = note;
            TitleTextBox.Text = note.Title;
            ContentTextBox.Text = note.Content;
            CategoryTextBox.Text = note.Category;
        }

        private void InitializeDatabase()
        {
            connectionString = $"Data Source={databaseFile};Version=3;";
            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute("CREATE TABLE IF NOT EXISTS Notes (ID INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, Content TEXT, Category TEXT, CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP, ModificationDate DATETIME)");
                }
            }
            else
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    var tableInfo = connection.QuerySingleOrDefault<string>("SELECT sql FROM sqlite_master WHERE type='table' AND name='Notes'");
                    if (!tableInfo.Contains("ModificationDate"))
                    {
                        connection.Execute("ALTER TABLE Notes ADD COLUMN ModificationDate DATETIME");
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;
            string category = CategoryTextBox.Text;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Execute("UPDATE Notes SET Title = @Title, Category = @Category, Content = @Content, ModificationDate = CURRENT_TIMESTAMP WHERE ID = @ID", new { Title = title, Category = category, Content = content, ID = note.ID });
            }

            NoteDataEventArgs args = new NoteDataEventArgs(note.ID, title, category, content, note.Title);
            OnNoteDataUpdated(args);

            Close();
        }

        protected virtual void OnNoteDataUpdated(NoteDataEventArgs e)
        {
            NoteDataUpdated?.Invoke(this, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
