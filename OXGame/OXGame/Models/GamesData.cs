using OXGame.Interfaces;

namespace OXGame.Models
{
    public class GamesData : IGamesData
    {
        public int Id { get; set; } //id игры
        public string Gameresult { get; set; } //результат игры (выйграл игрок, выйграл компьютер)
        public string GameTurn { get; set; } //ход
    }
}