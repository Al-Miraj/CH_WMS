using Microsoft.EntityFrameworkCore;
public class ModelContext : DbContext
{

    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentItem> ShipmentItems { get; set; }

    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<TransferItem> TransferItems { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Item> Items { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<ItemGroup> ItemGroups { get; set; }

    public DbSet<ItemLine> ItemLines { get; set; }

    public DbSet<ItemType> ItemTypes { get; set; }

    public DbSet<Inventory> Inventorys { get; set; }

    public DbSet<Client> Clients { get; set; }




    public ModelContext(DbContextOptions<ModelContext> options) : base(options) { }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        // ITEM // succes
        modelBuilder.Entity<Item>()
            .HasKey(i => i.uid);

        modelBuilder.Entity<Item>()
            .Property(i => i.uid)
            .IsRequired();

        modelBuilder.Entity<Item>()
            .HasOne<ItemLine>()
            .WithMany()
            .HasForeignKey(i => i.item_line)
            .IsRequired(false);

        modelBuilder.Entity<Item>()
            .HasOne<ItemType>()
            .WithMany()
            .HasForeignKey(i => i.item_type)
            .IsRequired(false);

        modelBuilder.Entity<Item>()
            .HasOne<ItemGroup>()
            .WithMany()
            .HasForeignKey(i => i.item_group)
            .IsRequired(false);

        modelBuilder.Entity<Item>()
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(i => i.supplier_id);

        // Inventory // succes
        modelBuilder.Entity<Inventory>()
            .HasKey(i => i.id);

        modelBuilder.Entity<Inventory>()
            .Property(i => i.id)
            .IsRequired();

        modelBuilder.Entity<Inventory>()
            .HasOne<Item>()
            .WithOne()
            .HasForeignKey<Inventory>(i => i.item_id);


        // Location // succes
        modelBuilder.Entity<Location>()
            .HasKey(i => i.id);

        modelBuilder.Entity<Location>()
            .Property(i => i.id)
            .IsRequired();



        // Configuratie voor Order
        modelBuilder.Entity<Order>()
            .HasKey(o => o.id);

        modelBuilder.Entity<Order>()
            .Property(o => o.id)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .HasOne<Warehouse>()
            .WithMany()
            .HasForeignKey(o => o.warehouse_id);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
    .HasKey(oi => oi.id);

        modelBuilder.Entity<OrderItem>()
    .Property(oi => oi.order_item_id)
    .HasColumnName("order_item_id"); // Correcte kolomnaam uit de database


        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)          // Zorg dat dit verwijst naar de Order-relatie
            .WithMany(o => o.items)          // Zorg dat Order de lijst van OrderItems bevat
            .HasForeignKey(oi => oi.OrderId); // Zorg dat dit overeenkomt met je databasekolom



        // shipment
        modelBuilder.Entity<Shipment>()
            .HasKey(i => i.id);

        modelBuilder.Entity<Shipment>()
            .Property(i => i.id)
            .IsRequired();

        // modelBuilder.Entity<Shipment>()    => removed to prevent circular fk restraint
        //     .HasOne<Order>()
        //     .WithOne()
        //     .HasForeignKey<Shipment>(i => i.order_id);

        modelBuilder.Entity<Shipment>(shipment =>
        {
            shipment.OwnsMany(s => s.items, item =>
            {
                item.Property(i => i.shipment_item_id).HasColumnName("shipment_item_id");
                item.Property(i => i.amount).HasColumnName("amount");
            });
        });


        // suppleir
        modelBuilder.Entity<Supplier>()
            .HasKey(i => i.id);

        modelBuilder.Entity<Supplier>()
            .Property(i => i.id)
            .IsRequired();

        // //transfer
        modelBuilder.Entity<Transfer>()
            .HasKey(i => i.id);

        modelBuilder.Entity<Transfer>()
            .Property(i => i.id)
            .IsRequired();

        modelBuilder.Entity<Transfer>()
            .HasOne<Location>()
            .WithMany()
            .HasForeignKey(i => i.transfer_from)
            .IsRequired(false);

        modelBuilder.Entity<Transfer>()
            .HasOne<Location>()
            .WithMany()
            .HasForeignKey(i => i.transfer_to)
            .IsRequired(false);

        modelBuilder.Entity<Transfer>(transfer =>
        {
            // is eigenlijk ``OwnsOne`` omdat in de data file elke tranfer maar 1 transfer item opslaat, 
            // maar omdat het wordt opgeslagen in een list moet het OwnsMany zijn. Is niet net als bij 
            // warehouse wnt daar word contact wel gwn opgeslagen ipv in een list.
            transfer.OwnsMany(t => t.items, item =>
            {
                item.Property(i => i.tranfer_item_id).HasColumnName("item_id");
                item.Property(i => i.amount).HasColumnName("amount");
            });
        });

        //warehouse 
        modelBuilder.Entity<Warehouse>()
            .HasKey(i => i.id);

        modelBuilder.Entity<Warehouse>()
            .Property(i => i.id)
            .IsRequired();

        modelBuilder.Entity<Warehouse>(warehouse =>
        {
            warehouse.OwnsOne(w => w.contact, contact =>
            {
                contact.Property(i => i.name).HasColumnName("contact_name");
                contact.Property(i => i.phone).HasColumnName("contact_phone");
                contact.Property(i => i.email).HasColumnName("contact_email");
            });
        });

    }
}
