using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFDemo
{
    // Entity: Contact
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Address> Addresses { get; set; } = new();
    }

    // Entity: Address
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int ContactId { get; set; }
        public Contact? Contact { get; set; }
    }

    // DbContext
    public class ContactsContext : DbContext
    {
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Address> Addresses => Set<Address>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ContactsDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Contact)
                .HasForeignKey(a => a.ContactId);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using var context = new ContactsContext();
            SeedData(context);

            var allContacts = context.Contacts
                .Include(c => c.Addresses)
                .ToList();
            foreach(var person in allContacts)
            {
                Console.WriteLine($"{person.Name} {person.Addresses.Count}");
            }


            var springfieldContacts = context.Contacts
                .Where(c => c.Addresses.Any(a => a.City == "Springfield"))
                .Select(c => c.Name)
                .ToList();
            foreach (var person in springfieldContacts)
            {
                Console.WriteLine($"- {person}");
            }

            var cities = context.Addresses
                .GroupBy(a => a.City)
                .Select(g => new
                {
                    City = g.Key,
                    Count = g.Count()
                })
                .ToList();
            foreach (var city in cities)
            {
                Console.WriteLine($"{city.City} {city.Count}");
            }

            var moreThanOne = context.Contacts
                .Where(c => c.Addresses.Count() > 1)
                .Select(c => c.Name)
                .ToList();
            foreach(var person in moreThanOne)
            {
                Console.WriteLine($"- {person}");
            }


            var henryInfo = context.Contacts
                .Include(c => c.Addresses)
                .FirstOrDefault(c => c.Name == "Henry Brown");
            if (henryInfo != null)
            {
                Console.WriteLine($"{henryInfo.Name}'s addresses");
                foreach (var address in henryInfo.Addresses)
                {
                    Console.WriteLine($"{address.Street}, {address.City}");
                }
            }
        }

        static void SeedData(ContactsContext context)
        {
            if (context.Contacts.Any()) return;

            var contacts = new List<Contact>
            {
                new Contact
                {
                    Name = "Alice Smith",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "123 Main St", City = "Springfield" },
                        new Address { Street = "456 Oak Ave", City = "Shelbyville" }
                    }
                },
                new Contact
                {
                    Name = "Bob Johnson",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "789 Pine Rd", City = "Springfield" }
                    }
                },
                new Contact
                {
                    Name = "Carol White",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "101 Maple St", City = "Ogdenville" }
                    }
                },
                new Contact
                {
                    Name = "David Martinez",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "234 Elm St", City = "Springfield" },
                        new Address { Street = "567 Cedar Ln", City = "Capital City" }
                    }
                },
                new Contact
                {
                    Name = "Emma Davis",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "890 Birch Ave", City = "Shelbyville" }
                    }
                },
                new Contact
                {
                    Name = "Frank Wilson",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "321 Willow Dr", City = "Ogdenville" },
                        new Address { Street = "654 Spruce Way", City = "Springfield" }
                    }
                },
                new Contact
                {
                    Name = "Grace Lee",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "987 Aspen Ct", City = "Capital City" }
                    }
                },
                new Contact
                {
                    Name = "Henry Brown",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "147 Poplar St", City = "Springfield" },
                        new Address { Street = "258 Cherry Blvd", City = "Shelbyville" },
                        new Address { Street = "369 Walnut Rd", City = "Ogdenville" }
                    }
                },
                new Contact
                {
                    Name = "Iris Chen",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "741 Magnolia Ave", City = "Capital City" }
                    }
                },
                new Contact
                {
                    Name = "Jack Thompson",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "852 Sycamore Ln", City = "Springfield" }
                    }
                },
                new Contact
                {
                    Name = "Kelly Anderson",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "963 Hickory Dr", City = "Shelbyville" },
                        new Address { Street = "159 Redwood Way", City = "Capital City" }
                    }
                },
                new Contact
                {
                    Name = "Liam Garcia",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "753 Cypress Ct", City = "Ogdenville" }
                    }
                },
                new Contact
                {
                    Name = "Maya Patel",
                    Addresses = new List<Address>
                    {
                        new Address { Street = "486 Beech St", City = "Springfield" },
                        new Address { Street = "297 Dogwood Ave", City = "Shelbyville" }
                    }
                }
            };

            context.Contacts.AddRange(contacts);
            context.SaveChanges();
        }
    }
}