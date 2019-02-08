namespace OXGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovesHistories", "Id", c => c.Int());
            AddColumn("dbo.MovesHistories", "Gameresult", c => c.String());
            AddColumn("dbo.MovesHistories", "GameTurn", c => c.String());
            AddColumn("dbo.MovesHistories", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovesHistories", "Discriminator");
            DropColumn("dbo.MovesHistories", "GameTurn");
            DropColumn("dbo.MovesHistories", "Gameresult");
            DropColumn("dbo.MovesHistories", "Id");
        }
    }
}
