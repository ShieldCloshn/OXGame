using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web.Mvc;
using OXGame.Models;

namespace KrestikiNulliki.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();


        public ActionResult Index()
        {
            return View();
        }

        //генерация значений для ячеек игрового поля
        [HttpGet]
        public ActionResult GetGameField()
        {
            var gamesdata = context.GamesData.OrderByDescending(t => t.Id).FirstOrDefault();
            var currentgame = new GamesData { };
            context.GamesData.Add(currentgame);
            context.SaveChanges();

            var result = new List<TableRows>();

            for (int i = 1; i < 9; i++)
            {
                var view = new TableRows()
                {
                    FirstColumn = i,
                    SecondColumn = i + 1,
                    ThirdColumn = i + 2,
                };
                result.Add(view);
                i = i + 2;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPlayerMove(string param)
        {
            var getcurrentgameid = context.GamesData.OrderByDescending(t => t.Id).FirstOrDefault();
            var currentgame = context.GamesData.FirstOrDefault(t => t.Id == getcurrentgameid.Id);
            var moves = context.MovesHistory.FirstOrDefault(t => t.GameId == getcurrentgameid.Id);

            var playermove = context.GamesData.FirstOrDefault();
            var board = new List<string>(); //игровое поле
            var table = JsonConvert.DeserializeObject<TableRows>(param); //десериализация данных о поле
            var move = new MovesHistory();

            if (moves == null)
            {
                move = new MovesHistory() { GameId = getcurrentgameid.Id, Player = "игрок", Move = "совершает ход в ячейку " + table.CellId };
                context.MovesHistory.Add(move);
                context.SaveChanges();
            }
            else
            {
                var lastturnId = context.MovesHistory.Max(t => t.TurnId);
                move = new MovesHistory() { GameId = getcurrentgameid.Id, Player = "игрок", Move = "совершает ход в ячейку " + table.CellId };
                context.MovesHistory.Add(move);
                context.SaveChanges();
            }

            var view = new ViewModel();
            foreach (var value in table.FirstRow) //сборка игрового поля
                board.Add(value);

            foreach (var value in table.SecondRow)
                board.Add(value);

            foreach (var value in table.ThirdRow)
                board.Add(value);

            bool result = WinningPositions(board, "X"); //проверка на победу игрока

            if (result == true)
            {
                view.Message = "Вы победили"; //если получается выйграшная комбинация из крестиков, то выйграл игрок
                currentgame.Gameresult = "Победил игрок";
                context.SaveChanges();
                return Json(view, JsonRequestBehavior.AllowGet);
            }

            if (IndexCheck(board).Count == 0) //если нет доступных клеток для хода, тогда обьявляется ничья
            {
                view.Message = "Ничья";
                currentgame.Gameresult = "Ничья";
                context.SaveChanges();
                return Json(view, JsonRequestBehavior.AllowGet);
            }

            var aiMove = new Move();
            aiMove = MiniMax(board, "O"); //вызов минимакс функции для хода компьютера, на неё передаются игровое поле и знак, которым ходит компьютер
            view.CellId = aiMove.CellIndex;

            move = new MovesHistory() { GameId = getcurrentgameid.Id, Player = "комьютер", Move = "совершает ход в ячейку " + aiMove.CellIndex };
            context.MovesHistory.Add(move);
            context.SaveChanges();

            board[Convert.ToInt32(aiMove.CellIndex) - 1] = "O";
            result = WinningPositions(board, "O");

            if (result == true)
            {
                view.Message = "Вы проиграли";
                currentgame.Gameresult = "Игрок проиграл";
                context.SaveChanges();
                return Json(view, JsonRequestBehavior.AllowGet);
            }

            if (IndexCheck(board).Count == 0)
            {
                view.Message = "Ничья";
                currentgame.Gameresult = "Ничья";
                context.SaveChanges();
                return Json(view, JsonRequestBehavior.AllowGet);
            }

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        //функция определения свободных для хода ячеек игрового поля
        public List<string> IndexCheck(List<string> board)
        {
            var checkboard = new List<string>();
            foreach (var value in board)
            {
                if (value != "O" && value != "X")
                    checkboard.Add(value);
            }
            return (checkboard);
        }

        //проверка условия победы
        public bool WinningPositions(List<string> board, string player)
        {
            if ((board[0] == player && board[1] == player && board[2] == player) ||
                (board[3] == player && board[4] == player && board[5] == player) ||
                (board[6] == player && board[7] == player && board[8] == player) ||
                (board[0] == player && board[3] == player && board[6] == player) ||
                (board[1] == player && board[4] == player && board[7] == player) ||
                (board[2] == player && board[5] == player && board[8] == player) ||
                (board[0] == player && board[4] == player && board[8] == player) ||
                (board[2] == player && board[4] == player && board[6] == player))
                return true;
            else
                return false;
        }

        public Move MiniMax(List<string> board, string player)
        {
            var availbleSpots = IndexCheck(board); //получение списка доступных ходов
            var ai = "O"; //компьютер
            var Hplayer = "X"; //игрок

            //проверка на победу игрока, компьютера или на результат ничьей.
            if (WinningPositions(board, Hplayer))
                return (new Move { Score = -10 });

            else if (WinningPositions(board, ai))
                return (new Move { Score = 10 });

            else if (availbleSpots.Count == 0)
                return (new Move { Score = 0 });

            var moves = new List<Move>(); //List ходов
            //цикл доступных клеток
            for (int i = 0; i < availbleSpots.Count; i++)
            {
                var move = new Move //обьект, в котором хранятся данные о ходе текущего игрока
                {
                    CellIndex = board[Convert.ToInt32(availbleSpots[i]) - 1]
                };
                //ход текущего игрока
                board[Convert.ToInt32(availbleSpots[i]) - 1] = player;

                var tempmove = new Move();
                //получение очков текущего игрока, после вызова минимакс функции
                if (player == ai)
                    tempmove = MiniMax(board, Hplayer);
                else
                    tempmove = MiniMax(board, ai);

                move.Score = tempmove.Score;

                //отчистка клетки
                board[Convert.ToInt32(availbleSpots[i]) - 1] = move.CellIndex;
                //запись хода в List ходов
                moves.Add(move);
            }

            var bestMove = new Move();
            int bestScore;
            //поиск хода для ИИ, ход должен принести максимально возможное количество очков
            if (player == ai)
            {
                bestScore = -100;
                foreach (var value in moves)
                {
                    if (value.Score > bestScore)
                    {
                        bestScore = value.Score;
                        bestMove = value;
                    }
                }
            }
            else //если ходит игрок, то необходимо выбрать ход с наименьшим количеством очков
            {
                bestScore = 100;
                foreach (var value in moves)
                {
                    if (value.Score < bestScore)
                    {
                        bestScore = value.Score;
                        bestMove = value;
                    }
                }
            }
            //возвращение выбранного оптимального хода
            return (bestMove);
        }

        //получение данных для таблицы истории игр
        [HttpGet]
        public ActionResult GetGamesHistory(string param)
        {
            var filterParam = JsonConvert.DeserializeObject<MovesHistoryViewModel>(param);
           IQueryable<GamesData> games = context.GamesData;

            if (!String.IsNullOrEmpty(filterParam.Gameresult))
                games = games.Where(t => filterParam.Gameresult.Equals(t.Gameresult));

            if (filterParam.Id != 0)
                games = games.Where(t => t.Id == filterParam.Id);

            var listOfGames = games.ToList();
            var result = new List<MovesHistoryViewModel>();
            var check = context.MovesHistory.ToList();

            foreach (var game in listOfGames)
            {
                var gamesHistory = context.MovesHistory.Where(t => t.GameId == game.Id).ToList();
                foreach (var gameHistory in gamesHistory)
                {
                    var gameMove = new MovesHistoryViewModel
                    {
                        GameId = gameHistory.GameId,
                        Move = gameHistory.Move,
                        Player = gameHistory.Player,
                        Gameresult = game.Gameresult
                    };
                    result.Add(gameMove);
                }
            }

            var sortedResult = result.OrderByDescending(t => t.GameId);

            return Json(sortedResult, JsonRequestBehavior.AllowGet);
        }

        //фукнция получения id и результатов игр для заполнения droplist в view, далее эти данные будут использоватся для поиска данных об играх в бд.
        [HttpGet]
        public ActionResult GetGamesData()
        {
            var games = context.GamesData.ToList();

            return Json(games, JsonRequestBehavior.AllowGet);
        }
    }
}