using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OXGame.Interfaces
{
    interface IGamesData
    {
        int Id { get; set; } //id игры
        string Gameresult { get; set; } //результат игры (выйграл игрок, выйграл компьютер)
        string GameTurn { get; set; } //ход
    }
}
