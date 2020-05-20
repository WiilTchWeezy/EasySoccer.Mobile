namespace EasySoccer.Mobile.API.ApiResponses
{
    public class SoccerPitchResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool HasRoof { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int SportTypeId { get; set; }
        public string SportTypeName { get; set; }
        public string Image { get; set; }

        public string ImageName { get; set; }
    }
}
