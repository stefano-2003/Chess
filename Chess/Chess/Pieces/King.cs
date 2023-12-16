namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.SouthWest,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
        };
        public King(Player color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position pos, Board board)
        {
            if (board.IsEmpty(pos))
            {
                return false;
            }

            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(pos => board.IsEmpty(pos));
        }

        private bool CanCastelKingSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }
            //if(game.CurrentPlayer == Player.White)
            //{
            //    Position rookPos = new Position(8, 7);
            //    Position[] betweenPositions = new Position[2];
            //    betweenPositions[0] = new Position(8, 5);
            //    betweenPositions[1] = new Position(8, 6);

            //    return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
            //}
            //else
            //{
            //    Position rookPos = new Position(0, 7);
            //    Position[] betweenPositions = new Position[2];
            //    betweenPositions[0] = new Position(0, 5);
            //    betweenPositions[1] = new Position(0, 6);

            //    return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);

            //}
            Position rookPos = new Position(from.Row, 7);
            Position[] betweenPositions = new Position[] { new(from.Row, 5), new(from.Row, 6) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastelQueenSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }
            //if (game.CurrentPlayer == Player.White)
            //{
            //    Position rookPos = new Position(0, 0);
            //    Position[] betweenPositions = new Position[3];
            //    betweenPositions[0] = new Position(0, 1);
            //    betweenPositions[1] = new Position(0, 2);
            //    betweenPositions[2] = new Position(0, 3);

            //    return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
            //}
            //else
            //{
            //    Position rookPos = new Position(0,0);
            //    Position[] betweenPositions = new Position[3];
            //    betweenPositions[0] = new Position(0, 1);
            //    betweenPositions[1] = new Position(0, 2);
            //    betweenPositions[2] = new Position(0, 3);

            Position rookPos = new Position(from.Row, 0);
            Position[] betweenPositions = new Position[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach(Direction dir in dirs)
            {
                Position to = from + dir;

                if (!Board.IsInside(to))
                {
                    continue;
                }

                if(board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from,  Board board) 
        {
            foreach(Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }

            if(CanCastelKingSide(from, board))
            {
                yield return new Castle(MoveType.CastlesKS, from);
            }
            if(CanCastelQueenSide(from, board))
            {
                yield return new Castle(MoveType.CasterlQS, from);
            }

        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type ==PieceType.King;
            });    
        }
    }
}
