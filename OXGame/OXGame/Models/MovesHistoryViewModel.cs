using OXGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OXGame.Models
{
    public class MovesHistoryViewModel : MovesHistory, IGamesData
    {
        public int Id { get; set; } //id игры
        public string Gameresult { get; set; } //результат игры (выйграл игрок, выйграл компьютер)
        public string GameTurn { get; set; } //ход
    }
}