using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string databaseFile = "NotesDatabase.sqlite";
        private string connectionString;
        private int currentNoteID = 1; // Przeniesienie zmiennej currentNoteID do klasy MainWindow

        public ObservableCollection<Note> Notes { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            Notes = new ObservableCollection<Note>();
            DataContext = this;
            LoadNotes();
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
                    connection.Execute(@"CREATE TABLE IF NOT EXISTS Notes (
                                            ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                                            Title TEXT, 
                                            Content TEXT, 
                                            Category TEXT, 
                                            CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP, 
                                            ModificationDate DATETIME
                                        )");
                }
            }
            else
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    var tableInfo = connection.QuerySingleOrDefault<string>("SELECT sql FROM sqlite_master WHERE type='table' AND name='Notes'");
                    if (!tableInfo.Contains("ID"))
                    {
                        connection.Execute("ALTER TABLE Notes ADD COLUMN ID INTEGER PRIMARY KEY AUTOINCREMENT");
                    }
                }
            }
        }

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var addNoteWindow = new AddNoteWindow(connectionString);
            addNoteWindow.NoteDataAdded += AddNoteWindow_NoteDataAdded; // Dodaj przypisanie obsługi zdarzenia
            addNoteWindow.ShowDialog();
        }



        private void AddNoteWindow_NoteDataAdded(object sender, NoteDataEventArgs e)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                var lastID = connection.QueryFirstOrDefault<int?>("SELECT MAX(ID) FROM Notes");

                // Increment currentNoteID only if lastID is not null
                if (lastID != null)
                {
                    currentNoteID = lastID.Value + 1;
                }

                // Assign unique ID to the new note
                e.ID = currentNoteID;

                connection.Execute("INSERT INTO Notes (ID, Title, Content, Category) VALUES (@ID, @Title, @Content, @Category)", e);
            }

            // Add the note to the Notes collection
            Notes.Add(e.ToNote());

            // Refresh the view to reflect the changes
            LoadNotes();
        }
        private void EditNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesDataGrid.SelectedItem != null)
            {
                var selectedNote = NotesDataGrid.SelectedItem as Note;
                var editNoteWindow = new EditNoteWindow(selectedNote);
                editNoteWindow.NoteDataUpdated += EditNoteWindow_NoteDataUpdated;
                editNoteWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a note to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditNoteWindow_NoteDataUpdated(object sender, NoteDataEventArgs e)
        {
            var updatedNote = Notes.FirstOrDefault(n => n.ID == e.ID);
            if (updatedNote != null)
            {
                updatedNote.Title = e.Title;
                updatedNote.Content = e.Content;
                updatedNote.Category = e.Category;
                updatedNote.ModificationDate = DateTime.Now;
            }

            LoadNotes(); // Odśwież listę notatek
        }

        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesDataGrid.SelectedItem != null)
            {
                var selectedNote = NotesDataGrid.SelectedItem as Note;
                var deleteNoteWindow = new DeleteNoteWindow(selectedNote);
                deleteNoteWindow.NoteDeleted += DeleteNoteWindow_NoteDeleted;
                deleteNoteWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a note to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteNoteWindow_NoteDeleted(object sender, EventArgs e)
        {
            var deletedNote = NotesDataGrid.SelectedItem as Note;
            Notes.Remove(deletedNote);
        }

        private void ClearAllNotesButton_Click(object sender, RoutedEventArgs e)
        {
            ClearNotesTable();
            LoadNotes(); // Refresh the list of notes
        }

        private void ClearNotesTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Execute("DELETE FROM Notes");
            }
        }

        private void LoadNotes()
        {
            Notes.Clear();
            using (var connection = new SQLiteConnection(connectionString))
            {
                var notes = connection.Query<Note>("SELECT * FROM Notes WHERE ID <> 0"); // Wybierz notatki, których ID nie jest równe 0
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
