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
    class CustomerViewModel : INotifyPropertyChanged
    {
        //save and load customers
        private string? _firstName;
        public string? FirstName
        {
            get => _firstName;
            set 
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string? _lastName;
        public string? LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private DateOnly? _membershipDate;
        public DateOnly? MembershipDate
        {
            get => _membershipDate;
            set
            {
                if(_membershipDate == value) return;
                _membershipDate = value;
                OnPropertyChanged(nameof(MembershipDate));
            }
        }

        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();
        public ICommand SaveCustomerCommand { get; }
        public CustomerViewModel()
        {
            SaveCustomerCommand = new RelayCommand(SaveCustomer);
            LoadCustomers();
        }
        private void SaveCustomer()
        {
            using var context = new RentalDbContext();
            var newCustomer = new Customer
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                MembershipDate = this.MembershipDate
            };
            context.Customers.Add(newCustomer);
            context.SaveChanges();
            Customers.Add(newCustomer);
        }
        public void LoadCustomers()
        {
            using var context = new RentalDbContext();
            Customers.Clear();
            foreach(var customer in context.Customers)
            {
                Customers.Add(customer);
            }
        }

        //search customers
        private string _searchFirstName;
        public string SearchFirstName
        {
            get => _searchFirstName;
            set
            {
                if(_searchFirstName != value)
                {
                    _searchFirstName = value;
                    OnPropertyChanged(nameof(SearchFirstName));
                    SearchCustomers();
                }
            }
        }
        private string _searchLastName;
        public string SearchLastName
        {
            get => _searchLastName;
            set
            {
                if(_searchLastName != value)
                {
                    _searchLastName = value;
                    OnPropertyChanged(nameof(SearchLastName));
                    SearchCustomers();
                }
            }
        }
        private string _searchMembershipDate;
        public string SearchMembershipDate
        {
            get => _searchMembershipDate;
            set
            {
                _searchMembershipDate = value;
                OnPropertyChanged(nameof(SearchMembershipDate));
                SearchCustomers();
            }
        }

        public ObservableCollection<Customer> SearchResults { get; set; } = new ObservableCollection<Customer>();
        private void SearchCustomers()
        {
            using var context = new RentalDbContext();
            var query = context.Customers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchFirstName))
            {
                query = query.Where(customer => customer.FirstName.Contains(SearchFirstName));
            }
            if (!string.IsNullOrWhiteSpace(SearchLastName))
            {
                query = query.Where(customer => customer.LastName.Contains(SearchLastName));
            }
            //date if
            var results = query.ToList();
            SearchResults.Clear();
            foreach (var customer in results)
            {
                SearchResults.Add(customer);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
