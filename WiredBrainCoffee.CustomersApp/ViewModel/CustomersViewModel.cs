using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WiredBrainCoffee.CustomersApp.Data;
using WiredBrainCoffee.CustomersApp.Model;

namespace WiredBrainCoffee.CustomersApp.ViewModel {

    public class CustomersViewModel : ViewModelBase {
        private readonly ICustomerDataProvider _customerDataProvider;
        private CustomerItemViewModel? _selectedCustomer;
        private NavigationSide _navigationColumn;

        public CustomersViewModel(ICustomerDataProvider customerDataProvider) {
            _customerDataProvider = customerDataProvider;
        }

        public ObservableCollection<CustomerItemViewModel> Customers { get; } = new();

        public CustomerItemViewModel? SelectedCustomer { 
            get => _selectedCustomer;
            set {
                _selectedCustomer = value;
                RaisePropertyChanged();
            } 
        }

        public NavigationSide NavigationColumn { 
            get => _navigationColumn; 
            private set { 
                _navigationColumn= value; 
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync() {
            if (Customers.Any()) {
                return;
            }

            var customers = await _customerDataProvider.GetAllAsync();
            if (customers != null) {
                foreach (var customer in customers) {
                    Customers.Add(new CustomerItemViewModel(customer));
                } 
            }
        }

        internal void Add() {
            var newCustomer = new Customer { FirstName = "New" };
            var newCustomerViewModel = new CustomerItemViewModel(newCustomer);
            Customers.Add(newCustomerViewModel);
            SelectedCustomer = newCustomerViewModel;
        }

        internal void MoveNavigation() {
            NavigationColumn = NavigationColumn == NavigationSide.Left 
                ? NavigationSide.Right 
                : NavigationSide.Left;
        }
    }

    public enum NavigationSide {
        Left,
        Right
    }
}
