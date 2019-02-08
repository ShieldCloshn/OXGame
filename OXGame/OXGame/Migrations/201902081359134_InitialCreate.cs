namespace OXGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GamesDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gameresult = c.String(),
                        GameTurn = c.String(),
                        TurnId = c.Int(),
                        Player = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GamesDatas");
        }
    }
}
