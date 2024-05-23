using Dapper;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace WpfApp1
{
    public partial class AddNoteWindow : Window
    {
        private string connectionString;

        // Deklaracja zdarzenia NoteDataAdded
        public event EventHandler<NoteDataEventArgs> NoteDataAdded;

        public AddNoteWindow(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
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

            DateTime currentDateTime = DateTime.Now; // Aktualna data i godzina

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                int id = connection.ExecuteScalar<int>(
                    "INSERT INTO Notes (Title, Category, Content, CreationDate, ModificationDate) " +
                    "VALUES (@Title, @Category, @Content, @CreationDate, @ModificationDate);" +
                    "SELECT last_insert_rowid();",
                    new
                    {
                        Title = title,
                        Category = category,
                        Content = content,
                        CreationDate = currentDateTime,
                        ModificationDate = currentDateTime
                    });

                if (id > 0)
                {
                    // Jeśli notatka została dodana pomyślnie, wywołaj zdarzenie NoteDataAdded
                    OnNoteDataAdded(new NoteDataEventArgs(id, title, category, content)); // Przekazanie daty utworzenia do zdarzenia
                    Close();
                }
                else
                {
                    MessageBox.Show("An error occurred while adding the note. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Metoda wywołująca zdarzenie NoteDataAdded
        protected virtual void OnNoteDataAdded(NoteDataEventArgs e)
        {
            NoteDataAdded?.Invoke(this, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
