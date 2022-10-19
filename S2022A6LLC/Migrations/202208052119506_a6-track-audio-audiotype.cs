namespace S2022A6LLC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a6trackaudioaudiotype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "AudioType", c => c.String(maxLength: 200));
            AddColumn("dbo.Tracks", "Audio", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "Audio");
            DropColumn("dbo.Tracks", "AudioType");
        }
    }
}
