﻿namespace ShopMVC.Models
{
    public class Customer
    {
        
        public int CustomerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public DateTime MembershipDate { get; set; }
        public ICollection<RentalHeader>? RentalHeaders { get; set; }
    }
}
