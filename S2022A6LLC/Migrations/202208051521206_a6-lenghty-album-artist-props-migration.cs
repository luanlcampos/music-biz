namespace S2022A6LLC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a6lenghtyalbumartistpropsmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "Background", c => c.String());
            AddColumn("dbo.Artists", "Portrayal", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "Portrayal");
            DropColumn("dbo.Albums", "Background");
        }
    }
}
