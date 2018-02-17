
namespace TradingBot.Data.Entities
{
	using Core.Enums;

	public class Account : Entity
	{
        public string Name { get; set; }
        public Exchange Exchange { get; set; }
        public string ApiSettings { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}