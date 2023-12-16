namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason Reason { get; }

        public Result(Player Winner, EndReason Reason)
        {
            this.Winner = Winner;
            this.Reason = Reason;
        }

        public static Result Win(Player Winner)
        {
            return new Result(Winner, EndReason.Checkmate);
        }

        public static Result Draw(EndReason Reason)
        {
            return new Result(Player.None, Reason);
        }
    }
}
