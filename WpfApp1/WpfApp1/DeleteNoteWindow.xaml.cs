using Dapper;
using System;
using System.Data.SQLite;
using System.Windows;

namespace WpfApp1
{
    public partial class DeleteNoteWindow : Window
    {
        private string connectionString;

        private Note noteToDelete;

        public DeleteNoteWindow(Note note)
        {
            InitializeComponent();
            noteToDelete = note;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            connectionString = $"Data Source=NotesDatabase.sqlite;Version=3;";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (noteToDelete != null)
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Execute("DELETE FROM Notes WHERE ID = @ID", new { ID = noteToDelete.ID });
                }
                MessageBox.Show("Note deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                OnNoteDeleted();
                Close();
            }
            else
            {
                MessageBox.Show("No note selected to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected virtual void OnNoteDeleted()
        {
            NoteDeleted?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler NoteDeleted;
    }
}
