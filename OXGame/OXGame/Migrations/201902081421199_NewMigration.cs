namespace OXGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovesHistories",
                c => new
                    {
                        TurnId = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        Move = c.String(),
                        Player = c.String(),
                    })
                .PrimaryKey(t => t.TurnId);
            
            DropColumn("dbo.GamesDatas", "TurnId");
            DropColumn("dbo.GamesDatas", "Player");
            DropColumn("dbo.GamesDatas", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GamesDatas", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.GamesDatas", "Player", c => c.String());
            AddColumn("dbo.GamesDatas", "TurnId", c => c.Int());
            DropTable("dbo.MovesHistories");
        }
    }
}
