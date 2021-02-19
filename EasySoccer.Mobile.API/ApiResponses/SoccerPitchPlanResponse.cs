namespace EasySoccer.Mobile.API.ApiResponses
{
    public class SoccerPitchPlanResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }
    }
}
