namespace DA_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entity_person_changed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "PartnerId", c => c.Int());
            AddColumn("dbo.People", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.People", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "BirthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.People", "UserId");
            DropColumn("dbo.People", "PartnerId");
        }
    }
}
