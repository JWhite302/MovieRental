using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualBasic;
using MovieRental.Commands;
using MovieRental.Models;
using MovieRental.ViewModels;

namespace MovieRental.ViewModels
{
    class InventoryViewModel : INotifyPropertyChanged
    { 
        //save and load movies
        private string? _title;
        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string? _genre;
        public string? Genre
        {
            get => _genre;
            set
            {
                _genre = value;
                OnPropertyChanged(nameof(Genre));
            }
        }

        private int _releaseYear;
        public int ReleaseYear
        {
            get => _releaseYear;
            set
            {
                if(_releaseYear == value) return;
                _releaseYear = value;
                OnPropertyChanged(nameof(ReleaseYear));
            }
        }

        public ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();
        public ICommand SaveMovieCommand { get; }
        public InventoryViewModel()
        {
            SaveMovieCommand = new RelayCommand(SaveMovie);
            LoadMovies();
        }
        private void SaveMovie()
        {
            using var context = new RentalDbContext();
            var newMovie = new Movie
            {
                Title = this.Title,
                Genre = this.Genre,
                ReleaseYear = this.ReleaseYear
            };
            context.Movies.Add(newMovie);
            context.SaveChanges();
            Movies.Add(newMovie);
        }

        public void LoadMovies()
        {
            using var context = new RentalDbContext();
            Movies.Clear();
            foreach(var movie in context.Movies)
            {
                Movies.Add(movie);
            }
        }

        //search movies
        private string _searchTitle;
        public string SearchTitle
        {
            get => _searchTitle;
            set
            {
                if(_searchTitle != value)
                {
                    _searchTitle = value;
                    OnPropertyChanged(nameof(SearchTitle));
                    SearchMovies();
                }
            }
        }
        private string _searchGenre;
        public string SearchGenre
        {
            get => _searchGenre;
            set
            {
                if(_searchGenre != value)
                {
                    _searchGenre = value;
                    OnPropertyChanged(nameof(SearchGenre));
                    SearchMovies();
                }
            }
        }
        private string _searchReleaseYear;
        public string SearchReleaseYear
        {
            get => _searchReleaseYear;
            set
            {
                if (_searchReleaseYear != value)
                {
                    _searchReleaseYear = value;
                    OnPropertyChanged(nameof(SearchReleaseYear));
                    SearchMovies();
                }
            }
        }
        public ObservableCollection<Movie> SearchResults { get; set; } = new ObservableCollection<Movie>();
        private void SearchMovies()
        {
            using var context = new RentalDbContext();
            var query = context.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchTitle))
            {
                query = query.Where(movie => movie.Title.Contains(SearchTitle));
            }
            if (!string.IsNullOrWhiteSpace(SearchGenre))
            {
                query = query.Where(movie => movie.Genre.Contains(SearchGenre));
            }
            if (!string.IsNullOrWhiteSpace(SearchReleaseYear))
            {
                if(int.TryParse(SearchReleaseYear, out int year))
                {
                    query = query.Where(movie => movie.ReleaseYear == year);
                }
                else
                {
                    SearchResults.Clear();
                    return;
                }
            }

            var results = query.ToList();
            SearchResults.Clear();
            foreach (var movie in results)
            {
                SearchResults.Add(movie);
            }
        }
        
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
