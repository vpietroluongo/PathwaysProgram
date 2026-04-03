using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Markup;
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

            //Activity 1
            Console.WriteLine("*** Activity 1 ***");
            var allContacts = context.Contacts.Include(c => c.Addresses).ToList();
            foreach (var contact in allContacts)
            {
                Console.WriteLine($"- {contact.Name} has {contact.Addresses.Count} addresses");
            }

            //Activity 2
            Console.WriteLine("*** Activity 2 ***");
            var springfieldContacts = context.Contacts
                .Where(c => c.Addresses.Any(allContacts => allContacts.City == "Springfield"))
                .Select(c => c.Name)
                .ToList();
            foreach(var name in springfieldContacts)
            {
                Console.WriteLine($"- {name}");
            }

            //Activity 3
            Console.WriteLine("*** Activity 3 ***");
            var cities = context.Addresses
                .GroupBy(a => a.City)
                .Select(g => new
                {
                    City = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            foreach (var item in cities)
            {
                Console.WriteLine($"{item.City}: {item.Count} addresses ");
            }

            //Activity 4
            Console.WriteLine("*** Activity 4 ***");
            var multiAddressContacts = context.Contacts
                .Where(c => c.Addresses.Count() > 1)
                .Select(c => c.Name)
                .ToList();
            foreach (var name in multiAddressContacts)
            {
                Console.WriteLine($"- {name}");
            }

            //Activity 5
            Console.WriteLine("*** Activity 5 ***");
            var henry = context.Contacts
                .FirstOrDefault(c => c.Name == "Henry Brown");
            if(henry != null)
            {
                Console.WriteLine($"Name: {henry.Name}");
                Console.WriteLine("Addresses:");
                foreach(var addr in henry.Addresses)
                {
                    Console.WriteLine($"    * {addr.Street}, {addr.City}");
                }
            }
            else
            {
                Console.WriteLine("Henry Brown not found.");
            }

            //Activity 6
            Console.WriteLine("*** Activity 6 ***");
            var totalContacts = context.Contacts.Count();
            var totalAddresses = context.Addresses.Count();
            Console.WriteLine($"{totalContacts} total contacts, {totalAddresses} total addresses");

            // var totalContacts = context.Contacts
            //     .Select(c => $"{c.Name.Count()} total contacts, {c.Addresses.Count()} total addresses")
            //     .ToList();
            // Console.WriteLine(totalContacts);

            //Activity 7
            Console.WriteLine("*** Activity 7 ***");
            var shelbyvilleContacts = context.Contacts
                .Where(c => c.Addresses.Any(a => a.City == "Shelbyville"))
                .Select(c => c.Name)
                .ToList();
            foreach (var person in shelbyvilleContacts)
            {
                Console.WriteLine($"- {person}");
            }

            //Activity 8
            var sortedAddresses = context.Addresses
                .OrderBy(a => a.City)
                .ThenBy(a => a.Street)
                .ToList();
            foreach (var address in sortedAddresses)
            {
                Console.WriteLine($"- City: {address.City}, Street: {address.Street}");
            } 

            //Activity 9
            var hasMostAddresses = context.Contacts 
                //.Include(c => c.Addresses)
                .OrderByDescending(c => c.Addresses.Count())
                .Select(c => c.Name)
                .First();
            Console.WriteLine($"{hasMostAddresses} has the most addresses.");

            var contactWithMost = context.Contacts
                .Include(c => c.Addresses)
                .OrderByDescending(c => c.Addresses.Count())
                .FirstOrDefault();
            if (contactWithMost != null)
            {
                Console.WriteLine($"{contactWithMost.Name} has the most with {contactWithMost.Addresses.Count()} addresses.");
            }

            //Activity 10
            var notInSpringfield = context.Contacts
                .Where(c => c.Addresses.All(a => a.City != "Springfield"))
                .Select(c => c.Name)
                .ToList();
            foreach (var person in notInSpringfield)
            {
                Console.WriteLine($"- {person}");
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
